using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZA6
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

        public static Vector2 RandomDiagonal()
        {
            int r = RandomBetween(0,3);
            Vector2 v = Vector2.Zero;

            switch (r)
            {
                case 0: v = new Vector2(1,-1); break;
                case 1: v = new Vector2(1,1); break;
                case 2: v = new Vector2(-1,1); break;
                case 3: v = new Vector2(-1,-1); break;
            }

            v.Normalize();

            return v;
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
            var texture = new Texture2D(Static.Renderer.Device, width, height);
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

        private static Texture2D _texture;
        private static Texture2D GetTexture(SpriteBatch spriteBatch)
        {
            if (_texture == null)
            {
                _texture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
                _texture.SetData(new[] {Color.White});
            }

            return _texture;
        }

        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 point1, Vector2 point2, Color color, float thickness = 1f)
        {
            var distance = Vector2.Distance(point1, point2);
            var angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            DrawLine(spriteBatch, point1, distance, angle, color, thickness);
        }

        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 point, float length, float angle, Color color, float thickness = 1f)
        {
            var origin = new Vector2(0f, 0.5f);
            var scale = new Vector2(length, thickness);
            spriteBatch.Draw(GetTexture(spriteBatch), point, null, color, angle, origin, scale, SpriteEffects.None, 0);
        }
    }
}