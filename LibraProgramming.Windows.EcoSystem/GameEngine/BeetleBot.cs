using LibraProgramming.Windows.EcoSystem.Core;
using Microsoft.Graphics.Canvas;
using System;
using System.Diagnostics;
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

        public float Angle
        {
            get;
            protected set;
        }

        public TimeSpan Lifespan
        {
            get;
            private set;
        }

        public Vector2 Position
        {
            get;
            private set;
        }

        public float Speed
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

        public BeetleBot(Coordinates coordinates, IGenome genome, TimeSpan lifespan, IPositioningSystem positioningSystem)
        {
            this.positioningSystem = positioningSystem;

            dies = new WeakEventHandler<BeetleBotEventArgs>();

            Angle = 0.0f;
            Genome = genome;
            Coordinates = coordinates;
            Position = positioningSystem.GetPosition(coordinates);
            Speed = 1.0f;
            Lifespan = lifespan;
            State = new StartState();
            //State = NodeState.Empty<BeetleBot>();
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
            private static readonly MovingDirection[] directions;

            protected int Ip
            {
                get;
                private set;
            }

            protected GenomeState(int ip)
            {
                Ip = ip;
            }

            static GenomeState()
            {
                directions = new[]
                {
                    MovingDirection.Right,
                    MovingDirection.DownRight,
                    MovingDirection.Down,
                    MovingDirection.DownLeft,
                    MovingDirection.Left,
                    MovingDirection.UpLeft,
                    MovingDirection.Up,
                    MovingDirection.UpRight
                };
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
                var index = opcode % directions.Length;
                return directions[index];
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
            private const float DistanceEpsilon = 1.0f;
            private const float AngleRotation = 0.03f;

            private readonly MovingDirection direction;

            private float rotation;
            private int step;

            protected Vector2 Origin
            {
                get;
                private set;
            }

            protected Vector2 Destination
            {
                get;
                private set;
            }

            protected float Angle
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
                    // initiate
                    case 0:
                    {
                        Coordinates = Node.Coordinates + GetDestinationDelta(direction);
                        Destination = GetPosition(Coordinates);
                        Angle = GetAngle(direction);

                        if (Node.Coordinates != Coordinates && CanMoveTo(Coordinates))
                        {
                            step++;
                        }
                        else
                        {
                            step = 4;
                        }

                        break;
                    }

                    // rotate
                    case 1:
                    {
                        var temp1 = Node.Angle - Angle;
                        var temp2 = Angle - Node.Angle;

                        Debug.WriteLine("Current: {0}; Destination: {1}; delta1: {2}; delta2: {3}", Node.Angle, Angle, temp1, temp2);

                        var sign = Math.Sign(Node.Angle);

                        if (sign == Math.Sign(Angle))
                        {
                            if (sign > 0)
                            {
                                rotation = Node.Angle > Angle ? -AngleRotation : AngleRotation;
                            }
                            else
                            {
                                rotation = Node.Angle > Angle ? AngleRotation : -AngleRotation;
                            }
                        }
                        else
                        {
                            rotation = AngleRotation;
                        }

                        step++;

                        break;
                    }

                    case 2:
                    {
                        var angle = Convert.ToSingle(Node.Angle % Math.PI * 2.0d);

                        if (AngleRotation >= Math.Abs(angle - Angle))
                        {
                            step++;
                            Node.Angle = Angle;
                        }
                        else
                        {
                            Node.Angle += rotation;
                        }

                        break;
                    }

                    // move
                    case 3:
                    {
                        if (DistanceEpsilon >= Vector2.Distance(Node.Position, Destination))
                        {
                            step++;
                            Node.Position = Destination;
                            Node.Coordinates = Coordinates;
                        }
                        else
                        {
                            var direction = new Point(Math.Cos(Node.Angle), Math.Sin(Node.Angle));
                            Node.Position += direction.ToVector2() * Node.Speed;
                        }

                        break;
                    }

                    // done
                    case 4:
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

                    case MovingDirection.Up:
                    {
                        return new Coordinates(0, -1);
                    }

                    case MovingDirection.UpRight:
                    {
                        return new Coordinates(1, -1);
                    }

                    default:
                    {
                        throw new Exception();
                    }
                }
            }

            protected float GetAngle(MovingDirection direction)
            {
                switch (direction)
                {
                    case MovingDirection.DownRight:
                    {
                        return Convert.ToSingle(Math.PI * 0.25d);
                    }

                    case MovingDirection.Down:
                    {
                        return Convert.ToSingle(Math.PI * 0.5d);
                    }

                    case MovingDirection.DownLeft:
                    {
                        return Convert.ToSingle(Math.PI * 0.75d);
                    }

                    case MovingDirection.Left:
                    {
                        return Convert.ToSingle(Math.PI);
                    }

                    case MovingDirection.UpLeft:
                    {
                        return Convert.ToSingle(5.0d * Math.PI / 4.0d);
                    }

                    case MovingDirection.Up:
                    {
                        return Convert.ToSingle(3.0d * Math.PI / 2.0d);
                    }

                    case MovingDirection.UpRight:
                    {
                        return Convert.ToSingle(7.0d * Math.PI / 4.0d);
                    }

                    case MovingDirection.Right:
                    default:
                    {
                        return 0.0f;
                    }
                }
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