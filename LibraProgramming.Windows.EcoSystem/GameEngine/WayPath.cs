using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.UI;

namespace LibraProgramming.Windows.Games.Towers.GameEngine
{
    public sealed class WayPath : SceneNode
    {
        private readonly ICollection<Point> waypoints;
        private readonly Color edgeColor;
        private readonly Color vertexColor;
        private ICanvasBrush edgeBrush;
        private ICanvasBrush vertexBrush;

        public WayPath(ICollection<Point> waypoints, Color edgeColor, Color vertexColor)
        {
            this.waypoints = waypoints;
            this.edgeColor = edgeColor;
            this.vertexColor = vertexColor;
        }

        public override Task CreateResourcesAsync(ICanvasResourceCreatorWithDpi creator, CanvasCreateResourcesReason reason)
        {
            edgeBrush = new CanvasSolidColorBrush(creator, edgeColor);
            vertexBrush = new CanvasSolidColorBrush(creator, vertexColor);

            return Task.CompletedTask;
        }

        public override void Draw(CanvasDrawingSession session)
        {
            using (session.CreateLayer(1.0f))
            {
                Vector2? vertex = null;

                foreach (var point in waypoints)
                {
                    var temp = point.ToVector2();

                    if (vertex.HasValue)
                    {
                        session.DrawLine(vertex.Value, temp, edgeBrush);
                    }

                    session.DrawCircle(temp, 2.0f, vertexBrush);

                    vertex = temp;
                }
            }
        }
    }
}