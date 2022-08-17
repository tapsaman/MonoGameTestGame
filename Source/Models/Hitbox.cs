using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZA6
{
    public class Hitbox
    {
        public Vector2 Position;
        public float x;
        public Color Color = Color.Yellow;
        public bool Enabled = true;
        private Texture2D _texture;
        private int _width;
        private int _height;

        public Hitbox()
        {
            Static.Scene.RegisterHitbox(this);
        }
        public Hitbox(int width, int height)
        {
            Load(width, height);
            Static.Scene.RegisterHitbox(this);
        }
        ~Hitbox()
        {
            Dispose();
        }
        public void Load(int width, int height)
        {
            _width = width;
            _height = height;
            _texture = Utility.CreateColorTexture(_width, _height, Color.White);
        }
        // TODO run on scene unload
        public void Dispose()
        {
            if (_texture != null)
                _texture.Dispose();
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            if (Enabled)
                spriteBatch.Draw(_texture, Position + offset, Color * 0.5f);
        }
        public FloatRectangle Rectangle
        {
            get
            {
                return new FloatRectangle(Position.X, Position.Y, _width, _height);
            }
        }
        public bool IsColliding(Hitbox hitbox)
        {
            return Rectangle.Intersects(hitbox.Rectangle);
        }
    }
}