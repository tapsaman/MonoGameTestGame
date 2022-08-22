using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TapsasEngine.Enums;
using TapsasEngine.Utilities;
using ZA6.Managers;

namespace ZA6.Models
{
    public class GuardStateDefault : CharacterState
    {
        private Guard _guard;
        private float _walkTime;
        private bool _turning;
        private float _elapsedTurnTime;
        private float _elapsedWalkTime;
        private const float _TURN_TIME = 0.1f;

        public GuardStateDefault(Guard guard) : base(guard)
        {
            _guard = guard;
        }

        public override void Enter(StateArgs _)
        {
            _guard.Sprite.SetAnimation("Walk" + _guard.Facing);
            _guard.Velocity = _guard.Facing.ToVector() * _guard.WalkSpeed;
            
            _walkTime = 2 + (float)Utility.RandomDouble() * 5;
            _turning = false;
            _elapsedTurnTime = 0;
            _elapsedWalkTime = 0;
        }

        public override void Update(GameTime gameTime)
        {
            _elapsedWalkTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            if (_guard.DetectingPlayer())
            {
                stateMachine.TransitionTo("NoticedPlayer");
            }
            else if (_turning)
            {
                _elapsedTurnTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (_elapsedTurnTime > _TURN_TIME)
                {
                    _turning = false;
                    _guard.Facing = _guard.Facing.Next();
                    _elapsedTurnTime = 0;
                    _guard.Sprite.SetAnimation("Walk" + _guard.Facing);
                }
            }
            else if (_guard.CollisionX == CollisionType.Full || _guard.CollisionY == CollisionType.Full)
            {
                // Hitting obstacle, turn around
                _turning = true;
                _guard.Facing = _guard.Facing.Next();
                _guard.Velocity = Vector2.Zero;
                _guard.Sprite.SetAnimation("Walk" + _guard.Facing);
            }
            else
            {
                _guard.Velocity = _guard.Facing.ToVector() * _guard.WalkSpeed;
                _guard.Sprite.SetAnimation("Walk" + _guard.Facing);

                if (_elapsedWalkTime > _walkTime)
                {
                    stateMachine.TransitionTo("LookAround");
                }
            }
        }

        public override void Exit() {}
    } 
}