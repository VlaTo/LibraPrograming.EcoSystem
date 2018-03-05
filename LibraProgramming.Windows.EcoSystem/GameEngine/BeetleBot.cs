using LibraProgramming.Windows.EcoSystem.Core;
using System;
using System.Numerics;
using Windows.Foundation;

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
            State = new StartGenomeState();
        }



        private void DoDie()
        {
            dies.Invoke(this, new BeetleBotEventArgs());
        }

        /// <summary>
        /// 
        /// </summary>
        private abstract class GenomeStep : SceneNodeState<BeetleBot>
        {
            protected int Ip
            {
                get;
                private set;
            }

            protected GenomeStep(int ip)
            {
                Ip = ip;
            }

            protected ISceneNodeState Execute()
            {
                var moves = 10;

                while (0 < moves--)
                {
                    var opcode = Node.Genome[Ip];

                    if (0 <= opcode && opcode < 8)
                    {
                        var direction = GetDirection(opcode);
                        return new MovingState(direction, Ip + opcode);
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
                            return MovingDirection.Top;
                        }

                    case 1:
                        {
                            return MovingDirection.TopRight;
                        }

                    case 2:
                        {
                            return MovingDirection.TopRight;
                        }

                    case 3:
                        {
                            return MovingDirection.Right;
                        }

                    case 4:
                        {
                            return MovingDirection.DownRight;
                        }

                    case 5:
                        {
                            return MovingDirection.Down;
                        }

                    case 6:
                        {
                            return MovingDirection.DownLeft;
                        }

                    case 7:
                        {
                            return MovingDirection.Left;
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
        private sealed class StartGenomeState : GenomeStep
        {
            public StartGenomeState()
                : base(0)
            {
            }

            public override void Update(TimeSpan elapsed)
            {
                Node.State = Execute();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private enum MovingDirection
        {
            Top,
            TopRight,
            Right,
            DownRight,
            Down,
            DownLeft,
            Left,
            TopLeft
        }

        /// <summary>
        /// 
        /// </summary>
        private class MovingState : GenomeStep
        {
            private const float Epsilon = 1.0f;

            protected Vector2 Destination
            {
                get;
                private set;
            }

            public MovingState(MovingDirection direction, int ip)
                : base(ip)
            {
                Destination = GetDestination(direction);
            }

            public override void Update(TimeSpan elapsed)
            {
                if (Epsilon >= Vector2.Distance(Node.Position, Destination))
                {
                    DoDestinationReached();
                }
                else
                {
                    var direction = new Point(Math.Cos(Node.Angle), Math.Sin(Node.Angle));
                    Node.Position += direction.ToVector2() * Node.speed;
                }
            }

            protected virtual void DoDestinationReached()
            {
                Node.State = NodeState.Empty<BeetleBot>();
            }

            protected Vector2 GetDestination(MovingDirection direction)
            {
                switch (direction)
                {
                    case MovingDirection.Top:
                        {
                            var temp = Node.Coordinates + new Coordinates(0, -1);
                            return GetPosition(temp);
                        }

                    case MovingDirection.TopLeft:
                        {
                            var temp = Node.Coordinates + new Coordinates(-1, 0);
                            return GetPosition(temp);
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
        }

        /// <summary>
        /// 
        /// </summary>
        private class MoveAndEatState : MovingState
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