using System;
using System.Numerics;
using Windows.Foundation;

namespace LibraProgramming.Windows.EcoSystem.GameEngine
{
    internal sealed class PositioningSystem : IPositioningSystem
    {
        private readonly Size size;
        private readonly Coordinates cells;
        private readonly Point grid;

        public PositioningSystem(Size size, Coordinates cells)
        {
            this.size = size;
            this.cells = cells;

            grid = new Point(size.Width / cells.X, size.Height / cells.Y);
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
                y += grid.Y * (coordinates.Y - 1);
            }

            return new Vector2((float)x, (float)y);
        }

        public Coordinates GetRandomCoordinates()
        {
            return new Coordinates(cells.X / 2, cells.Y / 2);
        }

        public bool IsFree(Coordinates coordinates)
        {
            return true;
        }

        public bool IsObstacle(Coordinates coordinates)
        {
            return false;
        }
    }
}
