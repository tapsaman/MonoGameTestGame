using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZA6.Managers;

namespace ZA6.Models
{
    public class BatStateDefault : CharacterState
    {
        private Bat _bat;
        private const float _FLY_SPEED = 140f;
        private Vector2 _velocity;
        private double _angle;
        private float _elapsedTimeFromHit = 0;
        private const float _HIT_WAIT_TIME = 1f;

        public BatStateDefault(Bat bat)
            : base(bat)
        {
            _bat = bat;
        }

        public override void Enter(StateArgs _)
        {
            Character.Sprite.SetAnimation("Default");
            SFX.Keese.Play();
            _angle = (Math.PI / 2);
        }

        public override void Update(GameTime gameTime)
        {
            if (!_bat.DidHitPlayer)
            {
                _angle += gameTime.ElapsedGameTime.TotalSeconds / 6;
                _velocity = (Character.Position - Static.Scene.Player.Position);
                _velocity.Normalize();
                _velocity = RotateVector(_velocity, _angle) * _FLY_SPEED;
            }
            else
            {
                _elapsedTimeFromHit += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (_elapsedTimeFromHit > _HIT_WAIT_TIME)
                {
                    _elapsedTimeFromHit = 0f;
                    _bat.DidHitPlayer = false;
                }
            }

            _bat.Velocity = _velocity;
            
            //Character.Position -= Character.Velocity * 0.001f;
        }

        public Vector2 RotateVector(Vector2 v, double radianAngle)
        {
            float sine = (float)Math.Sin(radianAngle);
            float cosi = (float)Math.Cos(radianAngle);

            return new Vector2
            (
                v.X * cosi - v.Y * sine,
                v.X * sine + v.Y * cosi
            );
        }

        public override void Exit() {}
    } 
}