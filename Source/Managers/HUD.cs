using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZA6
{
    public class HUD : IUpdatable, IDrawable
    {
        public Player Player {
            get => _player;
            set
            {
                _player = value;
                _drawnHealth = _player.Health;
            }
        }
        private Player _player;
        private Texture2D _lifeTexture;
        private int _drawnHealth;
        private float _elapsedUpdateTime;
        private const float _LOW_HP_SFX_TIME = 0.60f;
        private const float _UPDATE_TIME = 0.1f;

        public void Load()
        {
            _lifeTexture = Static.Content.Load<Texture2D>("linktothepast/life-hud");
        }
        
        public void Update(GameTime gameTime)
        {
            if (_player == null)
                return;
            
            if (_drawnHealth == _player.Health)
            {
                if (_player.CanDie && _drawnHealth < _player.MaxHealth / 3 && Static.Game.StateMachine.CurrentStateKey == "Default")
                {
                    SFX.LowHP.Play();
                }
                else
                {
                    SFX.LowHP.Stop();
                }

                return;
            }

            SFX.LowHP.Stop();
            
            _elapsedUpdateTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_elapsedUpdateTime < _UPDATE_TIME)
                return;
            
            _elapsedUpdateTime = 0f;
            
            if (_drawnHealth > _player.Health)
            {
                _drawnHealth -= 1;
            }
            else
            {
                _drawnHealth += 1;
                
                if (_drawnHealth % 2 == 0)
                {
                    SFX.LifeRefill.Play();
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_player == null)
                return;
            
            spriteBatch.Draw(_lifeTexture, new Vector2(Static.NativeWidth - 95, 15), new Rectangle(0, 0, 61, 7), Color.White);
            int health = _drawnHealth;
            int maxHealth = _player.MaxHealth;

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