using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZA6.Managers;
using TapsasEngine.Sprites;

namespace ZA6.Animations
{
    public class FadeSprite : Animation
    {
        public FadeSprite(Sprite target)
        {
            Stages = new AnimationStage[]
            {
                new FadeSpriteStage(target)
            };
        }

        private class FadeSpriteStage : AnimationStage
        {
            public float Time = 1.5f;
            private Sprite _target;
            private float _elapsedTime = 0;
            
            public FadeSpriteStage(Sprite target)
            {
                _target = target;
            }
            public override void Enter()
            {
                _elapsedTime = 0;
            }
            public override void Update(GameTime gameTime)
            {
                _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (_elapsedTime < Time)
                {
                    float changePercentage = _elapsedTime / Time;
                    float value = 1f - changePercentage;
                    _target.Color = new Color(value, value, value, value);
                }
                else
                {
                    //_target.Color.A = 0;
                    IsDone = true;
                }
            }
            public override void Draw(SpriteBatch spriteBatch) {}
        }
    }
}