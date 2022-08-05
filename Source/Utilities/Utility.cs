using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameTestGame
{
    public static class Utility
    {
        private static Random _random = new Random();
        private static Texture2D _fullScreenOverlay = Utility.CreateColorTexture(Static.NativeWidth, Static.NativeHeight, Color.White);

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

        public static float GetSeconds(this GameTime gameTime)
        {
            return (float)gameTime.ElapsedGameTime.TotalSeconds;
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

        public static Vector2 ToAbsolute(this Vector2 vector)
        {
            return new Vector2(Math.Abs(vector.X), Math.Abs(vector.Y));
        }

        public static float ToAbsoluteFloat(this Vector2 vector)
        {
            return Math.Abs(vector.X) + Math.Abs(vector.Y);
        }

        // Must be disposed!
        public static Texture2D CreateColorTexture(int width, int height, Color color)
        {
            var texture = new Texture2D(Static.Graphics.GraphicsDevice, width, height);
            var data = new Color[width * height];

            for (int i = 0; i < data.Length; i++)
            {
                data[i] = color;
            }

            texture.SetData(data);

            return texture;
        }

        public static void DrawOverlay(SpriteBatch spriteBatch, Color color)
        {
            spriteBatch.Draw(_fullScreenOverlay, Vector2.Zero, color);
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