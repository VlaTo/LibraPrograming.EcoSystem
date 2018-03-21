using System;
using System.Diagnostics;
using Windows.Foundation;

namespace LibraProgramming.Windows.EcoSystem.GameEngine
{
    /// <summary>
    /// 
    /// </summary>
    [DebuggerDisplay("Width = {Width}, Height = {Height}")]
    public sealed class MapSize : IEquatable<MapSize>
    {
        /// <summary>
        /// 
        /// </summary>
        public int Width
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Height
        {
            get;
        }

        public MapSize(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public bool Equals(MapSize other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Width == other.Width && Height == other.Height;
        }

        public override bool Equals(object other)
        {
            if (other is null)
            {
                return false;
            }

            return other is Coordinates position && Equals(position);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Width * 397) ^ Height;
            }
        }

        public Size ToSize()
        {
            return new Size(Width, Height);
        }

        public static bool operator ==(MapSize left, MapSize right)
        {
            if (left is null)
            {
                return right is null;
            }

            return left.Equals(right);
        }

        public static bool operator !=(MapSize left, MapSize right)
        {
            if (left is null)
            {
                return false == right is null;
            }

            return false == left.Equals(right);
        }
    }
}
