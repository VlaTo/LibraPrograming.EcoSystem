using System;
using LibraProgramming.Windows.Games.Towers.Core.ServiceContainer;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System.Threading.Tasks;
using Windows.UI.Xaml.Input;
using System.Collections.Immutable;

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
        private readonly IPositioningSystem positioningSystem;
        private IScene scene;
        private IImmutableList<BeetleBot> beetleBots;

        public IScene Scene
        {
            get
            {
                return scene;
            }
            set
            {
                scene = value;
            }
        }

        [PrefferedConstructor]
        public EcoSystemController(CanvasAnimatedControl control)
        {
            this.control = control;

            positioningSystem = new PositioningSystem(control.Size, new Coordinates(60, 40));
            generator = new OpCodeGenerator();
            genomeProducer = new GenomeProducer(generator, GenomeLength);
            factory = new BeetleBotFactory(genomeProducer, positioningSystem);
            beetleBots = ImmutableList<BeetleBot>.Empty;

            control.PointerEntered += OnPointerEntered;
            control.PointerExited += OnPointerExited;
            control.PointerPressed += OnPointerPressed;
            control.PointerMoved += OnPointerMoved;
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
            for (var primary = 0; primary < PrimaryBotsCount; primary++)
            {
                var primaryBot = factory.CreateBeetleBot();

                AddBeetleBot(primaryBot);

                for (var index = 0; index < NestedBotsCount; index++)
                {
                    var nestedBot = factory.CreateBeetleBot(primaryBot.Genome);

                    AddBeetleBot(nestedBot);
                }
            }

            return scene.CreateResourcesAsync(control, reason);
        }

        public void Update(TimeSpan elapsed)
        {
            scene.Update(elapsed);
        }

        private void AddBeetleBot(BeetleBot beetleBot)
        {
            beetleBots = beetleBots.Add(beetleBot);
            scene.Children.Add(beetleBot);
            beetleBot.Dies += OnBeetleBotDies;
        }

        private void OnBeetleBotDies(object sender, BeetleBotEventArgs e)
        {
            var beetleBot = (BeetleBot)sender;
            beetleBots = beetleBots.Remove(beetleBot);
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
        /*private class GameCoordinatesSystem : ICoordinatesSystem
        {
            private const float CellHeight = 10.0f;
            private const float CellWidth = 10.0f;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="position"></param>
            /// <returns></returns>
            public Vector2 GetPoint(Position position)
            {
                var point = new Vector2(CellWidth / 2.0f, CellHeight / 2.0f);

                if (position.Column > 0)
                {
                    point.X += CellWidth * (position.Column - 1);
                }

                if (position.Row > 0)
                {
                    point.Y += CellHeight * (position.Row - 1);
                }

                return point;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="point"></param>
            /// <returns></returns>
            public Position GetPosition(Vector2 point)
            {
                var column = (int) (point.X / CellWidth);
                var row = (int) (point.Y / CellHeight);

                if (Math.IEEERemainder(point.X, CellWidth) >= Single.Epsilon)
                {
                    column++;
                }

                if (Math.IEEERemainder(point.Y, CellHeight) >= Single.Epsilon)
                {
                    row++;
                }

                return new Position(column, row);
            }
        }*/
    }
}