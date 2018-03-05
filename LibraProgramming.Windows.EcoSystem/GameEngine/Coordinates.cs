using System;
using System.Diagnostics;
using Windows.Foundation;

namespace LibraProgramming.Windows.EcoSystem.GameEngine
{
    /// <summary>
    /// 
    /// </summary>
    [DebuggerDisplay("X = {X}, Y = {Y}")]
    public struct Coordinates : IEquatable<Coordinates>
    {
        /// <summary>
        /// 
        /// </summary>
        public int X
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Y
        {
            get;
        }

        public Coordinates(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(Coordinates other)
        {
            return X == other.X && Y == other.Y;
        }

        public Coordinates Add(Coordinates other)
        {
            if (null == other)
            {
                throw new ArgumentNullException(nameof(other));
            }

            return new Coordinates(X + other.X, Y + other.Y);
        }

        public Coordinates Subtract(Coordinates other)
        {
            if (null == other)
            {
                throw new ArgumentNullException(nameof(other));
            }

            return new Coordinates(X - other.X, Y - other.Y);
        }

        public Coordinates Multiply(Coordinates other)
        {
            if (null == other)
            {
                throw new ArgumentNullException(nameof(other));
            }

            return new Coordinates(X * other.X, Y * other.Y);
        }

        public Coordinates Divide(Coordinates other)
        {
            if (null == other)
            {
                throw new ArgumentNullException(nameof(other));
            }

            return new Coordinates(X / other.X, Y / other.Y);
        }

        public Point ToPoint()
        {
            return new Point(X, Y);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is Coordinates position && Equals(position);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }

        public static bool operator ==(Coordinates left, Coordinates right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Coordinates left, Coordinates right)
        {
            return false == left.Equals(right);
        }

        public static Coordinates operator +(Coordinates first, Coordinates second)
        {
            return first.Add(second);
        }

        public static Coordinates operator -(Coordinates first, Coordinates second)
        {
            return first.Subtract(second);
        }

        public static Coordinates operator *(Coordinates first, Coordinates second)
        {
            return first.Multiply(second);
        }

        public static Coordinates operator /(Coordinates first, Coordinates second)
        {
            return first.Divide(second);
        }
    }
}