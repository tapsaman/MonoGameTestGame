using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTestGame.Managers;

namespace MonoGameTestGame.Animations
{
    public class Jump : Animation
    {
        public Jump(MapObject target)
        {
            Stages = new AnimationStage[]
            {
                new JumpStage(target)
            };
        }

        private class JumpStage : AnimationStage
        {
            public float Time = 0.5f;
            public float JumpHeight = 0.5f;
            private Vector2 _defaultPosition;
            private MapObject _target;
            private float _gravity = 0;
            private Vector2 _velocity;
            private float _elapsedTime = 0;
            
            public JumpStage(MapObject target)
            {
                _target = target;
            }
            public override void Enter()
            {
                _defaultPosition = _target.SpriteOffset;
                _gravity = (float)(8 * (JumpHeight / Math.Pow(Time, 2)));
                _velocity = new Vector2(0, -(float)(_gravity * 0.5 * Time));
                SFX.Error.Play();
            }
            public override void Update(GameTime gameTime)
            {
                _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (_elapsedTime > Time)
                {
                    IsDone = true;
                    _target.SpriteOffset = _defaultPosition;
                }
                else
                {
                    _velocity.Y += _gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    _target.SpriteOffset += _velocity;
                }
            }
            public override void Draw(SpriteBatch spriteBatch) {}
        }
    }
}