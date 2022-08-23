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
            private Effect _effect = Shaders.Spotlight;
            private const float _FULL_SIZE = 1f;
            
            public SpotlightStage(Character target, bool closing)
            {
                _target = target;
                _closing = closing;
            }

            public override void Enter()
            {
                _effect.Parameters["target"].SetValue(
                    (Static.Scene.DrawOffset + Static.Player.Hitbox.Rectangle.Center) / Static.NativeSize
                );
                _effect.Parameters["size"].SetValue(
                    _closing ? _FULL_SIZE : 0f
                );
                Static.Renderer.ApplyPostEffect(_effect);
            }

            public override void Update(float elapsedTime)
            {
                if (elapsedTime < Time)
                {
                    Vector2 target = (Static.Scene.DrawOffset + Static.Player.Hitbox.Rectangle.Center) / Static.NativeSize;
                    float size = _FULL_SIZE * (elapsedTime / Time);
                    
                    if (_closing)
                        size = _FULL_SIZE - size;

                    _effect.Parameters["target"].SetValue(target);
                    _effect.Parameters["size"].SetValue(size);
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
        }
    }
}