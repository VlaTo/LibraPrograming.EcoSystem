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
using System.Numerics;
using Windows.UI.Xaml;
using Windows.System;
using Windows.Devices.Input;

namespace LibraProgramming.Windows.EcoSystem.GameEngine
{
    /// <summary>
    /// 
    /// </summary>
    public sealed partial class EcoSystemController : IEcoSystemController
    {
        private const int GenomeLength = 64;
        private const int AlphaBotsCount = 8;
        private const int NestedBotsCount = 8;

        private readonly CanvasAnimatedControl control;
        private readonly IOpCodeGenerator generator;
        private readonly IGenomeProducer genomeProducer;
        private readonly ICreatePositionProvider positionProvider;
        private readonly IBeetleBotFactory factory;
        private readonly IEnumerable<Coordinates> obstacles;
        private readonly Point cellSize;
        private readonly Coordinates map;
        private readonly Random random;

        private LandscapeGrid landscape;
        private ImmutableList<BeetleBot> beetleBots;
        private IDisposable subscription;
        private CellType[,] cells;

        public IScene Scene
        {
            get;
            private set;
        }

        public ISubject<BeetleBotMessage> BeetleBotMessage
        {
            get;
        }

        public ILand Land
        {
            get;
        }

        public int Epoch
        {
            get;
            private set;
        }

        [PrefferedConstructor]
        public EcoSystemController(CanvasAnimatedControl control, IEnumerable<Coordinates> obstacles)
        {
            this.control = control;
            this.obstacles = obstacles;

            random = new Random();
            generator = new OpCodeGenerator();
            beetleBots = ImmutableList<BeetleBot>.Empty;
            cellSize = new Point(control.Size.Width / map.X, control.Size.Height / map.Y);
            genomeProducer = new GenomeProducer(generator, GenomeLength, 1);
            positionProvider = new PositionProvider(obstacles, map);
            factory = new BeetleBotFactory(positionProvider, genomeProducer);

            Epoch = 0;
            Scene = new Scene(this);
            BeetleBotMessage = new BeetleBotSubject();
            Land = new Landscape(this, new MapSize(60, 40));

            control.PointerEntered += OnPointerEntered;
            control.PointerExited += OnPointerExited;
            control.PointerPressed += OnPointerPressed;
            control.PointerReleased += OnPointerReleased;
            control.PointerMoved += OnPointerMoved;

            subscription = BeetleBotMessage.Subscribe(new BeetleBotObserver(this));
        }

        /// <summary>
        /// 
        /// </summary>
        public void Shutdown()
        {
            control.PointerEntered -= OnPointerEntered;
            control.PointerExited -= OnPointerExited;
            control.PointerPressed -= OnPointerPressed;
            control.PointerReleased -= OnPointerReleased;
            control.PointerMoved -= OnPointerMoved;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reason"></param>
        /// <returns></returns>
        public Task InitializeAsync(CanvasCreateResourcesReason reason)
        {
            landscape = new LandscapeGrid(control.Size, new Size(20.0d, 20.0d), Colors.LightGray);

            Scene.Children.Add(landscape);

            foreach (var position in obstacles)
            {
                cells[position.X, position.Y] = CellType.Free;
            }

            var genomes = new List<IGenome>(AlphaBotsCount);

            for(var index = 0; index < AlphaBotsCount; index++)
            {
                genomes.Add(genomeProducer.CreateGenome());
            }

            StartEpoch(genomes);

            return Scene.CreateResourcesAsync(control, reason);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elapsed"></param>
        public void Update(TimeSpan elapsed)
        {
            Scene.Update(elapsed);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Coordinates GetCoordinates(Vector2 position)
        {
            var x = Convert.ToByte(position.X / cellSize.X);
            var y = Convert.ToByte(position.Y / cellSize.Y);

            return new Coordinates(x, y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coordinates"></param>
        /// <returns></returns>
        public Vector2 GetPosition(Coordinates coordinates)
        {
            var x = cellSize.X * 0.5d;
            var y = cellSize.Y * 0.5d;

            if (0 < coordinates.X)
            {
                x += (cellSize.X * coordinates.X);
            }

            if (0 < coordinates.Y)
            {
                y += (cellSize.Y * coordinates.Y);
            }

            return new Vector2((float)x, (float)y);
        }

        /// <summary>
        /// 
        /// </summary>
        private IEnumerable<IGenome> StopEpoch()
        {
            var genomes = new List<IGenome>();

            while (false == beetleBots.IsEmpty)
            {
                var bot = beetleBots[0];

                RemoveBeetBot(bot);

                genomes.Add(bot.Genome);
            }

            return genomes;
        }

        /// <summary>
        /// 
        /// </summary>
        private void StartEpoch(IEnumerable<IGenome> genomes)
        {
            const int mutations = 1;

            foreach (var genome in genomes)
            {
                var alpha = factory.CreateBeetleBot(genome);

                beetleBots = beetleBots.Add(alpha);
                Scene.Children.Add(alpha);

                for (var count = 0; count < (NestedBotsCount - mutations); count++)
                {
                    var child = factory.CreateBeetleBot(alpha.Genome);

                    beetleBots = beetleBots.Add(child);
                    Scene.Children.Add(child);
                }

                for(var count = 0; count < mutations; count++)
                {
                    var mutatedGenome = genomeProducer.MutateGenome(alpha.Genome);
                    var child = factory.CreateBeetleBot(mutatedGenome);

                    beetleBots = beetleBots.Add(child);
                    Scene.Children.Add(child);
                }   
            }
        }

        private void RemoveBeetBot(BeetleBot beetleBot)
        {
            beetleBots = beetleBots.Remove(beetleBot);
            Scene.Children.Remove(beetleBot);

            if (CellType.Occupied == cells[beetleBot.Coordinates.X, beetleBot.Coordinates.Y])
            {
                cells[beetleBot.Coordinates.X, beetleBot.Coordinates.Y] = CellType.Free;
            }
        }

        private void DoBeetleBotDies(BeetleBot beetleBot)
        {
            RemoveBeetBot(beetleBot);

            if (AlphaBotsCount == beetleBots.Count)
            {
                var genomes = StopEpoch();

                Epoch++;

                StartEpoch(genomes);
            }
        }

        private void DoBeetleBotMoves(BeetleBot beetleBot, Coordinates origin, Coordinates destination)
        {
            if (null != origin)
            {
                if (CellType.Occupied == cells[origin.X, origin.Y])
                {
                    cells[origin.X, origin.Y] = CellType.Free;
                }
            }

            if (null == destination)
            {
                return;
            }

            cells[destination.X, destination.Y] = CellType.Occupied;
        }

        private void OnPointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (PointerDeviceType.Mouse != e.Pointer.PointerDeviceType)
            {
                return;
            }

            var point = e.GetCurrentPoint((UIElement)sender);

            landscape.Cursor = GetCoordinates(point.Position.ToVector2());

            if (point.Properties.IsLeftButtonPressed)
            {
                MarkCell(landscape.Cursor, VirtualKeyModifiers.Shift == e.KeyModifiers);
            }
        }

        private void OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            var point = e.GetCurrentPoint((UIElement)sender);
            landscape.Cursor = GetCoordinates(point.Position.ToVector2());
        }

        private void OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            landscape.Cursor = null;
        }

        private void OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (PointerDeviceType.Mouse == e.Pointer.PointerDeviceType)
            {
                var point = e.GetCurrentPoint((UIElement)sender);

                if (point.Properties.IsLeftButtonPressed)
                {
                    var cursor = GetCoordinates(point.Position.ToVector2());

                    MarkCell(cursor, VirtualKeyModifiers.Shift == e.KeyModifiers);
                }
            }
        }

        private void OnPointerReleased(object sender, PointerRoutedEventArgs e)
        {
        }

        private void MarkCell(Coordinates coordinates, bool forceFood)
        {
            var value = cells[coordinates.X, coordinates.Y];

            if (CellType.Free == value)
            {
                cells[coordinates.X, coordinates.Y] = forceFood ? CellType.Food : CellType.Wall;
            }
            else
            {
                cells[coordinates.X, coordinates.Y] = CellType.Free;
            }
        }

        public void Stop()
        {
            throw new NotImplementedException();
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

        /// <summary>
        /// 
        /// </summary>
        private class PositionProvider : ICreatePositionProvider
        {
            private readonly IList<Coordinates> free;
            private readonly Random random;

            public PositionProvider(IEnumerable<Coordinates> obstacles, Coordinates map)
            {
                random = new Random();
                free = new List<Coordinates>();

                var occupied = new List<Coordinates>(obstacles);

                for (var y = 0; y < map.Y; y++)
                {
                    for (var x = 0; x < map.X; x++)
                    {
                        var position = new Coordinates(x, y);

                        if (occupied.Contains(position))
                        {
                            continue;
                        }

                        free.Add(position);
                    }
                }
            }

            public Coordinates CreatePosition()
            {
                if (0 == free.Count)
                {
                    throw new Exception();
                }

                var index = random.Next(free.Count);
                var position = free[index];

                free.RemoveAt(index);

                return position;
            }
        }
    }
}