using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TapsasEngine.Sprites;
using ZA6.Managers;

namespace ZA6
{
    public abstract class DialogBox
    {
        protected Texture2D _texture;
        protected Texture2D _arrowTexture;
        protected int _height;
        protected Vector2 _arrowOffset;
        protected Vector2 _nameOffset;
        protected Vector2 _textOffset;

        public virtual void Draw(SpriteBatch spriteBatch, string name, string text, bool drawArrow, bool top = false) {}
        public virtual void Draw(SpriteBatch spriteBatch, bool top, string text, float yCrop) {}
        public virtual void Draw(SpriteBatch spriteBatch, bool top, string text, float yCrop, int preservedTextHeight, Vector2 offset, bool borderless = false) {}
    }

    public class LinkToThePastDialogBox : DialogBox
    {
        public LinkToThePastDialogBox()
        {
            _texture = Static.Content.Load<Texture2D>("UI/dialogbox");
            _height = _texture.Height;
            _textOffset = new Vector2(8,6);
        }

        public override void Draw(SpriteBatch spriteBatch, string name, string text, bool drawArrow, bool top = false)
        {
            var position = new Vector2(33, top ? 36 : Static.NativeHeight - _height - 18);

            spriteBatch.Draw(_texture, position, Color.White);

            if (name != null && name != "")
            {
                text = name.ToUpper() + ":  " + text;
            }
            
            BitmapFontRenderer.DrawString(spriteBatch, text, position + _textOffset);
        }

        public override void Draw(SpriteBatch spriteBatch, bool top, string text, float yCrop)
        {
            var position = new Vector2(33, top ? 18 : Static.NativeHeight - _height - 18);

            spriteBatch.Draw(_texture, position, Color.White);
            
            BitmapFontRenderer.DrawString(spriteBatch, text, position + _textOffset, yCrop);
        }
    }

    public class LinkToThePastSectionedDialogBox : DialogBox
    {
        private int _width;
        private SectionedSprite _background;

        public LinkToThePastSectionedDialogBox()
        {
            _texture = Static.Content.Load<Texture2D>("UI/dialogbox");
            
            _width = _texture.Width;
            _textOffset = new Vector2(8,6);

            _background = new SectionedSprite(_texture, 7);
        }

        public override void Draw(SpriteBatch spriteBatch, bool top, string text, float yCrop, int preservedTextHeight, Vector2 offset, bool borderless = false)
        {
            int height = (int)(preservedTextHeight + _textOffset.Y * 1.5); // don't know why 1.5 works here but it does
            var position = new Vector2(33, top ? 38 : Static.NativeHeight - height - 18) + offset;
            BitmapFontRenderer.DrawString(spriteBatch, text, position + _textOffset, yCrop);

            if (borderless)
                return;

            _background.Draw(spriteBatch, position, _width, height);
        }
    }

    public class FantasyDialogBox : DialogBox
    {
        public FantasyDialogBox()
        {
            _texture = Static.Content.Load<Texture2D>("UI/dialogbox-fantasy-scaled");
            _arrowTexture = Static.Content.Load<Texture2D>("UI/dialogbox-fantasy-arrow-scaled");
            _height = _texture.Height;
            _arrowOffset = new Vector2(230,50);
            _nameOffset = new Vector2(44,2);
            _textOffset = new Vector2(10,17);
        }

        public override void Draw(SpriteBatch spriteBatch, string name, string text, bool drawArrow, bool top = false)
        {
            var position = new Vector2(0, top ? 0 : Static.NativeHeight - _height);

            spriteBatch.Draw(_texture, position, Color.White);

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