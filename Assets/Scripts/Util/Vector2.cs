using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor.Experimental.GraphView;

namespace rogueLike
{
    public struct Vector2
    {
        public int X, Y;

        public Vector2(int x, int y)
        {
            X = x; Y = y;
        }

        public static Vector2 One = new(1, 1);
        public static Vector2 UnitX = new(1, 0);
        public static Vector2 UnitY = new(0, 1);
        public static Vector2 Zero = new(0, 0);

        public static bool operator <(Vector2 lhs, Vector2 rhs) => lhs.X < rhs.X && lhs.Y < rhs.Y;
        public static bool operator >(Vector2 lhs, Vector2 rhs) => lhs.X > rhs.X && lhs.Y > rhs.Y;
        public static bool operator <=(Vector2 lhs, Vector2 rhs) => lhs.X <= rhs.X && lhs.Y <= rhs.Y;
        public static bool operator >=(Vector2 lhs, Vector2 rhs) => lhs.X >= rhs.X && lhs.Y >= rhs.Y;
        public static bool operator ==(Vector2 lhs, Vector2 rhs) => lhs.X == rhs.X && lhs.Y == rhs.Y;
        public static bool operator !=(Vector2 lhs, Vector2 rhs) => !(lhs == rhs);

        public static Vector2 operator +(Vector2 lhs, Vector2 rhs) => new(lhs.X + rhs.X, lhs.Y + rhs.Y);
        public static Vector2 operator -(Vector2 value) => new(value.X * -1, value.Y * -1);
        public static Vector2 operator -(Vector2 lhs, Vector2 rhs) => new(lhs.X - rhs.X, lhs.Y - rhs.Y);
        public static Vector2 operator *(int lhs, Vector2 rhs) => new(lhs * rhs.X, lhs * rhs.Y);
        public static Vector2 operator *(Vector2 lhs, Vector2 rhs) => new(lhs.X * rhs.X, lhs.Y * rhs.Y);
        public static Vector2 operator *(Vector2 lhs, int rhs) => new(lhs.X * rhs, lhs.Y * rhs);
        public static Vector2 operator /(Vector2 lhs, Vector2 rhs) => new(lhs.X / rhs.X, lhs.Y / rhs.Y);
        public static Vector2 operator /(Vector2 lhs, int rhs) => new(lhs.X / rhs, lhs.Y / rhs);

        public static Dictionary<Vector2, Direction> ToDirection = new()
        {
                {UnitX, Direction.Down},
                {-UnitX, Direction.Up},
                {UnitY, Direction.Right},
                {-UnitY, Direction.Left}
        };

        public static Dictionary<Direction, Vector2> FromDirection = new()
        {
                {Direction.Down, UnitX},
                {Direction.Up, -UnitX},
                {Direction.Right, UnitY},
                {Direction.Left, -UnitY}
        };

        public static int Distance(Vector2 value1, Vector2 value2)
        {
            return (int)Math.Sqrt((value1.X - value2.X) * (value1.X - value2.X) + (value1.Y - value2.Y) * (value1.Y - value2.Y));
        }

        public override readonly bool Equals(object obj)
        {
            return obj is Vector2 vector &&
                   X == vector.X &&
                   Y == vector.Y;
        }

        public override readonly int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public static Direction GetToDirection(Vector2 vector)
        {
            if (ToDirection.TryGetValue(vector, out Direction value))
            {
                return value;
            }
            else
            {
                return Direction.None;
            }
        }

        public static Vector2 GetFromDirection(Direction direct)
        {
            if (FromDirection.TryGetValue(direct, out Vector2 value))
            {
                return value;
            }
            else
            {
                return Vector2.Zero;
            }
        }
    }
}

