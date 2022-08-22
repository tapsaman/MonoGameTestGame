using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZA6.UI
{
    public class TextComponent : UIComponent
    {
        protected SpriteFont _font;
        protected Texture2D _texture;
        public Color PenColor = Color.Black;
        private string _text;

        public TextComponent(SpriteFont font, string text, int maxWidth = int.MaxValue)
        {
            _font = font;
            _text = text;
            var size = _font.MeasureString(_text);
            Width = (int)size.X;
            Height = (int)size.Y;
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