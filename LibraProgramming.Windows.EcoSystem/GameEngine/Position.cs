using System;
using System.Diagnostics;

namespace LibraProgramming.Windows.Games.Towers.GameEngine
{
    [DebuggerDisplay("Column = {Column}, Row = {Row}")]
    public struct Position : IEquatable<Position>
    {
        public int Column
        {
            get;
        }

        public int Row
        {
            get;
        }

        public Position(int column, int row)
        {
            Column = column;
            Row = row;
        }

        public bool Equals(Position other)
        {
            return Column == other.Column && Row == other.Row;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is Position position && Equals(position);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Column * 397) ^ Row;
            }
        }

        public static Position Add(Position first, Position second)
        {
            return new Position(first.Column + second.Column, first.Row + second.Row);
        }

        public static Position Subtract(Position first, Position second)
        {
            return new Position(first.Column - second.Column, first.Row - second.Row);
        }

        public static bool operator ==(Position left, Position right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Position left, Position right)
        {
            return false == left.Equals(right);
        }

        public static Position operator +(Position first, Position second)
        {
            return Add(first, second);
        }

        public static Position operator -(Position first, Position second)
        {
            return Subtract(first, second);
        }
    }
}