using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TapsasEngine.Enums;
using ZA6;

namespace TapsasEngine.Utilities
{
    public static class Utility
    {
        private static Random _random = new Random();
        private static Texture2D _fullScreenOverlay = Utility.CreateColorTexture(Static.NativeWidth, Static.NativeHeight, Color.White);
        private static Texture2D _pixelTexture;

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

        /* Rounds to closest number divisible by divider or zero */
        public static float RoundTo(float value, float divider)
        {
            return (float)Math.Round(value / divider) * divider;
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

        public static Texture2D GetPixelTexture(SpriteBatch spriteBatch)
        {
            if (_pixelTexture == null)
            {
                _pixelTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
                _pixelTexture.SetData(new[] {Color.White});
            }

            return _pixelTexture;
        }
    }
}