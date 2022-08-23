using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TapsasEngine.Utilities;

namespace ZA6.Animations
{
    public class Walk : Animation
    {
        public Walk(Character target, Vector2 distance, float? speed = null)
        {
            Stages = new AnimationStage[]
            {
                new WalkStage(
                    target,
                    distance,
                    speed == null ? target.WalkSpeed : (float)speed
                )
            };
        }

        public class Timed : Animation
        {
            public Timed(Character target, Vector2 distance, float time)
            {
                Stages = new AnimationStage[]
                {
                    new TimedWalkStage(target, distance, time)
                };
            }
        }

        public class To : Animation
        {
            public To(Character target, Vector2 endPostion, float? speed = null)
            {
                Stages = new AnimationStage[]
                {
                    new WalkToStage(
                        target,
                        endPostion,
                        speed == null ? target.WalkSpeed : (float)speed
                    )
                };
            }
        }

        private class WalkStage : AnimationStage
        {
            protected Vector2 _distance;
            protected float _speed;
            protected Vector2 _endPosition;
            protected Vector2 _startPosition;
            protected Vector2 _velocity;
            protected Character _target;
            
            public WalkStage(Character target, Vector2 distance, float speed)
            {
                _target = target;
                _distance = distance;
                _speed = speed;
            }
            public override void Enter()
            {
                _distance.Floor();
                _startPosition = _target.Position;
                _endPosition = _target.Position + _distance;
                _velocity = _distance;
                _velocity.Normalize();
                _velocity *= _speed;
                _target.Facing = _velocity.ToDirection();
            }
            public override void Update(float _)
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
        }

        private class WalkToStage : WalkStage
        {
            public WalkToStage(Character target, Vector2 endPosition, float speed)
                : base(target, endPosition, speed)
            {
                _endPosition = endPosition;
            }
            public override void Enter()
            {
                _startPosition = _target.Position;
                _distance = _endPosition - _startPosition;
                _velocity = _distance;
                _velocity.Normalize();
                _velocity *= _speed;
                _target.Facing = _velocity.ToDirection();
            }
        }

        private class TimedWalkStage : AnimationStage
        {
            public float Time = 1f;
            private Vector2 _distance;
            private Vector2 _endPosition;
            private Character _target;
            
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
            public override void Update(float elapsedTime)
            {
                if (elapsedTime > Time)
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
        }
    }
}