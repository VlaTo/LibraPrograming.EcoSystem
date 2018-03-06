using LibraProgramming.Windows.EcoSystem.Core;
using Microsoft.Graphics.Canvas;
using System;
using System.Numerics;
using Windows.Foundation;
using Windows.UI;

namespace LibraProgramming.Windows.EcoSystem.GameEngine
{
    /// <summary>
    /// 
    /// </summary>
    public class BeetleBotEventArgs : EventArgs
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public class BeetleBot : StateAwareSceneNode<BeetleBot>
    {
        private readonly IPositioningSystem positioningSystem;
        private readonly WeakEventHandler<BeetleBotEventArgs> dies;
        private readonly float speed;

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

        public Vector2 Position
        {
            get;
            private set;
        }

        public Coordinates Coordinates
        {
            get;
            private set;
        }

        public IGenome Genome
        {
            get;
            private set;
        }

        public event EventHandler<BeetleBotEventArgs> Dies
        {
            add => dies.AddHandler(value);
            remove => dies.RemoveHandler(value);
        }

        public BeetleBot(Coordinates coordinates, IGenome genome, IPositioningSystem positioningSystem)
        {
            this.positioningSystem = positioningSystem;

            dies = new WeakEventHandler<BeetleBotEventArgs>();
            speed = 1.0f;

            Angle = 0.0f;
            Genome = genome;
            Coordinates = coordinates;
            Position = positioningSystem.GetPosition(coordinates);
            State = new StartState();
        }

        public override void Draw(CanvasDrawingSession session)
        {
            var direction = new Point(Math.Cos(Angle), Math.Sin(Angle));
            var end = Position + direction.ToVector2() * 11.0f;

            session.FillCircle(Position, 8.0f, Colors.Blue);
            session.DrawCircle(Position, 8.0f, Colors.LightGray);
            session.DrawLine(Position, end, Colors.LightGray);
        }

        private void DoDie()
        {
            dies.Invoke(this, new BeetleBotEventArgs());
        }

        /// <summary>
        /// 
        /// </summary>
        private abstract class GenomeState : SceneNodeState<BeetleBot>
        {
            protected int Ip
            {
                get;
                private set;
            }

            protected GenomeState(int ip)
            {
                Ip = ip;
            }

            protected ISceneNodeState GetNextState()
            {
                var moves = 10;

                while (0 < moves--)
                {
                    var opcode = Node.Genome[Ip];

                    if (0 <= opcode && opcode < 8)
                    {
                        var direction = GetDirection(opcode);
                        var forward = Math.Max((byte)1, opcode);
                        return new RotateAndMoveState(direction, Ip + forward);
                    }

                    if (8 <= opcode && opcode < 16)
                    {
                        var direction = GetDirection((byte)(opcode - 8));
                        return new MoveAndEatState(direction, Ip + opcode);
                    }

                    Ip += opcode;
                }

                return NodeState.Empty<BeetleBot>();
            }

            private MovingDirection GetDirection(byte opcode)
            {
                switch (opcode)
                {
                    case 0:
                    {
                        return MovingDirection.Up;
                    }

                    case 1:
                    {
                        return MovingDirection.UpRight;
                    }

                    case 2:
                    {
                        return MovingDirection.Right;
                    }

                    case 3:
                    {
                        return MovingDirection.DownRight;
                    }

                    case 4:
                    {
                        return MovingDirection.Down;
                    }

                    case 5:
                    {
                        return MovingDirection.DownLeft;
                    }

                    case 6:
                    {
                        return MovingDirection.Left;
                    }

                    case 7:
                    {
                        return MovingDirection.UpLeft;
                    }

                    default:
                    {
                        throw new NotSupportedException();
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private sealed class StartState : GenomeState
        {
            public StartState()
                : base(0)
            {
            }

            public override void Update(TimeSpan elapsed)
            {
                Node.State = GetNextState();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private enum MovingDirection
        {
            Up,
            UpRight,
            Right,
            DownRight,
            Down,
            DownLeft,
            Left,
            UpLeft
        }

        /// <summary>
        /// 
        /// </summary>
        private class RotateAndMoveState : GenomeState
        {
            private const float Epsilon = 1.0f;

            private readonly MovingDirection direction;
            private int step;

            protected Vector2 Destination
            {
                get;
                private set;
            }

            protected Coordinates Coordinates
            {
                get;
                set;
            }

            public RotateAndMoveState(MovingDirection direction, int ip)
                : base(ip)
            {
                this.direction = direction;
            }

            public override void Update(TimeSpan elapsed)
            {
                switch (step)
                {
                    case 0:
                    {
                        Coordinates = Node.Coordinates + GetDestinationDelta(direction);
                        Destination = GetPosition(Coordinates);

                        if (false == CanMoveTo(Coordinates))
                        {
                            step = 2;
                        }
                        else
                        {
                            step++;
                        }

                        break;
                    }

                    case 1:
                    {
                        if (Epsilon >= Vector2.Distance(Node.Position, Destination))
                        {
                            step++;
                        }
                        else
                        {
                            var direction = new Point(Math.Cos(Node.Angle), Math.Sin(Node.Angle));
                            Node.Position += direction.ToVector2() * Node.speed;
                        }

                        break;
                    }

                    case 2:
                    {
                        DoComplete();
                        break;
                    }
                }
            }

            protected virtual void DoComplete()
            {
                Node.State = GetNextState();
            }

            protected Coordinates GetDestinationDelta(MovingDirection direction)
            {
                switch (direction)
                {
                    case MovingDirection.Up:
                    {
                        return new Coordinates(0, -1);
                    }

                    case MovingDirection.UpRight:
                    {
                        return new Coordinates(1, -1);
                    }

                    case MovingDirection.Right:
                    {
                        return new Coordinates(1, 0);
                    }

                    case MovingDirection.DownRight:
                    {
                        return new Coordinates(1, 1);
                    }

                    case MovingDirection.Down:
                    {
                        return new Coordinates(0, 1);
                    }

                    case MovingDirection.DownLeft:
                    {
                        return new Coordinates(-1, 1);
                    }

                    case MovingDirection.Left:
                    {
                        return new Coordinates(-1, 0);
                    }

                    case MovingDirection.UpLeft:
                    {
                        return new Coordinates(-1, -1);
                    }

                    default:
                    {
                        throw new Exception();
                    }
                }
            }

            protected Vector2 GetPosition()
            {
                return GetPosition(Node.Coordinates);
            }

            protected Vector2 GetPosition(Coordinates coordinates)
            {
                var positioningSystem = Node.positioningSystem;
                return positioningSystem.GetPosition(coordinates);
            }

            protected bool CanMoveTo(Coordinates coordinates)
            {
                var positioningSystem = Node.positioningSystem;
                return positioningSystem.IsFree(coordinates) && false == positioningSystem.IsObstacle(coordinates);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private class MoveAndEatState : RotateAndMoveState
        {
            public MoveAndEatState(MovingDirection direction, int ip)
                : base(direction, ip)
            {
            }

            public override void Update(TimeSpan elapsed)
            {
                base.Update(elapsed);
            }
        }
    }
}