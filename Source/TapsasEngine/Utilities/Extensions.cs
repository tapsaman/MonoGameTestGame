using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TapsasEngine.Enums;

namespace TapsasEngine.Utilities
{
    public static class Extensions
    {
        /* string */

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

        /* Microsoft.Xna.Framework.GameTime */
        
        public static float GetSeconds(this GameTime gameTime)
        {
            return (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        /* Microsoft.Xna.Framework.Vector2 */
        
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

        /* Microsoft.Xna.Framework.Graphics.SpriteBatch */

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
            spriteBatch.Draw(Utility.GetPixelTexture(spriteBatch), point, null, color, angle, origin, scale, SpriteEffects.None, 0);
        }
    }
}