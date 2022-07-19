using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameTestGame
{
    public class Hitbox
    {
        private Texture2D _texture;
        private Color[] _data;
        private int _width;
        private int _height;
        public Vector2 Position;
        public float x;
        public Color Color = Color.Blue;
        public bool Enabled = true;
        public Hitbox() {}
        public Hitbox(int width, int height)
        {
            Load(width, height);
        }
        ~Hitbox()
        {
            Unload();
        }
        public void Load(int width, int height)
        {
            _width = width;
            _height = height;
            _texture = new Texture2D(StaticData.Graphics.GraphicsDevice, _width, _height);
            _data = new Color[_width * _height];
            for (int i = 0; i < _data.Length; i++)
            {
                _data[i] = Color.White;
            }
            _texture.SetData(_data);
        }
        // TODO run on scene unload
        public void Unload()
        {
            if (_texture != null)
                _texture.Dispose();
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (Enabled)
                spriteBatch.Draw(_texture, Position, Color * 0.5f);
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