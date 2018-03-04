using System.Threading.Tasks;
using Windows.UI;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.UI;

namespace LibraProgramming.Windows.Games.Towers.GameEngine
{
    public sealed class LaserBeam : SceneNode
    {
        private readonly Enemy enemy;
        private readonly Color color;
        private ICanvasBrush drawBrush;

        public LaserBeam(Enemy enemy, Color color)
        {
            this.enemy = enemy;
            this.color = color;
        }

        public override Task CreateResourcesAsync(ICanvasResourceCreatorWithDpi creator, CanvasCreateResourcesReason reason)
        {
            drawBrush = new CanvasSolidColorBrush(creator, color);
            return Task.CompletedTask;
        }

        public override void Draw(CanvasDrawingSession session)
        {
            var origin = ((LaserTower) Parent).Position;
            session.DrawLine(origin, enemy.Position, drawBrush);
        }
    }
}