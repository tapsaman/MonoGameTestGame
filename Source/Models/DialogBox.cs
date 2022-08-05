using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTestGame.Managers;

namespace MonoGameTestGame
{
    public abstract class DialogBox
    {
        protected Texture2D _bgTexture;
        protected Texture2D _arrowTexture;
        protected int _height;
        protected Vector2 _arrowOffset;
        protected Vector2 _nameOffset;
        protected Vector2 _textOffset;

        public virtual void Draw(SpriteBatch spriteBatch, string name, string text, bool drawArrow, bool top = false) {}
        public virtual void Draw(SpriteBatch spriteBatch, bool top, string text, float yCrop) {}
        public virtual void Draw(SpriteBatch spriteBatch, bool top, string text, float yCrop, int preservedTextHeight, bool borderless = false, int? highlightRow = null) {}
    }

    public class LinkToThePastDialogBox : DialogBox
    {
        public LinkToThePastDialogBox()
        {
            _bgTexture = Static.Content.Load<Texture2D>("linktothepast/dialogbox");
            _height = _bgTexture.Height;
            _textOffset = new Vector2(8,6);
        }

        public override void Draw(SpriteBatch spriteBatch, string name, string text, bool drawArrow, bool top = false)
        {
            var position = new Vector2(33, top ? 18 : Static.NativeHeight - _height - 18);

            spriteBatch.Draw(_bgTexture, position, Color.White);

            if (name != null && name != "")
            {
                text = name.ToUpper() + ":  " + text;
            }
            
            BitmapFontRenderer.DrawString(spriteBatch, text, position + _textOffset);
        }

        public override void Draw(SpriteBatch spriteBatch, bool top, string text, float yCrop)
        {
            var position = new Vector2(33, top ? 18 : Static.NativeHeight - _height - 18);

            spriteBatch.Draw(_bgTexture, position, Color.White);
            
            BitmapFontRenderer.DrawString(spriteBatch, text, position + _textOffset, yCrop);
        }
    }

    public class LinkToThePastSectionedDialogBox : DialogBox
    {
        private int _width;
        private Rectangle _sourceTop;
        private Rectangle _sourceBottom;
        private Rectangle _sourceRight;
        private Rectangle _sourceLeft;
        private Rectangle _sourceTopLeft;
        private Rectangle _sourceTopRight;
        private Rectangle _sourceBottomLeft;
        private Rectangle _sourceBottomRight;

        public LinkToThePastSectionedDialogBox()
        {
            _bgTexture = Static.Content.Load<Texture2D>("linktothepast/dialogbox");
            
            int textureHeight = _bgTexture.Height;
            _width = _bgTexture.Width;
            _textOffset = new Vector2(8,6);

            int cornerLength = 7;
            _sourceTop = new Rectangle(cornerLength,0,1,cornerLength);
            _sourceBottom = new Rectangle(cornerLength, textureHeight - cornerLength, 1, cornerLength);
            _sourceLeft = new Rectangle(0,cornerLength,cornerLength,1);
            _sourceRight = new Rectangle(_width - cornerLength, cornerLength,cornerLength,1);

            _sourceTopLeft = new Rectangle(0,0,cornerLength,cornerLength);
            _sourceTopRight = new Rectangle(_width - cornerLength,0,cornerLength,cornerLength);
            _sourceBottomLeft = new Rectangle(0, textureHeight - cornerLength,cornerLength,cornerLength);
            _sourceBottomRight = new Rectangle(_width - cornerLength, textureHeight - cornerLength,cornerLength,cornerLength);
        }

        public override void Draw(SpriteBatch spriteBatch, bool top, string text, float yCrop, int preservedTextHeight, bool borderless = false, int? highlightRow = null)
        {
            int height = (int)(preservedTextHeight + _textOffset.Y * 1.5); // don't know why 1.5 works here but it does
            var position = new Vector2(33, top ? 18 : Static.NativeHeight - height - 18);
            BitmapFontRenderer.DrawString(spriteBatch, text, position + _textOffset, yCrop, highlightRow);

            if (borderless)
                return;

            int cornerLength = 7;

            Vector2 xScale = new Vector2(_width, 1);
            Vector2 yScale = new Vector2(1, height);

            // atm corners render on top of borders, this could be fixed
            // Top
            spriteBatch.Draw(_bgTexture, position, _sourceTop, Color.White, 0, Vector2.Zero, xScale, SpriteEffects.None, 0);
            // Bottom
            spriteBatch.Draw(_bgTexture, position + new Vector2(0, height), _sourceBottom, Color.White, 0, Vector2.Zero, xScale, SpriteEffects.None, 0);
            // Left
            spriteBatch.Draw(_bgTexture, position, _sourceLeft, Color.White, 0, Vector2.Zero, yScale, SpriteEffects.None, 0);
            // Right    
            spriteBatch.Draw(_bgTexture, position + new Vector2(_width - cornerLength, 0), _sourceRight, Color.White, 0, Vector2.Zero, yScale, SpriteEffects.None, 0);

            // Top left
            spriteBatch.Draw(_bgTexture, position, _sourceTopLeft, Color.White);
            // Top right
            spriteBatch.Draw(_bgTexture, position + new Vector2(_width - cornerLength, 0), _sourceTopRight, Color.White);
            // Bottom left
            spriteBatch.Draw(_bgTexture, position + new Vector2(0, height), _sourceBottomLeft, Color.White);
            // Bottom right
            spriteBatch.Draw(_bgTexture, position + new Vector2(_width - cornerLength, height), _sourceBottomRight, Color.White);
            
        }
    }

    public class FantasyDialogBox : DialogBox
    {
        public FantasyDialogBox()
        {
            _bgTexture = Static.Content.Load<Texture2D>("dialogbox-fantasy-scaled");
            _arrowTexture = Static.Content.Load<Texture2D>("dialogbox-fantasy-arrow-scaled");
            _height = _bgTexture.Height;
            _arrowOffset = new Vector2(230,50);
            _nameOffset = new Vector2(44,2);
            _textOffset = new Vector2(10,17);
        }

        public override void Draw(SpriteBatch spriteBatch, string name, string text, bool drawArrow, bool top = false)
        {
            var position = new Vector2(0, top ? 0 : Static.NativeHeight - _height);

            spriteBatch.Draw(_bgTexture, position, Color.White);

            if (drawArrow)
            {
                spriteBatch.Draw(_arrowTexture, position + _arrowOffset, Color.White);
            }

            //spriteBatch.DrawString(Static.Font, name, position + _nameOffset, Color.White);
            //spriteBatch.DrawString(Static.Font, text, position + _textOffset, Color.Black);

            BitmapFontRenderer.DrawString(spriteBatch, name, position + _nameOffset);
            BitmapFontRenderer.DrawString(spriteBatch, text, position + _textOffset);
        }

        public override void Draw(SpriteBatch spriteBatch, bool top, string text, float yCrop)
        {
            throw new System.NotImplementedException();
        }
    }
}