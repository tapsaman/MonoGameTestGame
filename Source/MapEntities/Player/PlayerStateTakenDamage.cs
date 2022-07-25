using Microsoft.Xna.Framework;

namespace MonoGameTestGame.Models
{
    public class PlayerStateTakenDamage : PlayerState
    {
        private Vector2 _flyVelocity;
        private float _elapsedTime;
        private const float _FLY_TIME = 0.5f;

        public PlayerStateTakenDamage(Player player) : base(player) {}

        public override void Enter()
        {
            SFX.EnemyHit.Play();
            _elapsedTime = 0;
            var vel = (Player.Position - Player.HitPosition);
            vel.Normalize();
            _flyVelocity = vel * 150f;
            Player.SwordHitbox.Enabled = false;
            Player.Sprite.SetAnimation("Idle" + Player.Direction);
        }

        public override void Update(GameTime gameTime)
        {
            Player.Velocity = _flyVelocity;
            _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_elapsedTime > _FLY_TIME)
            {
                _elapsedTime = 0f;
                stateMachine.TransitionTo("Idle");
            }
        }

        public override void Exit() {}
    } 
}