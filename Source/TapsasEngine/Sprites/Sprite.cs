using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TapsasEngine.Sprites
{
    public class Sprite
    {
        public Color Color = Color.White;
        public Texture2D Texture;
        public Rectangle? SourceRectangle;

        protected Sprite() {}
    
        public Sprite(Texture2D texture, Rectangle? sourceRectangle = null)
        {
            Texture = texture;
            SourceRectangle = sourceRectangle;
        }

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(Texture, position, SourceRectangle, Color);
        }

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 position, int width, int height)
        {
            var scale = new Vector2(width / Texture.Width, height / Texture.Height);

            spriteBatch.Draw(
                Texture,
                position,
                SourceRectangle,
                Color
            );
        }
    }
}