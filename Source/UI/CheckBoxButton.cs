using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZA6.UI
{
    public class CheckBoxButton : Button
    {
        public Vector2 Padding = new Vector2(5,5);
        public bool IsChecked;
        private Texture2D _checkBoxTexture;
        private Rectangle _checkedSourceRectangle;
        private Rectangle _uncheckedSourceRectangle;

        public CheckBoxButton(SpriteFont font, EventHandler onClick = null)
            : base(font, onClick)
        {
            _checkBoxTexture = Static.Content.Load<Texture2D>("Checkboxes");
            _uncheckedSourceRectangle = new Rectangle(0, 0, 12, 12);
            _checkedSourceRectangle = new Rectangle(12, 0, 12, 12);
        }
    
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Text != null)
            {
                TextMargin = new Vector2(
                    (12 + Padding.X * 2),
                    (Rectangle.Height / 2) - (_font.MeasureString(Text).Y / 2)
                );
            }

            base.Draw(spriteBatch);

            spriteBatch.Draw(
                _checkBoxTexture,
                new Vector2(Position.X + Padding.X, Position.Y + Padding.Y),
                IsChecked ? _checkedSourceRectangle : _uncheckedSourceRectangle,
                Color.White
            );
        }
    }
}