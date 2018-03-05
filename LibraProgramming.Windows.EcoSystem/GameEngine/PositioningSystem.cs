using System;
using System.Numerics;
using Windows.Foundation;

namespace LibraProgramming.Windows.EcoSystem.GameEngine
{
    internal sealed class PositioningSystem : IPositioningSystem
    {
        private readonly Size size;
        private readonly Coordinates cell;
        private readonly Point grid;

        public PositioningSystem(Size size, Coordinates cell)
        {
            this.size = size;
            this.cell = cell;

            grid = new Point(size.Width / cell.X, size.Height / cell.Y);
        }

        public Coordinates GetCoordinates(Vector2 position)
        {
            throw new NotImplementedException();
        }

        public Vector2 GetPosition(Coordinates coordinates)
        {
            var x = grid.X * 0.5d;
            var y = grid.Y * 0.5d;
            
            if (0 < coordinates.X)
            {
                x += grid.X * (coordinates.X - 1);
            }

            if (0 < coordinates.Y)
            {
                x += grid.Y * (coordinates.Y - 1);
            }

            return new Vector2((float)x, (float)y);
        }
    }
}
