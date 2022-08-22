using Microsoft.Xna.Framework;
using TapsasEngine;
using TapsasEngine.Enums;
using TapsasEngine.Utilities;

namespace ZA6.Models
{
    public class GuardStateNoticedPlayer : CharacterState
    {
        private Guard _guard;
        private float _elapsedTime;
        private const float _LOOK_TIME = 0.5f;

        public GuardStateNoticedPlayer(Guard guard) : base(guard)
        {
            _guard = guard;
        }

        public override void Enter(StateArgs _)
        {
            _elapsedTime = 0;
            _guard.Moving = false;
            _guard.Velocity = Vector2.Zero;
            var direction = Utility.GetDirectionBetween(_guard.Position, Static.Scene.Player.Position);
            Sys.Log("Noticed player in direction " + direction);
            
            if (direction == _guard.Facing)
                _guard.Sprite.SetAnimation("Idle" + _guard.Facing);
            else if (direction == _guard.Facing.Opposite())
            {
                _guard.Sprite.SetAnimation("Idle" + direction);
                _guard.Facing = direction;
            }
            else
                _guard.Sprite.SetAnimation("Idle" + _guard.Facing + "Look" + direction);

        }
        public override void Update(GameTime gameTime)
        {
            _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_elapsedTime > _LOOK_TIME)
            {
                stateMachine.TransitionTo("Attacking");
            }
        }

        public override void Exit() {}
    } 
}