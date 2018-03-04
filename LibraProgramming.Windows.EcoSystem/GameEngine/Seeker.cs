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
    public class Seeker : StateAwareSceneNode<Seeker>
    {
        private readonly Color borderColor;
        private readonly Color fillColor;
        private readonly float speed;
        private ICanvasBrush borderBrush;
        private ICanvasBrush fillBrush;

        public double Angle
        {
            get;
            protected set;
        }

        public Vector2 Position
        {
            get;
            private set;
        }

        public Seeker(Vector2 position, float speed, Color borderColor, Color fillColor)
        {
            this.borderColor = borderColor;
            this.fillColor = fillColor;

            this.speed = speed;

            Position = position;
        }

        public void MoveTo(Vector2 destination)
        {
            State = new MoveToState(destination);
        }

        public void LookAt(Vector2 target)
        {
            Angle = Math.Atan2(target.Y - Position.Y, target.X - Position.X);
        }

        public override Task CreateResourcesAsync(ICanvasResourceCreatorWithDpi creator, CanvasCreateResourcesReason reason)
        {
            borderBrush = new CanvasSolidColorBrush(creator, borderColor);
            fillBrush = new CanvasSolidColorBrush(creator, fillColor);

            return Task.CompletedTask;
        }

        public override void Draw(CanvasDrawingSession session)
        {
            var direction = new Point(Math.Cos(Angle), Math.Sin(Angle));
            var end = Position + direction.ToVector2() * 11.0f;

            session.DrawCircle(Position, 8.0f, borderBrush);
            session.FillCircle(Position, 8.0f, fillBrush);
            session.DrawLine(Position, end, borderBrush);

            base.Draw(session);

            var point0 = new Point(Position.X - 100.0f / 2.0d, Position.Y + 22.0d);

            session.DrawText(Angle.ToString("F1"), point0.ToVector2(), borderColor);
        }

        /// <summary>
        /// 
        /// </summary>
        private class MoveToState : SceneNodeState<Seeker>
        {
            private readonly Vector2 destination;

            public MoveToState(Vector2 destination)
            {
                this.destination = destination;
            }

            public override void Update(TimeSpan elapsed)
            {
                if (1.0f >= Vector2.Distance(Node.Position, destination))
                {
                    Node.State = NodeState.Empty<Seeker>();
                    return;
                }

                var direction = new Point(Math.Cos(Node.Angle), Math.Sin(Node.Angle));

                Node.Position += direction.ToVector2() * Node.speed;
            }
        }
    }
}
