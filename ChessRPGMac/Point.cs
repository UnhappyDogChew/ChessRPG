using System;

namespace ChessRPGMac
{
    public struct Point
    {
        public int x { get; set; }
        public int y { get; set; }

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return string.Format("({0}, {1})", x, y);
        }

        public static bool operator <(Point a, Point b) => (a.x < b.x || a.y < b.y);
        public static bool operator >(Point a, Point b) => (a.x > b.x || a.y > b.y);
        public static bool operator <=(Point a, Point b) => (a.x <= b.x || a.y <= b.y);
        public static bool operator >=(Point a, Point b) => (a.x >= b.x || a.y >= b.y);
        public static bool operator ==(Point a, Point b) => (a.x == b.x && a.y == b.y);
        public static bool operator !=(Point a, Point b) => (a.x != b.x || a.y != b.y);
        public static Point operator +(Point a) => a;
        public static Point operator -(Point a) => new Point(-a.x, -a.y);
        public static Point operator +(Point a, Point b) => new Point(a.x + b.x, a.y + b.y);
        public static Point operator -(Point a, Point b) => new Point(a.x - b.x, a.y - b.y);
        public static Point operator *(Point a, Point b) => new Point(a.x * b.x, a.y * b.y);
        public static Point operator /(Point a, Point b)
        {
            if (b.x == 0 || b.y == 0)
                throw new DivideByZeroException();
            return new Point(a.x / b.x, a.y / b.y);
        }

        public override int GetHashCode()
        {
            return (x << 2) ^ y;
        }

        public override bool Equals(object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Point p = (Point)obj;
                return this == p;
            }
        }

    }
}
