using System;

namespace LibraProgramming.Windows.EcoSystem.GameEngine
{
    public partial class EcoSystemController
    {
        private class Landscape : ILand
        {
            internal const byte Free = 0;
            internal const byte Occupied = 1;
            internal const byte Wall = 2;

            private readonly EcoSystemController controller;
            private readonly byte[,] land;

            public MapSize Size
            {
                get;
            }

            public BeetleBot this[Coordinates coordinates]
            {
                get
                {
                    if (false == EnsureCoordinates(coordinates))
                    {
                        return null;
                    }

                    if (Free == land[coordinates.X, coordinates.Y])
                    {
                        return null;
                    }

                    return controller.beetleBots.Find(bot => bot.Coordinates == coordinates);
                }
                set
                {
                    if (EnsureCoordinates(coordinates) && Occupied == land[coordinates.X, coordinates.Y])
                    {
                        land[coordinates.X, coordinates.Y] = Free;
                    }

                    if (null == value || false == EnsureCoordinates(value.Coordinates))
                    {
                        return;
                    }

                    land[value.Coordinates.X, value.Coordinates.Y] = Occupied;
                }
            }

            public Landscape(EcoSystemController controller, MapSize mapSize)
            {
                this.controller = controller;

                land = new byte[mapSize.Width, mapSize.Height];

                Size = mapSize;
            }

            private bool EnsureCoordinates(Coordinates coordinates)
            {
                if (null == coordinates)
                {
                    return false;
                }

                if (0 > coordinates.X || coordinates.X >= controller.map.X)
                {
                    return false;
                }

                if (0 > coordinates.Y || coordinates.Y >= controller.map.Y)
                {
                    return false;
                }

                return true;
            }
        }
    }
}
