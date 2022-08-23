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
                new JumpToStage(target, Vector2.Zero)
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
        
        private class JumpToStage : AnimationStage
        {
            public float Time = 0.5f;
            public float JumpHeight = 0.5f;
            private Vector2 _distance;
            private Vector2 _startPosition;
            private Vector2 _startOffset;
            private MapObject _target;
            private float _gravity;
            private Vector2 _velocity;
            
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
                SFX.Jump.Play();

                if (_target is Character character)
                    character.Facing = _distance.ToDirection();
            }
            public override void Update(float elapsedTime)
            {
                if (elapsedTime < Time)
                {
                    _velocity.Y += _gravity * Animation.Delta;
                    _target.SpriteOffset += _velocity;
                    _target.Position = _startPosition + _distance * (elapsedTime / Time);
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