using System;
using System.Collections.Generic;

namespace rogueLike
{
    public struct Vector2
    {
        public int X, Y;

        public Vector2(int x, int y)
        {
            X = x; Y = y;
        }

        public static Vector2 One = new Vector2(1, 1);
        public static Vector2 UnitX = new Vector2(1, 0);
        public static Vector2 UnitY = new Vector2(0, 1);
        public static Vector2 Zero = new Vector2(0, 0);

        public static bool operator < (Vector2 lhs, Vector2 rhs) => lhs.X < rhs.X && lhs.Y < rhs.Y;
        public static bool operator > (Vector2 lhs, Vector2 rhs) => lhs.X > rhs.X && lhs.Y > rhs.Y;
        public static bool operator <= (Vector2 lhs, Vector2 rhs) => lhs.X <= rhs.X && lhs.Y <= rhs.Y;
        public static bool operator >= (Vector2 lhs, Vector2 rhs) => lhs.X >= rhs.X && lhs.Y >= rhs.Y;
        public static bool operator == (Vector2 lhs, Vector2 rhs) => lhs.X == rhs.X && lhs.Y == rhs.Y;
        public static bool operator != (Vector2 lhs, Vector2 rhs) => !(lhs == rhs);

        public static Vector2 operator + (Vector2 lhs, Vector2 rhs) => new (lhs.X + rhs.X, lhs.Y + rhs.Y);
        public static Vector2 operator - (Vector2 value) => new (value.X * -1, value.Y * -1);
        public static Vector2 operator - (Vector2 lhs, Vector2 rhs) => new(lhs.X - rhs.X, lhs.Y - rhs.Y);
        public static Vector2 operator * (int lhs, Vector2 rhs) => new(lhs * rhs.X, lhs * rhs.Y);
        public static Vector2 operator * (Vector2 lhs, Vector2 rhs) => new(lhs.X * rhs.X, lhs.Y * rhs.Y);
        public static Vector2 operator * (Vector2 lhs, int rhs) => new(lhs.X * rhs, lhs.Y * rhs);
        public static Vector2 operator / (Vector2 lhs, Vector2 rhs) => new(lhs.X / rhs.X, lhs.Y / rhs.Y);
        public static Vector2 operator /(Vector2 lhs, int rhs) => new(lhs.X / rhs, lhs.Y / rhs);

        public static int Distance(Vector2 value1, Vector2 value2)
        {
            return (int)Math.Sqrt((value1.X - value2.X) * (value1.X - value2.X) + (value1.Y - value2.Y) * (value1.Y - value2.Y));
        }

        public override bool Equals(object obj)
        {
            return obj is Vector2 vector &&
                   X == vector.X &&
                   Y == vector.Y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public static Dictionary<Vector2, Direction> ToDirection = new Dictionary<Vector2, Direction>()
            {
                {UnitX, Direction.Down},
                {-UnitX, Direction.Up},
                {UnitY, Direction.Right},
                {-UnitY, Direction.Left}
            };

        public static Dictionary<Direction, Vector2> FromDirection = new Dictionary<Direction, Vector2>()
            {
                {Direction.Down, UnitX},
                {Direction.Up, -UnitX},
                {Direction.Right, UnitY},
                {Direction.Left, -UnitY}
            };
    }
}

