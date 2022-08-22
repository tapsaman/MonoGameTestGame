using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TapsasEngine.Sprites;
using TapsasEngine.Utilities;

namespace ZA6.UI
{
    public class DropdownMenu : UIContainer
    {
        protected override void CalculateSize()
        {
            int combinedHeight = 0;

            foreach (var item in Components)
            {
                item.Width = Width - 4;
                item.Height = 18;
                
                combinedHeight += item.Height;
            }

            Height = combinedHeight + 4;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            DefaultBackground.Draw(spriteBatch, Position, Width, Height, Color.White);

            var itemPosition = Position + new Vector2(2);

            foreach (var item in Components)
            {
                item.Draw(spriteBatch);
                itemPosition.Y += item.Height;
            }
        }
    }
}