using Microsoft.Xna.Framework;

namespace TapsasEngine.Enums
{
    public enum Direction
    {
        Up,
        Right,
        Down,
        Left,
        None
    }

    public static class DirectionMethods
    {
        public static Direction Opposite(this Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return Direction.Down;
                case Direction.Right:
                    return Direction.Left;
                case Direction.Down:
                    return Direction.Up;
                case Direction.Left:
                    return Direction.Right;
                default:
                    return Direction.None;
            }
        }

        public static Direction Next(this Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return Direction.Right;
                case Direction.Right:
                    return Direction.Down;
                case Direction.Down:
                    return Direction.Left;
                case Direction.Left:
                    return Direction.Up;
                default:
                    return Direction.None;
            }
        }

        public static Direction NextCounterclockwise(this Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return Direction.Left;
                case Direction.Right:
                    return Direction.Up;
                case Direction.Down:
                    return Direction.Right;
                case Direction.Left:
                    return Direction.Down;
                default:
                    return Direction.None;
            }
        }

        public static Vector2 ToVector(this Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return new Vector2(0, -1);
                case Direction.Right:
                    return new Vector2(1, 0);
                case Direction.Down:
                    return new Vector2(0, 1);
                case Direction.Left:
                    return new Vector2(-1, 0);
                default:
                    return Vector2.Zero;
            }
        }
    }
}