using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZA6
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
        public Vector2 Center
        {
            get 
            { 
                return new Vector2(X + Width / 2, Y + Height / 2); 
            }
        }
        public bool Intersects(FloatRectangle r)
        {
            return !(r.Left > Right
                || r.Right < Left
                || r.Top > Bottom
                || r.Bottom < Top
            );
        }
        public bool Intersects(Vector2 v)
        {
            return !(v.X > Right
                || v.X < Left
                || v.Y > Bottom
                || v.Y < Top
            );
        }
        public bool Intersects(Point p)
        {
            return !(p.X > Right
                || p.X < Left
                || p.Y > Bottom
                || p.Y < Top
            );
        }

        public bool Contains(FloatRectangle value)
        {
            return ((((this.X <= value.X) && ((value.X + value.Width) <= (this.X + this.Width))) && (this.Y <= value.Y)) && ((value.Y + value.Height) <= (this.Y + this.Height)));
        }
    }
}