using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameTestGame
{
    public abstract class DialogBox
    {
        public Texture2D _bgTexture;
        public Texture2D _arrowTexture;
        public abstract void Draw(SpriteBatch spriteBatch, string name, string text, bool drawArrow);
    }

    public class FantasyDialogBox : DialogBox
    {
        public FantasyDialogBox()
        {
            _bgTexture = StaticData.Content.Load<Texture2D>("dialogbox-fantasy");
            _arrowTexture = StaticData.Content.Load<Texture2D>("dialogbox-fantasy-arrow");
        }

        public override void Draw(SpriteBatch spriteBatch, string name, string text, bool drawArrow)
        {
            spriteBatch.Draw(
                _bgTexture,
                new Vector2(50, 350),
                null,
                Color.White,
                0f,
                Vector2.Zero,
                0.5f,
                SpriteEffects.None,
                0f
            );
            if (drawArrow)
            {
                spriteBatch.Draw(
                    _arrowTexture,
                    new Vector2(400, 430),
                    null,
                    Color.White,
                    0f,
                    Vector2.Zero,
                    0.5f,
                    SpriteEffects.None,
                    0f
                );
            }
            spriteBatch.DrawString(StaticData.Font, name, new Vector2(150, 360), Color.White);
            spriteBatch.DrawString(StaticData.Font, text, new Vector2(75, 390), Color.Black);
        }
    }
}