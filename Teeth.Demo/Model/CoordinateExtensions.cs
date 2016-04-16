namespace Teeth.Demo.Model
{
    using System;
    using System.Windows;

    public static class CoordinateExtensions
    {
        public static Point GetMiddle(this Rect source)
        {
            return new Point((source.Left + source.Right) / 2.0, (source.Top + source.Bottom) / 2);
        }

        public static Point MovePointTowards(this Point a, Point b, double distance)
        {
            var vector = new Point(b.X - a.X, b.Y - a.Y);
            var length = Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            var unitVector = new Point(vector.X / length, vector.Y / length);
            return new Point(a.X + unitVector.X * distance, a.Y + unitVector.Y * distance);
        }
    }
}