using System;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.UI;
using Windows.Foundation;
using Windows.UI;

namespace LibraProgramming.Windows.EcoSystem.GameEngine
{
    public sealed class LandscapeGrid : SceneNode
    {
        private readonly Color gridColor;
        private readonly Size size;
        private readonly Size cellSize;
        private CanvasSolidColorBrush gridColorBrush;

        public Coordinates Cursor
        {
            get;
            set;
        }

        public LandscapeGrid(Size size, Size cellSize, Color gridColor)
        {
            this.size = size;
            this.cellSize = cellSize;
            this.gridColor = gridColor;
        }

        public override Task CreateResourcesAsync(ICanvasResourceCreatorWithDpi creator, CanvasCreateResourcesReason reason)
        {
            gridColorBrush = new CanvasSolidColorBrush(creator, gridColor);

            return Task.CompletedTask;
        }

        public override void Draw(CanvasDrawingSession session)
        {
            const float thickness = 1.0f;

            var height = Convert.ToSingle(size.Height);
            var width = Convert.ToSingle(size.Width);
            var dx = Convert.ToSingle(cellSize.Width);
            var dy = Convert.ToSingle(cellSize.Height);

            DrawMeridians(session, width, height, thickness);
            DrawParallels(session, width, height, thickness);

            var cell = new Size(cellSize.Width - thickness, cellSize.Height - thickness);
            var cursor = Cursor;

            for (var y = 0; y < 40; y++)
            {
                for (var x = 0; x < 60; x++)
                {
                    var coordinates = new Coordinates(x, y);
                    var ct = CellType.Free;
                    Color brush = Colors.Transparent;

                    switch (Controller.Land. GetCellType(coordinates))
                    {
                        case CellType.Free:
                        {
                            break;
                        }

                        case CellType.Occupied:
                        {
                            break;
                        }

                        case CellType.Food:
                        {
                            brush = Colors.DarkGreen;
                            break;
                        }

                        case CellType.Poison:
                        {
                            brush = Colors.DarkRed;
                            break;
                        }

                        case CellType.Wall:
                        {
                            brush = Colors.SlateGray;
                            break;
                        }
                    }

                    var point = new Point(x * dx, y * dy);

                    if (Colors.Transparent != brush)
                    {
                        session.FillRectangle(new Rect(point, cell), brush);
                    }

                    if (coordinates == cursor)
                    {

                        session.DrawRectangle(new Rect(point, cell), Colors.WhiteSmoke);
                    }
                }
            }
        }

        private void DrawMeridians(CanvasDrawingSession session, float width, float height, float thickness)
        {
            var step = Convert.ToSingle(cellSize.Height);

            for (var y = 0.0f; y <= height; y += step)
            {
                session.DrawLine(new Vector2(0.0f, y), new Vector2(width, y), gridColorBrush, thickness);
            }
        }

        private void DrawParallels(CanvasDrawingSession session, float width, float height, float thickness)
        {
            var step = Convert.ToSingle(cellSize.Width);

            for (var x = 0.0f; x <= width; x += step)
            {
                session.DrawLine(new Vector2(x, 0.0f), new Vector2(x, height), gridColorBrush, thickness);
            }
        }
    }
}
