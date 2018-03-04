using System;
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
    public class Enemy : StateAwareSceneNode<Enemy>
    {
        private static readonly Size HealthBarSize = new Size(18.0d, 4.0d);

        private readonly double healthAmount;
        private readonly float speed;
        private readonly ICoordinatesSystem _coordinatesSystem;
        private readonly IPathFinder pathFinder;
        private float health;

        public float Angle
        {
            get;
            protected set;
        }

        public TimeSpan Age
        {
            get;
            private set;
        }

        public Position Origin => _coordinatesSystem.GetPosition(Position);

        public Vector2 Position
        {
            get;
            set;
        }

        public float Damage
        {
            get;
        }

        public float Health
        {
            get => health;
            private set => health = Math.Max(0.0f, value);
        }

        public bool IsAlive => 0.0d < Health;

        protected ICanvasBrush DrawBrush
        {
            get;
            set;
        }

        protected ICanvasBrush FillBrush
        {
            get;
            set;
        }

        protected ICanvasBrush HealthBarBrush
        {
            get;
            set;
        }

        protected ICanvasBrush GoodHealthBrush
        {
            get;
            set;
        }

        protected ICanvasBrush PoorHealthBrush
        {
            get;
            set;
        }

        public Enemy(
            Position origin,
            ICoordinatesSystem coordinatesSystem,
            IPathFinder pathFinder,
            float health,
            float speed,
            float damage)
        {
            this._coordinatesSystem = coordinatesSystem;
            this.pathFinder = pathFinder;
            this.health = health;
            this.speed = speed;

            healthAmount = health;

            Angle = 0.0f;
            Damage = damage;
            Position = coordinatesSystem.GetPoint(origin);
            State = new CalculateWaypointsState();
        }

        public override void Update(TimeSpan elapsed)
        {
            if (false == IsAlive)
            {
                return;
            }

            Age += elapsed;

            base.Update(elapsed);
        }

        public override void Draw(CanvasDrawingSession session)
        {
            var direction = new Point(Math.Cos(Angle), Math.Sin(Angle));
            var end = Position + direction.ToVector2() * 11.0f;

            session.DrawCircle(Position, 8.0f, DrawBrush);
            session.FillCircle(Position, 8.0f, FillBrush);
            session.DrawLine(Position, end, DrawBrush);

            DrawHealthBar(session);
        }

        /*public void TakeDamage(double value)
        {
            Health -= value;

            if (false == IsAlive)
            {
                State = new KilledState();
            }
        }*/

        public override Task CreateResourcesAsync(ICanvasResourceCreatorWithDpi creator, CanvasCreateResourcesReason reason)
        {
            DrawBrush = new CanvasSolidColorBrush(creator, Colors.White);
            FillBrush = new CanvasSolidColorBrush(creator, Colors.Gray);
            HealthBarBrush = new CanvasSolidColorBrush(creator, Colors.Chartreuse);
            GoodHealthBrush = new CanvasSolidColorBrush(creator, Colors.Chartreuse);
            PoorHealthBrush = new CanvasSolidColorBrush(creator, Colors.OrangeRed);

            return Task.CompletedTask;
        }

        /*public void ReportDamage()
        {
            var emitter = (EnemyWaveEmitter) Parent;
            emitter.ReportEnemyReachedEnd(this);
        }*/

        /*public void Die()
        {
            var emitter = (EnemyWaveEmitter) Parent;
            emitter.ReportDie(this);
        }*/

        protected void DrawHealthBar(CanvasDrawingSession session)
        {
            var percentage = Math.Min(Math.Max(health / healthAmount, 0.0f), 1.0f);
            var width = HealthBarSize.Width * percentage;
            var point0 = new Point(Position.X - HealthBarSize.Width / 2.0d, Position.Y + 12.0d);

            session.DrawRectangle(new Rect(point0, HealthBarSize), HealthBarBrush);
            session.FillRectangle(
                new Rect(point0, new Size(width, HealthBarSize.Height)),
                percentage >= 0.4d ? GoodHealthBrush : PoorHealthBrush
            );
        }

        /// <summary>
        /// 
        /// </summary>
        private class CalculateWaypointsState : SceneNodeState<Enemy>
        {
            public override void Update(TimeSpan elapsed)
            {
                var origin = Node.Origin;
                var waypoints = Node.pathFinder.GetWaypoints(origin);

                Node.State = new FindNextPositionState(waypoints, 0);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private class FindNextPositionState : SceneNodeState<Enemy>
        {
            private readonly IReadOnlyCollection<Position> waypoints;
            private readonly int index;

            public FindNextPositionState(IReadOnlyCollection<Position> waypoints, int index)
            {
                this.waypoints = waypoints;
                this.index = index;
            }

            public override void Update(TimeSpan elapsed)
            {
                Node.State = null == waypoints || index >= waypoints.Count
                    ? NodeState.Empty<Enemy>()
                    : new MoveToPositionState(waypoints, index);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private class MoveToPositionState : SceneNodeState<Enemy>
        {
            protected IReadOnlyCollection<Position> Waypoints
            {
                get;
            }

            protected int CurrentIndex
            {
                get;
            }

            protected Position CurrePosition
            {
                get;
                private set;
            }

            public MoveToPositionState(IReadOnlyCollection<Position> waypoints, int index)
            {
                Waypoints = waypoints;
                CurrentIndex = index;
            }

            protected override void OnEnter()
            {
                CurrePosition = GetCurrentPosition();
            }

            public override void Update(TimeSpan elapsed)
            {
                var point = Node._coordinatesSystem.GetPoint(CurrePosition);
                var angle = (float) Math.Atan2(point.Y - Node.Position.Y, point.X - Node.Position.X);

                var delta = Node.Angle - angle;

                if (0.0d < delta || delta < 0.0d)
                {
                    Node.State = new EnemyRotatingState(angle, Waypoints, CurrentIndex);
                }
                else
                {
                    Node.State = new EnemyMovingState(Waypoints, CurrentIndex);
                }
            }

            private Position GetCurrentPosition()
            {
                using (var enumerator = Waypoints.GetEnumerator())
                {
                    enumerator.Reset();

                    for (var count = CurrentIndex; 0 <= count; count--)
                    {
                        if (false == enumerator.MoveNext())
                        {
                            break;
                        }

                        if (0 == count)
                        {
                            return enumerator.Current;
                        }
                    }
                }

                throw new Exception();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private class EnemyRotatingState : SceneNodeState<Enemy>
        {
            private const float step = 0.01f;

            private readonly float angle;
            private readonly IReadOnlyCollection<Position> waypoints;
            private readonly int index;
            private float delta;

            public EnemyRotatingState(float angle, IReadOnlyCollection<Position> waypoints, int index)
            {
                this.angle = angle;
                this.waypoints = waypoints;
                this.index = index;
            }

            protected override void OnEnter()
            {
                var sign = Math.Sign(Node.Angle);

                if (sign == Math.Sign(angle))
                {
                    if (sign > 0)
                    {
                        delta = Node.Angle > angle ? -step : step;
                    }
                    else
                    {
                        delta = Node.Angle < angle ? -step : step;
                    }
                }
                else
                {
                    delta = step;
                }
            }

            public override void Update(TimeSpan elapsed)
            {
                if (Math.Abs(angle - Node.Angle) <= step)
                {
                    Node.Angle = angle;
                    Node.State = new EnemyMovingState(waypoints, index);

                    return;
                }

                var value = Node.Angle + delta;

                if (value > Math.PI)
                {
                    value *= -1.0f;
                }

                Node.Angle = value;
            }
        }

/*
        /// <summary>
        /// 
        /// </summary>
        private class AntiClockwiseRotatingState : SceneNodeState
        {
            private readonly double delta;
            private readonly CellPosition[] waypoints;
            private readonly int index;
            private Enemy enemy;

            public AntiClockwiseRotatingState(double delta, CellPosition[] waypoints, int index)
            {
                this.delta = delta;
                this.waypoints = waypoints;
                this.index = index;
            }

            public override void Leave(ISceneNode node)
            {
                enemy = null;
            }

            public override void Enter(ISceneNode node)
            {
                enemy = (Enemy) node;
            }

            public override void Update(TimeSpan elapsed)
            {
                const double angle = 0.05d;

                if (Double.Epsilon > (delta - angle))
                {
                    enemy.Angle -= angle;
                }
                else
                {
                    enemy.State = Empty;
                }
            }
        }
*/

        /// <summary>
        /// 
        /// </summary>
        private class EnemyMovingState : MoveToPositionState
        {
            public EnemyMovingState(IReadOnlyCollection<Position> waypoints, int index)
                : base(waypoints, index)
            {
            }

            public override void Update(TimeSpan elapsed)
            {
                var destination = Node._coordinatesSystem.GetPoint(CurrePosition);

                if (1.0f >= Vector2.Distance(Node.Position, destination))
                {
                    Node.State = new FindNextPositionState(Waypoints, CurrentIndex + 1);
                }
                else
                {
                    var direction = new Point(Math.Cos(Node.Angle), Math.Sin(Node.Angle));
                    Node.Position += direction.ToVector2() * Node.speed;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /*private class MoveToNextWaypointState : SceneNodeState
        {
            private readonly IList<Vector2> waypoints;
            private readonly int index;
            private Enemy enemy;

            public MoveToNextWaypointState(IList<Vector2> waypoints, int index)
            {
                this.waypoints = waypoints;
                this.index = index;
            }

            public override void Leave(ISceneNode node)
            {
                enemy = null;
            }

            public override void Enter(ISceneNode node)
            {
                enemy = (Enemy) node;
            }

            public override void Update(TimeSpan elapsed)
            {
                if (index >= waypoints.Count)
                {
                    return;
                }

                var waypoint = waypoints[index];
                var angle = Math.Atan2(waypoint.Y - enemy.Position.Y, waypoint.X - enemy.Position.X);

                enemy.State = new RotatingState(angle, waypoints, index + 1);
            }
        }*/


        /// <summary>
        /// 
        /// </summary>
        /*private class MoveToWaypointState : SceneNodeState
        {
            private readonly IList<Vector2> waypoints;
            private readonly int index;
            private Enemy enemy;
            private Vector2 destination;

            public MoveToWaypointState(IList<Vector2> waypoints, int index)
            {
                this.waypoints = waypoints;
                this.index = index;
            }

            public override void Leave(ISceneNode node)
            {
                enemy = null;
            }

            public override void Enter(ISceneNode node)
            {
                enemy = (Enemy) node;
                destination = waypoints[index];
            }

            public override void Update(TimeSpan elapsed)
            {
                if (1.0f >= Vector2.Distance(enemy.Position, destination))
                {
                    enemy.State = new MoveToNextWaypointState(waypoints, index + 1);
                    return;
                }

                var direction = new Point(Math.Cos(enemy.Angle), Math.Sin(enemy.Angle));

                enemy.Position += direction.ToVector2() * (float) enemy.speed;
            }
        }*/
    }
}