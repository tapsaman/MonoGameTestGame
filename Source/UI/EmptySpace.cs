using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZA6.UI
{
    public class EmptySpace : UIComponent
    {
        public EmptySpace(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public override void Update(GameTime gameTime) {}

        public override void Draw(SpriteBatch spriteBatch) {}
    }
}