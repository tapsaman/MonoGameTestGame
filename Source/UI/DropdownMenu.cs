using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TapsasEngine.Sprites;
using TapsasEngine.Utilities;

namespace ZA6.UI
{
    public class DropdownMenu : Menu
    {
        protected override void CalculateSize()
        {
            int combinedHeight = 0;

            foreach (var item in Components)
            {
                item.Width = Width - 4;
                item.Height = 14;
                
                combinedHeight += item.Height;
            }

            Height = combinedHeight + 2;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Input.P1.JustReleasedMouseLeft()
             || Input.P1.JustReleased(Input.P1.A)
             || Input.P1.JustReleased(Input.P1.B)
             || Input.P1.JustReleased(Input.P1.Select)
             || Input.P1.JustReleased(Input.P1.Start)
            )
            {
                UIManager.SetToRemove(this);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            DefaultBackground.Draw(spriteBatch, Position, Width, Height);

            var itemPosition = Position + new Vector2(2);

            foreach (var item in Components)
            {
                item.Position = itemPosition; 
                item.Draw(spriteBatch);
                itemPosition.Y += item.Height;
            }
        }
    }
}