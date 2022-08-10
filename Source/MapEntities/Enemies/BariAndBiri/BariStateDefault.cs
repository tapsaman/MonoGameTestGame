using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZA6.Managers;

namespace ZA6.Models
{
    public class BariStateDefault : CharacterState
    {
        private Enemy _enemy;
        private float _SPEED = 20f;
        private float _elapsedTime;
        private float _time;

        public BariStateDefault(Enemy enemy) : base(enemy)
        {
            _enemy = enemy;
        }

        public override void Enter(StateArgs _)
        {
            _enemy.Sprite.SetAnimation("Default");
            _time = 0.5f + (float)Utility.RandomDouble() * 3f;
            _enemy.IsInvincible = false;
        }

        public override void Update(GameTime gameTime)
        {
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