using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZA6.Managers;

namespace ZA6.Animations
{
    public class Spotlight : Animation
    {
        public Spotlight(Character target, bool closing)
        {
            Stages = new AnimationStage[]
            {
                new SpotlightStage(target, closing)
            };
        }

        private class SpotlightStage : AnimationStage
        {
            public float Time = 1f;
            private Character _target;
            private bool _closing;
            private float _elapsedTime;
            private Effect _effect = Shaders.Spotlight;
            private const float _FULL_SIZE = 1f;
            
            public SpotlightStage(Character target, bool closing)
            {
                _target = target;
                _closing = closing;
            }

            public override void Enter()
            {
                _elapsedTime = 0;
                _effect.Parameters["target"].SetValue(
                    (Static.Scene.DrawOffset + Static.Player.Hitbox.Rectangle.Center) / Static.NativeSize
                );
                _effect.Parameters["size"].SetValue(
                    _closing ? _FULL_SIZE : 0f
                );
                Static.Renderer.ApplyPostEffect(_effect);
            }
            public override void Update(GameTime gameTime)
            {
                _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (_elapsedTime <= Time)
                {
                    float size = _FULL_SIZE * (_elapsedTime / Time);
                    
                    _effect.Parameters["target"].SetValue(
                        (Static.Scene.DrawOffset + Static.Player.Hitbox.Rectangle.Center) / Static.NativeSize
                    );
                    _effect.Parameters["size"].SetValue(
                        _closing ? _FULL_SIZE - size : size
                    );
                }
                else
                {
                    if (!_closing)
                    {
                        Static.Renderer.ApplyPostEffect(null);
                    }
                    else
                    {
                        _effect.Parameters["size"].SetValue(0f);
                    }

                    IsDone = true;
                }
            }

            public override void Draw(SpriteBatch spriteBatch) {}
        }
    }
}