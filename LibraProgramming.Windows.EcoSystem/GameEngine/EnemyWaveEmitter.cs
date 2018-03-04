using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using LibraProgramming.Windows.Games.Towers.Core;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.UI;

namespace LibraProgramming.Windows.Games.Towers.GameEngine
{
    /// <summary>
    /// 
    /// </summary>
    public class EnemyEventArgs : EventArgs
    {
        public Enemy Enemy
        {
            get;
        }

        public EnemyEventArgs(Enemy enemy)
        {
            Enemy = enemy;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class EnemyWaveEmitter : StateAwareSceneNode<EnemyWaveEmitter>
    {
        private readonly EnemyWaveFactory factory;
        private readonly WeakEventHandler<EnemyEventArgs> enemyReachedEnd;
        private readonly WeakEventHandler<EnemyEventArgs> enemyCreated;
        private readonly WeakEventHandler<EnemyEventArgs> enemyKilled;
        private readonly Color color;
        private ICanvasBrush brush;

        public ICanvasResourceCreatorWithDpi Creator
        {
            get;
            private set;
        }
        public ICollection<Point> Waypoints
        {
            get;
        }

        public Rect Position
        {
            get;
        }

        public event EventHandler<EnemyEventArgs> EnemyCreated
        {
            add => enemyCreated.AddHandler(value);
            remove => enemyCreated.RemoveHandler(value);
        }

        public event EventHandler<EnemyEventArgs> EnemyReachedEnd
        {
            add => enemyReachedEnd.AddHandler(value);
            remove => enemyReachedEnd.RemoveHandler(value);
        }

        public event EventHandler<EnemyEventArgs> EnemyKilled
        {
            add => enemyKilled.AddHandler(value);
            remove => enemyKilled.RemoveHandler(value);
        }

        public EnemyWaveEmitter(EnemyWaveFactory factory, Rect position, Color color, ICollection<Point> waypoints)
        {
            this.factory = factory;
            this.color = color;
            enemyCreated = new WeakEventHandler<EnemyEventArgs>();
            enemyReachedEnd = new WeakEventHandler<EnemyEventArgs>();
            enemyKilled = new WeakEventHandler<EnemyEventArgs>();
            Position = position;
            Waypoints = waypoints;
        }

        public override Task CreateResourcesAsync(ICanvasResourceCreatorWithDpi creator, CanvasCreateResourcesReason reason)
        {
            brush = new CanvasSolidColorBrush(creator, color);
            State = new StartupTimeoutState(TimeSpan.FromSeconds(1.0d));
            Creator = creator;

            return base.CreateResourcesAsync(creator, reason);
        }

        public override void Draw(CanvasDrawingSession session)
        {
            var point0 = new Point(Position.X, Position.Y);
            var point1 = new Point(Position.X, Position.Bottom);

            session.DrawLine(point0.ToVector2(), point1.ToVector2(), brush, 2.0f);

            using (session.CreateLayer(1.0f))
            {
                base.Draw(session);
            }
        }

        public Point GetSpawnPoint()
        {
            if (null != Waypoints)
            {
                using (var enumerator = Waypoints.GetEnumerator())
                {
                    enumerator.Reset();

                    if (enumerator.MoveNext())
                    {
                        return enumerator.Current;
                    }
                }
            }

            return new Point();
        }

        public void ReportEnemyReachedEnd(Enemy enemy)
        {
            enemyReachedEnd.Invoke(this, new EnemyEventArgs(enemy));
            Children.Remove(enemy);
        }

        public void ReportDie(Enemy enemy)
        {
            enemyKilled.Invoke(this, new EnemyEventArgs(enemy));
            Children.Remove(enemy);
        }

        private void CreateEnemy(int waveNumber)
        {
            var enemy = factory.CreateEnemy(this, waveNumber);

            Children.Add(enemy);
            enemyCreated.Invoke(this, new EnemyEventArgs(enemy));
        }

        /// <summary>
        /// 
        /// </summary>
        private class StartupTimeoutState : SceneNodeState<EnemyWaveEmitter>
        {
            private readonly TimeSpan timeout;
            private TimeSpan current;

            public StartupTimeoutState(TimeSpan timeout)
            {
                this.timeout = timeout;
            }

            protected override void OnEnter()
            {
                current = TimeSpan.Zero;
            }

            public override void Update(TimeSpan elapsed)
            {
                current += elapsed;

                if (current >= timeout)
                {
                    Node.State = new EmitWaveState(1, 5);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private class EmitWaveState : SceneNodeState<EnemyWaveEmitter>
        {
            private static readonly TimeSpan NextNpcCooldown = TimeSpan.FromSeconds(1.4d);

            private readonly int number;
            private TimeSpan cooldown;
            private int npcs;

            public EmitWaveState(int number, int npcs)
            {
                this.number = number;
                this.npcs = npcs;
            }

            protected override void OnEnter()
            {
                cooldown = TimeSpan.Zero;
            }

            public override void Update(TimeSpan elapsed)
            {
                if (0 < npcs)
                {
                    if (TimeSpan.Zero < cooldown)
                    {
                        cooldown -= elapsed;
                        return;
                    }

                    npcs--;
                    Node.CreateEnemy(number);
                    cooldown = NextNpcCooldown;
                }
                else
                {
                    Node.State = new WaveIntermediateTimeoutState(TimeSpan.FromSeconds(15.0d), number);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private class WaveIntermediateTimeoutState : SceneNodeState<EnemyWaveEmitter>
        {
            private readonly TimeSpan timeout;
            private readonly int waveNumber;
            private TimeSpan current;

            public WaveIntermediateTimeoutState(TimeSpan timeout, int waveNumber)
            {
                this.timeout = timeout;
                this.waveNumber = waveNumber;
            }

            protected override void OnEnter()
            {
                current = TimeSpan.Zero;
            }

            public override void Update(TimeSpan elapsed)
            {
                current += elapsed;

                if (current < timeout)
                {
                    return;
                }

                var number = waveNumber + 1;

                Node.State = number >= 10 ? NodeState.Empty<EnemyWaveEmitter>() : new EmitWaveState(number, 5);
            }
        }
    }
}