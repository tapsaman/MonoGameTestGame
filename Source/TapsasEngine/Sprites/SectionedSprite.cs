using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TapsasEngine.Sprites
{
    public class SectionedSprite : Sprite
    {
        private int _borderWidth;
        private Rectangle _sourceBackground;
        private Rectangle _sourceTop;
        private Rectangle _sourceBottom;
        private Rectangle _sourceRight;
        private Rectangle _sourceLeft;
        private Rectangle _sourceTopLeft;
        private Rectangle _sourceTopRight;
        private Rectangle _sourceBottomLeft;
        private Rectangle _sourceBottomRight;

        public SectionedSprite(Texture2D texture, int borderWidth)
            : base(texture)
        {
            _borderWidth = borderWidth;

            int height = Texture.Height;
            int width = Texture.Width;

            _sourceBackground = new Rectangle(_borderWidth, _borderWidth, width - _borderWidth * 2, height - _borderWidth * 2);

            _sourceTop = new Rectangle(_borderWidth, 0, 1, _borderWidth);
            _sourceBottom = new Rectangle(_borderWidth, height - _borderWidth, 1, _borderWidth);
            _sourceLeft = new Rectangle(0, _borderWidth, _borderWidth, 1);
            _sourceRight = new Rectangle(width - _borderWidth, _borderWidth, _borderWidth, 1);

            _sourceTopLeft = new Rectangle(0, 0, _borderWidth, _borderWidth);
            _sourceTopRight = new Rectangle(width - _borderWidth, 0, _borderWidth, _borderWidth);
            _sourceBottomLeft = new Rectangle(0, height - _borderWidth, _borderWidth, _borderWidth);
            _sourceBottomRight = new Rectangle(width - _borderWidth, height - _borderWidth, _borderWidth, _borderWidth);
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle drawTarget)
        {
            Draw(spriteBatch, new Vector2(drawTarget.X, drawTarget.Y), drawTarget.Width, drawTarget.Height);
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 position, int width, int height)
        {
            if (height == Texture.Height && width == Texture.Width)
            {
                Draw(spriteBatch, position);
                return;
            }

            Vector2 backgroundScale = new Vector2((float)width / _sourceBackground.Width, (float)height / _sourceBackground.Height);
            Vector2 xScale = new Vector2(width, 1);
            Vector2 yScale = new Vector2(1, height);

            // Backgorund
            spriteBatch.Draw(Texture, position, _sourceBackground, Color, 0, Vector2.Zero, backgroundScale, SpriteEffects.None, 0);

            // atm corners render on top of borders, this could be fixed
            // Top
            spriteBatch.Draw(Texture, position, _sourceTop, Color, 0, Vector2.Zero, xScale, SpriteEffects.None, 0);
            // Bottom
            spriteBatch.Draw(Texture, position + new Vector2(0, height), _sourceBottom, Color, 0, Vector2.Zero, xScale, SpriteEffects.None, 0);
            // Left
            spriteBatch.Draw(Texture, position, _sourceLeft, Color, 0, Vector2.Zero, yScale, SpriteEffects.None, 0);
            // Right    
            spriteBatch.Draw(Texture, position + new Vector2(width - _borderWidth, 0), _sourceRight, Color, 0, Vector2.Zero, yScale, SpriteEffects.None, 0);

            // Top left
            spriteBatch.Draw(Texture, position, _sourceTopLeft, Color);
            // Top right
            spriteBatch.Draw(Texture, position + new Vector2(width - _borderWidth, 0), _sourceTopRight, Color);
            // Bottom left
            spriteBatch.Draw(Texture, position + new Vector2(0, height), _sourceBottomLeft, Color);
            // Bottom right
            spriteBatch.Draw(Texture, position + new Vector2(width - _borderWidth, height), _sourceBottomRight, Color);
        }
    }
}