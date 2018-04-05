using System;

namespace LibraProgramming.Windows.EcoSystem.GameEngine
{
    public class Rotation
    {
        public static readonly float MaxValue;
        public static readonly float MinValue;

        static Rotation()
        {
            MaxValue = Convert.ToSingle(Math.PI * 2);
            MinValue = 0.0f;
        }

        public static float Angle(float from, float to, out RotateDirection direction)
        {
            float Calculate(float source, float destination)
            {
                var direct = destination - source;
                var indirect = MaxValue - direct;

                return Math.Min(direct, indirect);
            }

            from = Normalize(from);
            to = Normalize(to);

            if (from > to)
            {
                direction = RotateDirection.CW;
                return Calculate(to, from);
            }

            direction = RotateDirection.CCW;

            return Calculate(from, to);
        }

        public static float Normalize(float value)
        {
            if (0.0f > value)
            {
                return MaxValue + value;
            }

            return value % MaxValue;
        }
    }
}