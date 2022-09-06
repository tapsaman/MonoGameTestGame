using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using TapsasEngine;
using TapsasEngine.Sprites;
using TapsasEngine.Utilities;

namespace ZA6
{
    public class HUD : IUpdate, IDraw
    {
        public bool Drawing = true;
        public Player Player {
            get => _player;
            set
            {
                _player = value;
                _lifeHUD = new LifeHUD() { MaxHealth = _player.MaxHealth };
                _rupeeHUD = new RupeeHUD() { Rupees = _player.Rupees };
            }
        }
        private Player _player;
        private LifeHUD _lifeHUD;
        private RupeeHUD _rupeeHUD;

        public void Update(GameTime gameTime)
        {
            if (_player == null)
                return;

            _rupeeHUD.Update(gameTime, _player.Rupees);
            _lifeHUD.Update(
                gameTime,
                _player.Health,
                !_player.CanDie || Static.Game.StateMachine.CurrentStateKey != "Default"
            );
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!Drawing || _player == null)
                return;

            _rupeeHUD.Draw(spriteBatch);
            _lifeHUD.Draw(spriteBatch);
        }
    }

    public class LifeHUD : IDraw
    {
        public int MaxHealth
        {
            get => _maxHealth;
            set
            {
                _maxHealth = value;
                _drawnSprites = new Sprite[_maxHealth /  2];
            }
        }
        public static SoundEffectInstance LowHPSound;
        private Sprite[] _drawnSprites;
        private Sprite _emptyHeart = new Sprite(Img.LifeHUD, new Rectangle(0, 9, 7, 7));
        private Sprite _fullHeart = new Sprite(Img.LifeHUD, new Rectangle(8, 9, 7, 7));
        private Sprite _halfHeart = new Sprite(Img.LifeHUD, new Rectangle(24, 9, 7, 7));
        private int _maxHealth;
        private int _drawnHealth;
        private float _elapsedTime;
        private const float UPDATE_TIME = 0.1f;

        public LifeHUD()
        {
            LowHPSound = SFX.LowHP.CreateInstance();
            LowHPSound.Volume = 0.4f;
            LowHPSound.IsLooped = true;
        }

        public void Update(GameTime gameTime, int currentHealth, bool mute)
        {
            if (!mute && _drawnHealth < MaxHealth / 3)
            {
                LowHPSound.Play();
            }
            else
            {
                LowHPSound.Stop();
            }

            _elapsedTime += gameTime.GetSeconds();

            if (_drawnHealth == currentHealth || _elapsedTime < UPDATE_TIME)
                return;

            _elapsedTime = 0;

            if (_drawnHealth > currentHealth)
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

            int health = _drawnHealth;
            int spriteIndex = 0;

            for (int i = 0; i < _maxHealth; i += 2)
            {
                if (health > 1)
                {
                    _drawnSprites[spriteIndex] = _fullHeart;
                }
                else if (health == 1)
                {
                    _drawnSprites[spriteIndex] = _halfHeart;
                }
                else
                {
                    _drawnSprites[spriteIndex] = _emptyHeart;
                }

                spriteIndex += 1;
                health -= 2;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Img.LifeHUD, new Vector2(Static.NativeWidth - 95, 15), new Rectangle(0, 0, 61, 7), Color.White);
            
            Vector2 heartPosition = new Vector2(Static.NativeWidth - 95, 24);

            for (int i = 0; i < _drawnSprites.Length; i++)
            {
                if (_drawnSprites[i] == null)
                    return;
                
                _drawnSprites[i].Draw(spriteBatch, heartPosition);
                heartPosition.X += 8;
            }
        }
    }

    public class RupeeHUD : IDraw
    {
        public const float UPDATE_TIME = 0.015f;
        public int Rupees { set => _drawnRupees = value; } 
        private static Sprite[] _numberSprites = new Sprite[]
        {
            new Sprite(Img.RupeeHUD, new Rectangle(0, 9, 7, 7)),
            new Sprite(Img.RupeeHUD, new Rectangle(7, 9, 7, 7)),
            new Sprite(Img.RupeeHUD, new Rectangle(14, 9, 7, 7)),
            new Sprite(Img.RupeeHUD, new Rectangle(21, 9, 7, 7)),
            new Sprite(Img.RupeeHUD, new Rectangle(28, 9, 7, 7)),
            new Sprite(Img.RupeeHUD, new Rectangle(0, 16, 7, 7)),
            new Sprite(Img.RupeeHUD, new Rectangle(7, 16, 7, 7)),
            new Sprite(Img.RupeeHUD, new Rectangle(14, 16, 7, 7)),
            new Sprite(Img.RupeeHUD, new Rectangle(21, 16, 7, 7)),
            new Sprite(Img.RupeeHUD, new Rectangle(28, 16, 7, 7)),
        };
        private const int _MAX_CHARS = 5;
        private float _elapsedTime;
        private int _drawnRupees;
        private bool _initDone;
        private SoundEffectInstance _sfx;
        private Sprite[] _drawnSprites = new Sprite[_MAX_CHARS];
        private Sprite _head = new Sprite(Img.RupeeHUD, new Rectangle(0, 0, 15, 8));
        private Sprite _minusSprite = new Sprite(Img.RupeeHUD, new Rectangle(28, 2, 7, 7));

        public RupeeHUD()
        {
            _sfx = SFX.Rupee.CreateInstance();
        }

        public void Update(GameTime gameTime, int currentRupees)
        {
            _elapsedTime += gameTime.GetSeconds();

            if (_drawnRupees == currentRupees)
            {
                _initDone = true;
                return;
            }

            if (!_initDone)
            {
                _drawnRupees = (int)(currentRupees * Math.Min(1f, _elapsedTime / 1f));
            }
            else if (_elapsedTime < UPDATE_TIME)
            {
                return;
            }
            else
            {
                SFX.Rupee.Play();
                _elapsedTime = 0f;
                _drawnRupees += _drawnRupees < currentRupees ? 1 : -1;
            }
            
            string stringValue = _drawnRupees.ToString();
            int startIndex = _MAX_CHARS - stringValue.Length;
            
            for (int i = 0; i < _MAX_CHARS; i++)
            {
                if (i < startIndex)
                {
                    _drawnSprites[i] = null;
                }
                else if (stringValue[i - startIndex] == '-')
                {
                    _drawnSprites[i] = _minusSprite;
                }
                else
                {
                    _drawnSprites[i] = _numberSprites[Int16.Parse(stringValue[i - startIndex].ToString())];
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_drawnRupees == 0)
                return;
            
            _head.Draw(spriteBatch, new Vector2(66, 15));

            Vector2 numberPosition = new Vector2(52, 24);

            for (int i = 0; i < _MAX_CHARS; i++)
            {
                if (_drawnSprites[i] != null)
                {
                    _drawnSprites[i].Draw(spriteBatch, numberPosition);
                }

                numberPosition.X += 7;
            }
        }
    }
}