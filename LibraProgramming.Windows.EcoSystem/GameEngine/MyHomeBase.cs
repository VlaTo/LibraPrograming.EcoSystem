using System;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.UI;

namespace LibraProgramming.Windows.Games.Towers.GameEngine
{
    public class MyHomeBase : StateAwareSceneNode<MyHomeBase>
    {
        private readonly Color color;
        private ICanvasBrush brush;

        public double Health
        {
            get;
            private set;
        }

        public Rect Position
        {
            get;
        }

        public double MaxHealth
        {
            get;
        }

        public MyHomeBase(Rect position, Color color, double health)
        {
            this.color = color;
            Position = position;
            Health = health;
            MaxHealth = health;
        }

        public void TakeDamage(double value)
        {
            Health = Math.Max(0.0d, Health - value);

            if (0.0d > Health)
            {
                return;
            }

            State = new DefeatState();
        }

        public override Task CreateResourcesAsync(ICanvasResourceCreatorWithDpi creator, CanvasCreateResourcesReason reason)
        {
            brush = new CanvasSolidColorBrush(creator, color);
            return Task.CompletedTask;
        }

        public override void Draw(CanvasDrawingSession session)
        {
            var point0 = new Point(Position.X, Position.Y);
            var point1 = new Point(Position.Right, Position.Y);
            session.DrawLine(point0.ToVector2(), point1.ToVector2(), brush, 2.0f);
        }

        private void DoDefeat()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        private class DefeatState : SceneNodeState<MyHomeBase>
        {
            public override void Update(TimeSpan elapsed)
            {
                Node.DoDefeat();
                Node.State = NodeState.Empty<MyHomeBase>();
            }
        }
    }
}