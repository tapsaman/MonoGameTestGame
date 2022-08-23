using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TapsasEngine.Sprites;
using TapsasEngine.Utilities;

namespace ZA6.UI
{
    public class Menu : UIContainer
    {
        public Color? OverlayColor = null;
        public Sides Margin;
        public Menu Replacing;
        public SectionedSprite Background;
        private Rectangle _contentRectangle;
        private Rectangle _outerRectangle;

        protected override void CalculateSize()
        {
            int greatestWidth = 0;
            int combinedHeight = 0;
            int combinedPadding = Padding * Components.Length - 1;

            foreach (var item in Components)
            {
                if (greatestWidth < item.Width)
                    greatestWidth = item.Width;
                
                combinedHeight += item.Height;
            }

            int contentWidth = greatestWidth;
            int contentHeight = combinedHeight + combinedPadding;

            _contentRectangle = new Rectangle(
                Static.NativeWidth / 2 - contentWidth / 2 + Margin.Left,
                Static.NativeHeight / 2 - contentHeight / 2 + Margin.Top,
                contentWidth,
                contentHeight
            );

            _outerRectangle = new Rectangle(
                _contentRectangle.X - Padding,
                _contentRectangle.Y - Padding,
                _contentRectangle.Width + Padding * 2,
                _contentRectangle.Height + Padding * 2
            );
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (OverlayColor != null)
            {
                Utility.DrawOverlay(spriteBatch, (Color)OverlayColor);
            }

            if (Background != null)
            {
                Background.Draw(spriteBatch, _outerRectangle);
            }

            float y = _contentRectangle.Y;

            foreach (var item in Components)
            {
                // Center items 
                int x = _contentRectangle.X + _contentRectangle.Width / 2 - item.Width / 2;
                item.Position = new Vector2(x, y);
                item.Draw(spriteBatch);
                y += item.Height + Padding;
            }
        }
    }
}