using System;
using System.Collections.Generic;
using System.Numerics;
using Windows.Foundation;
using Microsoft.Graphics.Canvas;

namespace LibraProgramming.Windows.Games.Towers.GameEngine
{
/*
    public class FastMovementEnemy : Enemy
    {
        public FastMovementEnemy(Point origin, double health, ICollection<Point> waypoints, double speed, double damage)
            : base(origin, health, waypoints, speed, damage)
        {
        }

        public override void Draw(CanvasDrawingSession session)
        {
            var direction = new Point(Math.Cos(Angle), Math.Sin(Angle));
            var end = Position + direction.ToVector2() * 11.0f;
            var rect = new Rect(new Point(Position.X - 4.0d, Position.Y - 4.0d), new Size(8.0d, 8.0d));

            session.DrawRectangle(rect, DrawBrush);
            session.FillRectangle(rect, FillBrush);
            session.DrawLine(Position, end, DrawBrush);

            DrawHealthBar(session);
        }
    }
*/
}