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

        public abstract void Draw(SpriteBatch spriteBatch, string name, string text, bool drawArrow, bool top = false);
    }

    public class LinkToThePastDialogBox : DialogBox
    {
        public LinkToThePastDialogBox()
        {
            _bgTexture = StaticData.Content.Load<Texture2D>("linktothepast-dialogbox");
            _arrowTexture = StaticData.Content.Load<Texture2D>("dialogbox-fantasy-arrow-scaled");
            _height = _bgTexture.Height;
            _arrowOffset = new Vector2(230,50);
            _textOffset = new Vector2(8,6);
        }

        public override void Draw(SpriteBatch spriteBatch, string name, string text, bool drawArrow, bool top = false)
        {
            var position = new Vector2(33, top ? 18 : StaticData.NativeHeight - _height - 18);

            spriteBatch.Draw(_bgTexture, position, Color.White);

            if (drawArrow)
            {
                spriteBatch.Draw(_arrowTexture, position + _arrowOffset, Color.White);
            }

            if (name != null && name != "")
            {
                text = name.ToUpper() + ":  " + text;
            }
            
            BitmapFontRenderer.DrawString(spriteBatch, text, position + _textOffset);
        }
    }

    public class FantasyDialogBox : DialogBox
    {
        public FantasyDialogBox()
        {
            _bgTexture = StaticData.Content.Load<Texture2D>("dialogbox-fantasy-scaled");
            _arrowTexture = StaticData.Content.Load<Texture2D>("dialogbox-fantasy-arrow-scaled");
            _height = _bgTexture.Height;
            _arrowOffset = new Vector2(230,50);
            _nameOffset = new Vector2(44,2);
            _textOffset = new Vector2(10,17);
        }

        public override void Draw(SpriteBatch spriteBatch, string name, string text, bool drawArrow, bool top = false)
        {
            var position = new Vector2(0, top ? 0 : StaticData.NativeHeight - _height);

            spriteBatch.Draw(_bgTexture, position, Color.White);

            if (drawArrow)
            {
                spriteBatch.Draw(_arrowTexture, position + _arrowOffset, Color.White);
            }

            //spriteBatch.DrawString(StaticData.Font, name, position + _nameOffset, Color.White);
            //spriteBatch.DrawString(StaticData.Font, text, position + _textOffset, Color.Black);

            BitmapFontRenderer.DrawString(spriteBatch, name, position + _nameOffset);
            BitmapFontRenderer.DrawString(spriteBatch, text, position + _textOffset);
        }
    }
}