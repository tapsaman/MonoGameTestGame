using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TapsasEngine.Utilities;
using ZA6.Managers;

namespace ZA6.Models
{
    public class BariStateDefault : CharacterState
    {
        private Bari _bari;
        private float _SPEED = 20f;
        private float _elapsedTime;
        private float _time;

        public BariStateDefault(Bari bari) : base(bari)
        {
            _bari = bari;
        }

        public override void Enter(StateArgs _)
        {
            _bari.Sprite.SetAnimation("Default");
            _time = 0.5f + (float)Utility.RandomDouble() * 3f;
            _bari.IsInvincible = false;
        }

        public override void Update(GameTime gameTime)
        {
            if (_bari.PushVelocity != Vector2.Zero)
            {
                Character.Velocity = _bari.PushVelocity * 150f;
                _bari.PushVelocity -= _bari.PushVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds * 4;
            
                if (_bari.PushVelocity.ToAbsoluteFloat() < 0.6)
                {
                    _bari.PushVelocity = Vector2.Zero;
                }

                return;
            }

            _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_elapsedTime < _time)
            {
                var vel = (Static.Scene.Player.Position - Character.Position);
                vel.Normalize();
                Character.Velocity = vel * _SPEED;
            }
            else
            {
                _elapsedTime = 0;
                Character.StateMachine.TransitionTo("Attacking");
            }
        }

        public override void Exit() {}
    } 
}