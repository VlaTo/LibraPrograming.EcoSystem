using System;
using LibraProgramming.Windows.Games.Towers.Core.ServiceContainer;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System.Threading.Tasks;
using Windows.UI.Xaml.Input;
using System.Collections.Immutable;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
    public class EpochEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public int Epoch
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="epoch"></param>
        public EpochEventArgs(int epoch)
        {
            Epoch = epoch;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class EpochStartedEventArgs : EpochEventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyCollection<IGenome> Genomes
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="epoch"></param>
        public EpochStartedEventArgs(int epoch, IReadOnlyCollection<IGenome> genomes)
            : base(epoch)
        {
            Genomes = genomes;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class EcoSystemController : IEcoSystemController
    {
        private const int GenomeLength = 64;
        private const int AlphaBotsCount = 8;
        private const int NestedBotsCount = 8;
        private const int FoodCount = 50;
        private const int PoisonCount = 15;

        private readonly CanvasAnimatedControl control;
        private readonly IOpCodeGenerator generator;
        private readonly IGenomeProducer genomeProducer;
        private readonly IEnumerable<Coordinates> obstacles;
        private readonly Size cellSize;
        private readonly MapSize mapSize;
        private readonly CellType[,] map;

        private LandscapeGrid grid;
        private ImmutableList<BeetleBot> bots;
        private int foodCount;
        private int poisonCount;

        /// <inheritdoc />
        public IScene Scene
        {
            get;
        }

        public int Epoch
        {
            get;
            private set;
        }

        /// <inheritdoc />
        public event EventHandler<EpochStartedEventArgs> EpochStarted;

        [PrefferedConstructor]
        public EcoSystemController(CanvasAnimatedControl control, MapSize mapSize, IEnumerable<Coordinates> obstacles)
        {
            this.control = control;
            this.obstacles = obstacles;
            this.mapSize = mapSize;

            generator = new OpCodeGenerator();
            bots = ImmutableList<BeetleBot>.Empty;
            map = new CellType[mapSize.Width, mapSize.Height];
            cellSize = new Size(control.Size.Width / mapSize.Width, control.Size.Height / mapSize.Height);
            genomeProducer = new GenomeProducer(generator, GenomeLength, 1);

            Epoch = 0;
            Scene = new Scene(this);

            control.PointerEntered += OnPointerEntered;
            control.PointerExited += OnPointerExited;
            control.PointerPressed += OnPointerPressed;
            control.PointerReleased += OnPointerReleased;
            control.PointerMoved += OnPointerMoved;
        }

        /// <inheritdoc />
        public void Shutdown()
        {
            control.PointerEntered -= OnPointerEntered;
            control.PointerExited -= OnPointerExited;
            control.PointerPressed -= OnPointerPressed;
            control.PointerReleased -= OnPointerReleased;
            control.PointerMoved -= OnPointerMoved;
        }

        /// <inheritdoc />
        public Task InitializeAsync(CanvasCreateResourcesReason reason)
        {
            grid = new LandscapeGrid(control.Size, new Size(20.0d, 20.0d), Colors.LightGray);

            Scene.Children.Add(grid);

            foreach (var position in obstacles)
            {
                map[position.X, position.Y] = (CellType.Occupied | CellType.Wall);
            }

            var genomes = new List<IGenome>(AlphaBotsCount);

            for(var index = 0; index < AlphaBotsCount; index++)
            {
                genomes.Add(genomeProducer.CreateGenome());
            }

            StartEpoch(genomes);

            return Scene.CreateResourcesAsync(control, reason);
        }

        /// <inheritdoc />
        public void Update(TimeSpan elapsed)
        {

            Scene.Update(elapsed);
        }

        /// <inheritdoc />
        public Coordinates GetCoordinates(Vector2 position)
        {
            var x = Convert.ToByte(position.X / cellSize.Width);
            var y = Convert.ToByte(position.Y / cellSize.Height);

            return new Coordinates(x, y);
        }

        /// <inheritdoc />
        public Vector2 GetPosition(Coordinates coordinates)
        {
            var x = cellSize.Width * 0.5d;
            var y = cellSize.Height * 0.5d;

            if (0 < coordinates.X)
            {
                x += (cellSize.Width * coordinates.X);
            }

            if (0 < coordinates.Y)
            {
                y += (cellSize.Height * coordinates.Y);
            }

            return new Vector2((float)x, (float)y);
        }

        /// <inheritdoc />
        public void Occupy(Coordinates coordinates, bool occupy)
        {
            if (null == coordinates)
            {
                return;
            }

            if (occupy)
            {
                map[coordinates.X, coordinates.Y] |= CellType.Occupied;
            }
            else
            {
                map[coordinates.X, coordinates.Y] &= ~CellType.Occupied;
            }
        }

        /// <inheritdoc />
        public bool IsOccupied(Coordinates coordinates)
        {
            if (0 > coordinates.X || coordinates.X >= mapSize.Width)
            {
                return false;
            }

            if (0 > coordinates.Y || coordinates.Y >= mapSize.Height)
            {
                return false;
            }

            return CellType.Occupied == (map[coordinates.X, coordinates.Y] & CellType.OccupationMask);
        }

        /// <inheritdoc />
        public CellType GetAttribute(Coordinates coordinates)
        {
            return map[coordinates.X, coordinates.Y] & CellType.AttributeMask;
        }

        /// <inheritdoc />
        public bool Eat(Coordinates coordinates, out bool poisoned)
        {
            const CellType mask = CellType.AttributeMask & ~CellType.Wall;
            var cell = map[coordinates.X, coordinates.Y] & mask;

            map[coordinates.X, coordinates.Y] &= ~(CellType.Food | CellType.Poison);
            poisoned = false;

            if (CellType.Food == cell)
            {
                --foodCount;

                UpdateFoodAmount();

                return true;
            }

            if (CellType.Poison == cell)
            {
                poisoned = true;
                --poisonCount;

                UpdateFoodAmount();

                return true;
            }

            return false;
        }

        /// <inheritdoc />
        public void DoBeetleBotDies(BeetleBot bot)
        {
            RemoveBeetBot(bot);
            Occupy(bot.Coordinates, false);

            if (AlphaBotsCount < bots.Count)
            {
                return;
            }

            var genomes = StopEpoch();

            Epoch++;

            StartEpoch(genomes);
        }

        private IEnumerable<IGenome> StopEpoch()
        {
            var genomes = new List<IGenome>();

            while (false == bots.IsEmpty)
            {
                var bot = bots[0];

                RemoveBeetBot(bot);

                genomes.Add(bot.Genome);
            }

            return genomes;
        }

        private void StartEpoch(IEnumerable<IGenome> genomes)
        {
            /*const int mutations = 1;

            var positionProvider = new PositionProvider(obstacles, mapSize);
            var factory = new BeetleBotFactory(positionProvider, genomeProducer);

            var list = new List<IGenome>(genomes);
            DoEpochStarted(new EpochStartedEventArgs(Epoch,new ReadOnlyCollection<IGenome>(list)));

            foreach (var genome in genomes)
            {
                Debug.WriteLine($"Genome: mutations: {genome.Mutations.Count}");

                var alpha = factory.CreateBeetleBot(genome);

                bots = bots.Add(alpha);
                Scene.Children.Add(alpha);

                for (var count = 0; count < (NestedBotsCount - mutations); count++)
                {
                    var child = factory.CreateBeetleBot(alpha.Genome);

                    bots = bots.Add(child);
                    Scene.Children.Add(child);
                }

                for(var count = 0; count < mutations; count++)
                {
                    var mutatedGenome = genomeProducer.MutateGenome(alpha.Genome);
                    var child = factory.CreateBeetleBot(mutatedGenome);

                    bots = bots.Add(child);
                    Scene.Children.Add(child);
                }   
            }

            UpdateFoodAmount();*/

            const int mutations = 1;

            var positionProvider = new PositionProvider(obstacles, mapSize);
            var factory = new BeetleBotFactory(positionProvider, genomeProducer);

            var list = new List<IGenome>(genomes);
            DoEpochStarted(new EpochStartedEventArgs(Epoch,new ReadOnlyCollection<IGenome>(list)));

            foreach (var genome in genomes)
            {
                Debug.WriteLine($"Genome: mutations: {genome.Mutations.Count}");

                var alpha = factory.CreateBeetleBot(genome);

                bots = bots.Add(alpha);
                Scene.Children.Add(alpha);

                break;
            }

            UpdateFoodAmount();
        }

        private void RemoveBeetBot(BeetleBot bot)
        {
            bots = bots.Remove(bot);
            Scene.Children.Remove(bot);

            if (null == bot.Coordinates)
            {
                return;
            }

            map[bot.Coordinates.X, bot.Coordinates.Y] &= CellType.AttributeMask;
        }

        private void UpdateFoodAmount()
        {
            if (FoodCount <= foodCount)
            {
                return;
            }

            var positionProvider = new PositionProvider(obstacles, mapSize);

            while (FoodCount > foodCount)
            {
                var position = positionProvider.CreatePosition();

                if (CellType.Free != (map[position.X, position.Y] & CellType.AttributeMask))
                {
                    continue;
                }

                map[position.X, position.Y] |= CellType.Food;
                ++foodCount;
            }

            while (PoisonCount > poisonCount)
            {
                var position = positionProvider.CreatePosition();

                if (CellType.Free != (map[position.X, position.Y] & CellType.AttributeMask))
                {
                    continue;
                }

                map[position.X, position.Y] |= CellType.Poison;
                ++poisonCount;
            }
        }

        private void DoEpochStarted(EpochStartedEventArgs e)
        {
            EpochStarted?.Invoke(this, e);
        }

        private void OnPointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (PointerDeviceType.Mouse != e.Pointer.PointerDeviceType)
            {
                return;
            }

            var point = e.GetCurrentPoint((UIElement)sender);

            grid.Cursor = GetCoordinates(point.Position.ToVector2());

            if (point.Properties.IsLeftButtonPressed)
            {
                MarkCell(grid.Cursor, VirtualKeyModifiers.Shift == e.KeyModifiers);
            }
        }

        private void OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            var point = e.GetCurrentPoint((UIElement)sender);
            grid.Cursor = GetCoordinates(point.Position.ToVector2());
        }

        private void OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            grid.Cursor = null;
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
            var value = map[coordinates.X, coordinates.Y];

            if (CellType.Free == value)
            {
                map[coordinates.X, coordinates.Y] = forceFood ? CellType.Food : (CellType.Occupied | CellType.Wall);

                if (forceFood)
                {
                    ++foodCount;
                }
            }
            else
            {
                map[coordinates.X, coordinates.Y] = CellType.Free;
            }
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        private class PositionProvider : ICreatePositionProvider
        {
            private readonly IList<Coordinates> free;
            private readonly Random random;

            public PositionProvider(IEnumerable<Coordinates> obstacles, MapSize mapSize)
            {
                random = new Random();
                free = new List<Coordinates>();

                var occupied = new List<Coordinates>(obstacles);

                for (var y = 0; y < mapSize.Height; y++)
                {
                    for (var x = 0; x < mapSize.Width; x++)
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