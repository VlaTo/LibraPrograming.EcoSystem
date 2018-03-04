using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using LibraProgramming.Windows.Games.Towers.Core.ServiceContainer;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.UI;

namespace LibraProgramming.Windows.Games.Towers.GameEngine
{
    public sealed class LandscapeMap : SceneNode
    {
        private readonly Size size;
        private readonly Color border;
        private readonly Color floor;
        private ICanvasBrush borderBrush;
        private ICanvasBrush floorBrush;

        [PrefferedConstructor]
        public LandscapeMap(Size size, Color border, Color floor)
        {
            this.size = size;
            this.border = border;
            this.floor = floor;
        }

        public override Task CreateResourcesAsync(ICanvasResourceCreatorWithDpi creator, CanvasCreateResourcesReason reason)
        {
            borderBrush = new CanvasSolidColorBrush(creator, border);
            floorBrush = new CanvasSolidColorBrush(creator, floor);

            return Task.CompletedTask;
        }

        public override void Draw(CanvasDrawingSession session)
        {
            using (session.CreateLayer(1.0f))
            {
                var rect = new Rect(new Point(), size);

                session.DrawRectangle(rect, borderBrush);
                session.FillRectangle(rect, floorBrush);
            }
        }
    }
}