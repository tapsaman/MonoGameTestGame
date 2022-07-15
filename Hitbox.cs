using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TiledCS;

namespace MonoGameTestGame
{
    public class Hitbox
    {
        private Texture2D _hitbox;
        private Color[] _data;
        private int _width;
        private int _height;
        public Vector2 Position;
        public Color Color = Color.Blue;
        public bool Enabled = true;
        public Hitbox(int width, int height)
        {
            _width = width;
            _height = height;
            Load(_width, _height);
        }
        ~Hitbox()
        {
            Unload();
        }
        public void Load(int width, int height)
        {
            _width = width;
            _height = height;
            _hitbox = new Texture2D(StaticData.Graphics.GraphicsDevice, _width, _height);
            _data = new Color[_width * _height];
            for (int i = 0; i < _data.Length; i++)
            {
                _data[i] = Color.White;
            }
            _hitbox.SetData(_data);
        }
        // TODO run on scene unload
        public void Unload()
        {
            _hitbox.Dispose();
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (Enabled)
                spriteBatch.Draw(_hitbox, Position, Color * 0.5f);
        }
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, _width, _height);
            }
        }
        public bool IsColliding(Hitbox hitbox)
        {
            return Rectangle.Intersects(hitbox.Rectangle);
        }
    }
}