using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameTestGame
{
    public static class Utility
    {
        private static Random _random = new Random();

        public static int RandomBetween(int lowest, int highest)
        {
            return _random.Next(lowest, highest + 1);
        }

        public static double RandomDouble()
        {
            return _random.NextDouble();
        }

        public static Direction RandomDirection()
        {
            return (Direction)RandomBetween(0, 3);
        }

        public static Direction GetDirectionBetween(Vector2 from, Vector2 to)
        {
            Vector2 difference = from - to;

            if (Math.Abs(difference.X) > Math.Abs(difference.Y))
            {
                return difference.X > 0 ? Direction.Left : Direction.Right;
            }
            else
            {
                return difference.Y > 0 ? Direction.Up : Direction.Down;
            }
        }

        public static Direction ToDirection(this Vector2 vector)
        {
            if (vector == Vector2.Zero)
                return Direction.None;

            if (Math.Abs(vector.X) > Math.Abs(vector.Y))
            {
                return vector.X > 0 ? Direction.Right : Direction.Left;
            }
            else
            {
                return vector.Y > 0 ? Direction.Down : Direction.Up;
            }
        }

        // Must be disposed!
        public static Texture2D CreateColorTexture(int width, int height, Color color)
        {
            var texture = new Texture2D(StaticData.Graphics.GraphicsDevice, width, height);
            var data = new Color[width * height];

            for (int i = 0; i < data.Length; i++)
            {
                data[i] = Color.White;
            }

            texture.SetData(data);

            return texture;
        }

        public static int IndexOfNth(this string source, char toFind, int n, int fromIndex = 0)
        {
            int index = -1;

            for (int i = 0; i < n; i++)
            {
                index = source.IndexOf(toFind, fromIndex);

                if (index == -1)
                    return -1;
            
                fromIndex = index + 1;
            }

            return index;
        }
    }
}