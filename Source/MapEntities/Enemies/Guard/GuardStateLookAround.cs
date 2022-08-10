using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZA6.Managers;

namespace ZA6.Models
{
    public class GuardStateLookAround : CharacterState
    {
        private Guard _guard;
        private float _elapsedTime;
        private const float _DIRECTION_LOOK_TIME = 0.8f;
        private Direction _lookDirection;
        private int _lookIter;

        public GuardStateLookAround(Guard guard) : base(guard)
        {
            _guard = guard;
        }

        public override void Enter(StateArgs _)
        {
            Character.Velocity = Vector2.Zero;
            _elapsedTime = 0;
            _lookIter = 0;
            _guard.Moving = false;
            _lookDirection = _guard.Direction.NextCounterclockwise();
            Character.Sprite.SetAnimation("Idle" + _guard.Direction + "Look" + _lookDirection);
        }
        public override void Update(GameTime gameTime)
        {
            if (_guard.DetectingPlayer(_lookDirection))
            {
                stateMachine.TransitionTo("NoticedPlayer");
            }
            else
            {
                _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (_elapsedTime > _DIRECTION_LOOK_TIME)
                {
                    _elapsedTime = 0;
                    _lookIter++;

                    if (_lookIter == 1)
                    {
                        _lookDirection = _lookDirection.Next();
                        Character.Sprite.SetAnimation("Idle" + _lookDirection);
                    }
                    else if (_lookIter == 2)
                    {
                        _lookDirection = _lookDirection.Next();
                        Character.Sprite.SetAnimation("Idle" + _guard.Direction + "Look" + _lookDirection);
                    }
                    else if (_lookIter == 3)
                    {
                        _lookDirection = _lookDirection.NextCounterclockwise();
                        Character.Sprite.SetAnimation("Idle" + _lookDirection);
                    }
                    else
                    {
                        Character.Direction = Utility.RandomDirection();
                        stateMachine.TransitionTo("Default");
                    }
                }
            }
        }

        public override void Exit()
        {
            _guard.Moving = true;
        }
    } 
}