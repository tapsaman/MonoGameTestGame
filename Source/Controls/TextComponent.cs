using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameTestGame.Controls
{
    public class TextComponent : UIComponent
    {
        protected SpriteFont _font;
        protected Texture2D _texture;
        public Color PenColor = Color.Black;
        public override int Width { get { return _width; } }
        public override int Height { get { return _height; } }
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, _width, _height);
            }
        }
        private string _text;
        private int _width;
        private int _height;

        public TextComponent(SpriteFont font, string text, int maxWidth = int.MaxValue)
        {
            Focusable = false;
            _font = font;
            _text = text;
            var size = _font.MeasureString(_text);
            _width = (int)size.X;
            _height = (int)size.Y;
        }

        public override void Update(GameTime gameTime)
        {
        }
    
        public override void Draw(SpriteBatch spriteBatch)
        {
            var color = Color.White;
            
            if (!string.IsNullOrEmpty(_text)) {

                spriteBatch.DrawString(_font, _text, Position, PenColor);
            }
        }
    }
}