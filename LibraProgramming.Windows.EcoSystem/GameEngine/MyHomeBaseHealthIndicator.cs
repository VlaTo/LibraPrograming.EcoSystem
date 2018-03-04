using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.UI;

namespace LibraProgramming.Windows.Games.Towers.GameEngine
{
    public class MyHomeBaseHealthIndicator : SceneNode
    {
        private readonly MyHomeBase homeBase;
        private readonly Rect position;
        private readonly Color borderColor;
        private readonly Color fillColor;
        private readonly Color warningColor;
        private ICanvasBrush borderBrush;
        private ICanvasBrush fillBrush;
        private ICanvasBrush warningBrush;

        public MyHomeBaseHealthIndicator(MyHomeBase homeBase, Rect position, Color borderColor, Color fillColor, Color warningColor)
        {
            this.homeBase = homeBase;
            this.position = position;
            this.borderColor = borderColor;
            this.fillColor = fillColor;
            this.warningColor = warningColor;
        }

        public override Task CreateResourcesAsync(ICanvasResourceCreatorWithDpi creator, CanvasCreateResourcesReason reason)
        {
            borderBrush = new CanvasSolidColorBrush(creator, borderColor);
            fillBrush = new CanvasSolidColorBrush(creator, fillColor);
            warningBrush = new CanvasSolidColorBrush(creator, warningColor);

            return Task.CompletedTask;
        }

        public override void Draw(CanvasDrawingSession session)
        {
            DrawHealthBar(session);
        }

        private void DrawHealthBar(CanvasDrawingSession session)
        {
            var percentage = Math.Min(Math.Max(homeBase.Health / homeBase.MaxHealth, 0.0f), 1.0f);
            var width = position.Width * percentage;

            session.DrawRectangle(position, borderBrush);
            session.FillRectangle(
                new Rect(new Point(position.X, position.Y), new Size(width, position.Height)),
                percentage >= 0.4d ? fillBrush : warningBrush
            );
        }
    }
}