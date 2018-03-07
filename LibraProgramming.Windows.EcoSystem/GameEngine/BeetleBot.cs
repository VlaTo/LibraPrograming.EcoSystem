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
    public class BeetleBot : StateAwareSceneNode<BeetleBot>
    {
        private Coordinates coordinates;
        private bool canFireEvents;

        /// <summary>
        /// 
        /// </summary>
        public float Angle
        {
            get;
            protected set;
        }

        /// <summary>
        /// 
        /// </summary>
        public TimeSpan Lifespan
        {
            get;
            private set;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public float Speed
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public Coordinates Coordinates
        {
            get => coordinates;
            private set
            {
                if (value == coordinates)
                {
                    return;
                }

                var origin = coordinates;

                coordinates = value;

                DoMove(new BeetleBotMoveMessage(this, origin, coordinates));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IGenome Genome
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        protected Vector2 Position
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coordinates"></param>
        /// <param name="genome"></param>
        /// <param name="lifespan"></param>
        public BeetleBot(Coordinates coordinates, IGenome genome, TimeSpan lifespan)
        {
            Coordinates = coordinates;
            Angle = 0.0f;
            Genome = genome;
            Speed = 1.0f;
            Lifespan = lifespan;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        public override void Draw(CanvasDrawingSession session)
        {
            var direction = new Point(Math.Cos(Angle), Math.Sin(Angle));
            var end = Position + direction.ToVector2() * 11.0f;

            session.FillCircle(Position, 8.0f, Colors.Blue);
            session.DrawCircle(Position, 8.0f, Colors.LightGray);
            session.DrawLine(Position, end, Colors.LightGray);
        }

        protected override void DoParentAdded()
        {
            canFireEvents = true;
            Position = Controller.Positioning.GetPosition(Coordinates);
            State = new StartState();
            DoMove(new BeetleBotMoveMessage(this, null, Coordinates));
        }

        protected override void DoParentRemoved()
        {
            canFireEvents = false;
        }

        private void DoDie(BeetleBotDiesMessage message)
        {
            if (canFireEvents)
            {
                Controller.BeetleBotMessage.OnNext(message);
            }
        }

        private void DoMove(BeetleBotMoveMessage message)
        {
            if (canFireEvents)
            {
                Controller.BeetleBotMessage.OnNext(message);
            }
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
            private const float RotationEpsilon = 0.03f;
            private static readonly float RotationStep = Convert.ToSingle(Math.PI / 20.0d);
            private static readonly float PI2 = Convert.ToSingle(Math.PI * 2.0d);

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

                    // calculate rotation direction
                    case 1:
                    {
                        if(RotationEpsilon >= Math.Abs(Node.Angle - Angle))
                        {
                            step = 3;
                            break;
                        }
                        else if (Node.Angle > Angle)
                        {
                            rotation = -RotationStep;
                        }
                        else
                        {
                            rotation = RotationStep;
                        }

                        step++;

                        break;
                    }

                    // rotate
                    case 2:
                    {
                        var angle = NormalizeAngle(Node.Angle + rotation);

                        if (RotationEpsilon >= Math.Abs(Angle - angle))
                        {
                            step++;
                            Node.Angle = Angle;
                        }
                        else
                        {
                            Node.Angle = angle;
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
                var positioningSystem = Node.Controller.Positioning;
                return positioningSystem.GetPosition(coordinates);
            }

            protected bool CanMoveTo(Coordinates coordinates)
            {
                return Node.Controller.IsFreeCell(coordinates) && false == Node.Controller.IsObstacleInCell(coordinates);
            }

            private static float NormalizeAngle(float angle)
            {
                return Convert.ToSingle(angle % PI2);
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