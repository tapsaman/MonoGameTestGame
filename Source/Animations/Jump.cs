using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TapsasEngine.Utilities;

namespace ZA6.Animations
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

        public class To : Animation
        {
            public To(MapObject target, Vector2 distance)
            {
                Stages = new AnimationStage[]
                {
                    new JumpToStage(target, distance)
                };
            }
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

                if (_elapsedTime < Time)
                {
                    _velocity.Y += _gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    _target.SpriteOffset += _velocity;
                    
                }
                else
                {
                    IsDone = true;
                    _target.SpriteOffset = _defaultPosition;
                }
            }
            public override void Draw(SpriteBatch spriteBatch) {}
        }
        
        private class JumpToStage : AnimationStage
        {
            public float Time = 0.5f;
            public float JumpHeight = 0.5f;
            private Vector2 _distance;
            private Vector2 _startPosition;
            private Vector2 _startOffset;
            private MapObject _target;
            private float _gravity = 0;
            private Vector2 _velocity;
            private float _elapsedTime = 0;
            
            public JumpToStage(MapObject target, Vector2 distance)
            {
                _target = target;
                _distance = distance;
            }
            public override void Enter()
            {
                _startPosition = _target.Position;
                _startOffset = _target.SpriteOffset;
                _gravity = (float)(8 * (JumpHeight / Math.Pow(Time, 2)));
                _velocity = new Vector2(0, -(float)(_gravity * 0.5 * Time));
                SFX.Error.Play();

                if (_target is Character character)
                    character.Facing = _distance.ToDirection();
            }
            public override void Update(GameTime gameTime)
            {
                _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (_elapsedTime < Time)
                {
                    _velocity.Y += _gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    _target.SpriteOffset += _velocity;
                    _target.Position = _startPosition + _distance * (_elapsedTime / Time);
                }
                else
                {
                    _target.SpriteOffset = _startOffset;
                    _target.Position = _startPosition + _distance;
                    IsDone = true;
                }
            }
            public override void Draw(SpriteBatch spriteBatch) {}
        }
    }
}