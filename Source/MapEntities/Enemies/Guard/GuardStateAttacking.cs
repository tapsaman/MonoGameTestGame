using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TapsasEngine.Utilities;
using ZA6.Managers;

namespace ZA6.Models
{
    public class GuardStateAttacking : CharacterState
    {
        private Guard _guard;
        private float _elapsedTime;
        private Vector2 _runVelocity;
        private const float _RUN_SPEED = 60f;
        private const float _ATTACK_TIME = 2f;

        public GuardStateAttacking(Guard guard) : base(guard)
        {
            _guard = guard;
        }

        public override void Enter(StateArgs _)
        {
            SFX.Soldier.Play();
            _elapsedTime = 0;
            //_guard.FaceTowards(Static.Scene.Player.Position);
            //_guard.AnimatedSprite.SetAnimation("Run" + _guard.Facing);
            var vel = (Static.Scene.Player.Position - _guard.Position);
            vel.Normalize();
            _runVelocity = vel * _RUN_SPEED;
            _guard.Moving = true;
            _guard.Facing = _runVelocity.ToDirection();
            _guard.AnimatedSprite.SetAnimation("Run" + _guard.Facing);
        }
        public override void Update(GameTime gameTime)
        {
            _guard.Velocity = _runVelocity;
            _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_elapsedTime > _ATTACK_TIME)
            {
                if (_guard.DetectingPlayer())
                {
                    _elapsedTime = 0;
                    var vel = (Static.Scene.Player.Position - _guard.Position);
                    vel.Normalize();
                    _runVelocity = vel * _RUN_SPEED;
                    //_guard.FaceTowards(Static.Scene.Player.Position);
                    _guard.Facing = _runVelocity.ToDirection();
                    _guard.AnimatedSprite.SetAnimation("Run" + _guard.Facing);
                }
                else
                {
                    StateMachine.TransitionTo("LookAround");
                }
            }
        }

        public override void Exit() {}
    } 
}