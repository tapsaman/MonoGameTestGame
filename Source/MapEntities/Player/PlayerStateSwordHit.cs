using Microsoft.Xna.Framework;

namespace MonoGameTestGame.Models
{
    public class PlayerStateSwordHit : PlayerState
    {
        public const float HitTime = 0.4f;
        private float _elapsedHitTime = 0f;
        public PlayerStateSwordHit(Player player) : base(player) {}

        public override void Enter()
        {
            Player.Sprite.SetAnimation("SwordHit" + Player.Direction);
            Player.SwordHitbox.StartHit(Player.Hitbox.Rectangle, Player.Direction);
        }

        public override void Update(GameTime gameTime)
        {
            _elapsedHitTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_elapsedHitTime > HitTime)
            {
                _elapsedHitTime = 0f;
                Player.Hitting = false;
                Player.SwordHitbox.EndHit();
                stateMachine.TransitionTo("Idle");
            }
        }

        public override void Exit() {}
    } 
}