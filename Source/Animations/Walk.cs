using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTestGame.Managers;

namespace MonoGameTestGame.Animations
{
    public class Walk : Animation
    {
        public Walk(Character target, Vector2 distance, float time = 0f)
        {
            if (time == 0f)
            {
                Stages = new AnimationStage[]
                {
                    new WalkStage(target, distance, target.WalkSpeed)
                };
            }
            else
            {
                Stages = new AnimationStage[]
                {
                    new TimedWalkStage(target, distance, time)
                };
            }
        }

        private class TimedWalkStage : AnimationStage
        {
            public float Time = 1f;
            private Vector2 _distance;
            private Vector2 _endPosition;
            private Character _target;
            private float _elapsedTime = 0f;
            
            public TimedWalkStage(Character target, Vector2 distance, float time)
            {
                _target = target;
                _distance = distance;
                Time = time;
            }
            public override void Enter()
            {
                _endPosition = _target.Position + _distance;
            }
            public override void Update(GameTime gameTime)
            {
                _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (_elapsedTime > Time)
                {
                    IsDone = true;
                    _target.Position = _endPosition;
                    _target.Velocity = Vector2.Zero;
                }
                else
                {
                    _target.Velocity = _distance / Time;
                }
            }
            public override void Draw(SpriteBatch spriteBatch) {}
        }

        private class WalkStage : AnimationStage
        {
            public float Time = 1f;
            private Vector2 _distance;
            private float _speed;
            private Vector2 _endPosition;
            private Vector2 _startPosition;
            private Vector2 _velocity;
            private Character _target;
            
            public WalkStage(Character target, Vector2 distance, float speed)
            {
                _target = target;
                _distance = distance;
                _speed = speed;
            }
            public override void Enter()
            {
                _startPosition = _target.Position;
                _endPosition = _target.Position + _distance;
                _velocity = _distance;
                _velocity.Normalize();
                _velocity *= _speed;
                _target.Direction = _velocity.ToDirection();
            }
            public override void Update(GameTime gameTime)
            {
                var travel = _target.Position - _startPosition;

                if (Math.Abs(travel.X) >= Math.Abs(_distance.X) && Math.Abs(travel.Y) >= Math.Abs(_distance.Y))
                {
                    IsDone = true;
                    _target.Position = _endPosition;
                    _target.Velocity = Vector2.Zero;
                }
                else
                {
                    _target.Velocity = _velocity;
                }
            }
            public override void Draw(SpriteBatch spriteBatch) {}
        }
    }
}