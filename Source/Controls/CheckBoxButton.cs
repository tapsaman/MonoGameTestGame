using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameTestGame.Controls
{
    public class CheckBoxButton : Button
    {
        public bool IsChecked;

        public CheckBoxButton(Texture2D texture, SpriteFont font)
            : base(texture, font) {}
    
        public override void Draw(SpriteBatch spriteBatch)
        {
            var color = Color.White;
            
            if (_isActive)
                color = Color.Red;
            else if ((IsFocused == null && _isHovering) || IsFocused == true)
                color = Color.Gray;

            spriteBatch.Draw(_texture, Rectangle, color);

            if (IsChecked)
            {
                spriteBatch.DrawString(_font, "X", new Vector2(Rectangle.X, Rectangle.Y), PenColor);
                
            }

            if (!string.IsNullOrEmpty(Text))
            {
                var x = (Rectangle.X + 20 /* checkbox width and padding */);
                var y = (Rectangle.Y + (Rectangle.Height / 2)) - (_font.MeasureString(Text).Y / 2);

                spriteBatch.DrawString(_font, Text, new Vector2(x, y), PenColor);
            }
        }
    }
}