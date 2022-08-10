using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZA6.Controls
{
    public class CheckBoxButton : Button
    {
        public Vector2 Padding = new Vector2(5,5);
        public bool IsChecked;
        private Texture2D _checkBoxTexture;
        private Rectangle _checkedSourceRectangle;
        private Rectangle _uncheckedSourceRectangle;

        public CheckBoxButton(Texture2D texture, SpriteFont font)
            : base(texture, font)
        {
            _checkBoxTexture = Static.Content.Load<Texture2D>("Checkboxes");
            _uncheckedSourceRectangle = new Rectangle(0, 0, 18, 18);
            _checkedSourceRectangle = new Rectangle(18, 0, 18, 18);
        }
    
        public override void Draw(SpriteBatch spriteBatch)
        {
            var color = Color.White;
            
            if (_isActive)
                color = ActiveColor;
            else if (_isHovering || IsFocused == true)
                color = Color.Gray;

            spriteBatch.Draw(_texture, Rectangle, color);

            spriteBatch.Draw(
                _checkBoxTexture,
                new Vector2(Position.X + Padding.X, Position.Y + Padding.Y),
                IsChecked ? _checkedSourceRectangle : _uncheckedSourceRectangle,
                Color.White
            );

            if (!string.IsNullOrEmpty(Text))
            {
                var x = (Rectangle.X + 18 + Padding.X * 2);
                var y = (Rectangle.Y + (Rectangle.Height / 2)) - (_font.MeasureString(Text).Y / 2);

                spriteBatch.DrawString(_font, Text, new Vector2(x, y), PenColor);
            }
        }
    }
}