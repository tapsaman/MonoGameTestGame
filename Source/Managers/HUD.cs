using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameTestGame
{
    public class HUD
    {
        private Texture2D _lifeTexture;

        public void Load()
        {
            _lifeTexture = Static.Content.Load<Texture2D>("linktothepast/life-hud");
        }

        public void Draw(SpriteBatch spriteBatch, Player player)
        {
            spriteBatch.Draw(_lifeTexture, new Vector2(Static.NativeWidth - 95, 15), new Rectangle(0, 0, 61, 7), Color.White);
            int health = player.Health;
            int maxHealth = player.MaxHealth;

            for (int i = 0; i < maxHealth; i += 2)
            {
                if (health >= 2)
                {
                    // Full heart
                    spriteBatch.Draw(_lifeTexture, new Vector2(Static.NativeWidth - 95 + (int)(4 * i), 24), new Rectangle(8, 9, 7, 7), Color.White);
                }
                else if (health >= 1)
                {
                    // Half heart
                    spriteBatch.Draw(_lifeTexture, new Vector2(Static.NativeWidth - 95 + (int)(4 * i), 24), new Rectangle(24, 9, 7, 7), Color.White);
                }
                else
                {
                    // Empty heart
                    spriteBatch.Draw(_lifeTexture, new Vector2(Static.NativeWidth - 95 + (int)(4 * i), 24), new Rectangle(0, 9, 7, 7), Color.White);
                }

                health -= 2;
            }
        }
    }
}