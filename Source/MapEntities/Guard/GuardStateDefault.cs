using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTestGame.Managers;

namespace MonoGameTestGame.Models
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

        public override void Enter()
        {
            _guard.Sprite.SetAnimation("Walk" + _guard.Direction);
            _guard.Velocity = _guard.Direction.ToVector() * _guard.WalkSpeed;
            
            _walkTime = 4; //2 + (float)Utility.RandomDouble() * 5;
            _turning = false;
            _elapsedTurnTime = 0;
            _elapsedWalkTime = 0;
        }

        public override void Update(GameTime gameTime)
        {
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
                    _guard.Direction = _guard.Direction.Next();
                    _elapsedTurnTime = 0;
                    _guard.Sprite.SetAnimation("Walk" + _guard.Direction);
                }
            }
            else if (_guard.CollidingX == _guard.Direction || _guard.CollidingY == _guard.Direction)
            {
                // Hitting obstacle, turn around
                _turning = true;
                _guard.Direction = _guard.Direction.Next();
                _guard.Velocity = Vector2.Zero;
                _guard.Sprite.SetAnimation("Walk" + _guard.Direction);
            }
            else
            {
                _guard.Velocity = _guard.Direction.ToVector() * _guard.WalkSpeed;
                _guard.Sprite.SetAnimation("Walk" + _guard.Direction);

                _elapsedWalkTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (_elapsedWalkTime > _walkTime)
                {
                    stateMachine.TransitionTo("LookAround");
                }
            }
        }

        public override void Exit() {}
    } 
}