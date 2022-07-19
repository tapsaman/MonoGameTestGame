using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameTestGame
{
    public class FloatRectangle
    {
        public float X;
        public float Y;
        public float Width;
        public float Height;
        public FloatRectangle(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
        public float Left
        {
            get { return X; }
        }

        public float Right
        {
            get { return X + Width; }
        }

        public float Top
        {
            get { return Y; }
        }

        public float Bottom
        {
            get { return Y + Height; }
        }
        public bool Intersects(FloatRectangle r)
        {
            return !(r.Left > Right
                || r.Right < Left
                || r.Top > Bottom
                || r.Bottom < Top
            );
        }
        public Vector2 Center
        {
            get 
            { 
                return new Vector2((X + Width) / 2, (Y + Height) / 2); 
            }
        }
    }
}