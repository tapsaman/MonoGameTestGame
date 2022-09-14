using Microsoft.Xna.Framework;

namespace ZA6.Models
{
    public class PlayerStateTakenDamage : PlayerState
    {
        private Vector2 _flyVelocity;
        private float _elapsedTime;
        private const float _FLY_TIME = 0.5f;

        public PlayerStateTakenDamage(Player player) : base(player) {}

        public override void Enter(StateArgs _)
        {
            SFX.LinkHurt.Play();
            _elapsedTime = 0;
            var vel = (Player.Position - Player.HitPosition);
            vel.Normalize();
            _flyVelocity = vel * 150f;
            Player.SwordHitbox.Enabled = false;
            Player.AnimatedSprite.SetAnimation("Damaged" + Player.Facing);
        }

        public override void Update(GameTime gameTime)
        {
            /*if (Player.CollidingX != Facing.None || Player.CollidingY != Facing.None)
            {
                _flyVelocity = Vector2.Zero;
            }*/

            Player.Velocity = _flyVelocity;
            _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_elapsedTime > _FLY_TIME)
            {
                _elapsedTime = 0f;
                StateMachine.TransitionTo("Idle");
            }
        }

        public override void Exit() {}
    } 
}