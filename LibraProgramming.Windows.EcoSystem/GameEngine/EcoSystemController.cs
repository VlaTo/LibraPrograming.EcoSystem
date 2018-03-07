using System;
using LibraProgramming.Windows.Games.Towers.Core.ServiceContainer;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System.Threading.Tasks;
using Windows.UI.Xaml.Input;
using System.Collections.Immutable;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI;

namespace LibraProgramming.Windows.EcoSystem.GameEngine
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class EcoSystemController : IEcoSystemController
    {
        private const int GenomeLength = 64;
        private const int PrimaryBotsCount = 8;
        private const int NestedBotsCount = 8;

        private readonly CanvasAnimatedControl control;
        private readonly IGenomeProducer genomeProducer;
        private readonly IBeetleBotFactory factory;
        private readonly IOpCodeGenerator generator;
        private ImmutableList<BeetleBot> beetleBots;
        //private ImmutableList<Coordinates> occupied;
        //private ImmutableList<Coordinates> free;
        private IDisposable subscription;
        private byte[,] cells;

        public IScene Scene
        {
            get;
            private set;
        }

        public ISubject<BeetleBotMessage> BeetleBotMessage
        {
            get;
        }

        public IPositioningSystem Positioning
        {
            get;
        }

        [PrefferedConstructor]
        public EcoSystemController(CanvasAnimatedControl control)
        {
            this.control = control;

            cells = new byte[40, 60];
            beetleBots = ImmutableList<BeetleBot>.Empty;

            Scene = new Scene(this);
            Positioning = new PositioningSystem(control.Size, new Coordinates(60, 40));
            BeetleBotMessage = new BeetleBotSubject();

            generator = new OpCodeGenerator();
            genomeProducer = new GenomeProducer(generator, GenomeLength);
            factory = new BeetleBotFactory(genomeProducer, Positioning);

            control.PointerEntered += OnPointerEntered;
            control.PointerExited += OnPointerExited;
            control.PointerPressed += OnPointerPressed;
            control.PointerMoved += OnPointerMoved;

            subscription = BeetleBotMessage.Subscribe(new BeetleBotObserver(this));
        }

        public void Shutdown()
        {
            control.PointerEntered -= OnPointerEntered;
            control.PointerExited -= OnPointerExited;
            control.PointerPressed -= OnPointerPressed;
            control.PointerMoved -= OnPointerMoved;
        }

        public Task InitializeAsync(CanvasCreateResourcesReason reason)
        {
            var landscapeGrid = new LandscapeGrid(control.Size, new Size(20.0d, 20.0d), Colors.LightGray);

            Scene.Children.Add(landscapeGrid);

            for(var y = 0; y < 60; y++)
            {
                cells[0, y] = Byte.MaxValue;
                cells[39, y] = Byte.MaxValue;
            }

            for(var x = 1; x < 39; x++)
            {
                cells[x, 0] = Byte.MaxValue;
                cells[x, 59] = Byte.MaxValue;
            }

            /*for (var primary = 0; primary < PrimaryBotsCount; primary++)
            {
                var primaryBot = factory.CreateBeetleBot();

            beetleBots = beetleBots.Add(beetleBot);
            Scene.Children.Add(beetleBot);

                for (var index = 0; index < NestedBotsCount; index++)
                {
                    var nestedBot = factory.CreateBeetleBot(primaryBot.Genome);

            beetleBots = beetleBots.Add(beetleBot);
            Scene.Children.Add(beetleBot);
                }
            }*/

            var bot = factory.CreateBeetleBot();

            beetleBots = beetleBots.Add(bot);
            Scene.Children.Add(bot);

            return Scene.CreateResourcesAsync(control, reason);
        }

        public void Update(TimeSpan elapsed)
        {
            Scene.Update(elapsed);
        }

        public bool IsFreeCell(Coordinates coordinates)
        {
            return 0 == cells[coordinates.Y, coordinates.X];
        }

        public bool IsObstacleInCell(Coordinates coordinates)
        {
            return Byte.MaxValue == cells[coordinates.Y, coordinates.X];
        }

        private void DoBeetleBotDies(BeetleBot beetleBot)
        {
            beetleBots = beetleBots.Remove(beetleBot);

            if (1 == cells[beetleBot.Coordinates.Y, beetleBot.Coordinates.X])
            {
                cells[beetleBot.Coordinates.Y, beetleBot.Coordinates.X] = 0;
            }
        }

        private void DoBeetleBotMoves(BeetleBot beetleBot, Coordinates origin, Coordinates destination)
        {
            if (null != origin)
            {
                if (1 == cells[origin.Y, origin.X])
                {
                    cells[origin.Y, origin.X] = 0;
                }
            }

            if (null == destination)
            {
                return;
            }

            cells[destination.Y, destination.X] = 1;
        }

        private void OnPointerMoved(object sender, PointerRoutedEventArgs e)
        {
        }

        private void OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
        }

        private void OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
        }

        private void OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        private sealed class BeetleBotSubject : ISubject<BeetleBotMessage>
        {
            public ImmutableList<IObserver<BeetleBotMessage>> Observers
            {
                get;
                private set;
            }

            public BeetleBotSubject()
            {
                Observers = ImmutableList<IObserver<BeetleBotMessage>>.Empty;
            }

            public IDisposable Subscribe(IObserver<BeetleBotMessage> observer)
            {
                if (null == observer)
                {
                    throw new ArgumentNullException();
                }

                if (false == Observers.Contains(observer))
                {
                    Observers = Observers.Add(observer);
                }

                return new Subscription<IObserver<BeetleBotMessage>>(observer, DoRemoveObserver);
            }

            public void OnNext(BeetleBotMessage value)
            {
                ForEach(observer => observer.OnNext(value));
            }

            public void OnCompleted()
            {
                ForEach(observer => observer.OnCompleted());
            }

            public void OnError(Exception error)
            {
                ForEach(observer => observer.OnError(error));
            }

            private void DoRemoveObserver(IObserver<BeetleBotMessage> observer)
            {
                if (false == Observers.Contains(observer))
                {
                    return;
                }

                Observers = Observers.Remove(observer);
            }

            private void ForEach(Action<IObserver<BeetleBotMessage>> action)
            {
                if (null == action)
                {
                    throw new ArgumentNullException(nameof(action));
                }

                foreach(var observer in Observers)
                {
                    action.Invoke(observer);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private class BeetleBotObserver : IObserver<BeetleBotMessage>
        {
            private readonly EcoSystemController controller;

            public BeetleBotObserver(EcoSystemController controller)
            {
                this.controller = controller;
            }

            public void OnCompleted()
            {
                throw new NotImplementedException();
            }

            public void OnError(Exception error)
            {
                throw new NotImplementedException();
            }

            public void OnNext(BeetleBotMessage value)
            {
                switch (value.MessageType)
                {
                    case BeetleBotMessageType.Move:
                    {
                        var message = (BeetleBotMoveMessage)value;

                        controller.DoBeetleBotMoves(message.BeetleBot, message.Origin, message.Destination);

                        break;
                    }

                    case BeetleBotMessageType.Dies:
                    {
                        var message = (BeetleBotDiesMessage)value;

                        controller.DoBeetleBotDies(message.BeetleBot);

                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TArgument"></typeparam>
        private sealed class Subscription<TArgument> : IDisposable
        {
            private readonly TArgument arg;
            private readonly Action<TArgument> action;

            public Subscription(TArgument arg, Action<TArgument> action)
            {
                this.arg = arg;
                this.action = action;
            }

            public void Dispose()
            {
                action.Invoke(arg);
            }
        }
    }
}