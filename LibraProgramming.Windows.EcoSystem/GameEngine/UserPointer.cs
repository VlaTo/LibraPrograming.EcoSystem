using System.Numerics;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.UI;

namespace LibraProgramming.Windows.Games.Towers.GameEngine
{
    public sealed class UserPointer : SceneNode
    {
        private readonly Color color;
        private ICanvasBrush drawBrush;

        public Point Position
        {
            get;
            set;
        }

        public bool IsVisible
        {
            get;
            set;
        }

        public UserPointer(Color color)
        {
            this.color = color;
        }

        public override Task CreateResourcesAsync(ICanvasResourceCreatorWithDpi creator, CanvasCreateResourcesReason reason)
        {
            drawBrush = new CanvasSolidColorBrush(creator, color);
            return Task.CompletedTask;
        }

        public override void Draw(CanvasDrawingSession session)
        {
            if (false == IsVisible)
            {
                return;
            }

            session.DrawCircle(Position.ToVector2(), 2.0f, drawBrush);
        }
    }
}