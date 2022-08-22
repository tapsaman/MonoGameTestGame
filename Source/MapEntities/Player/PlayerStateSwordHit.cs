using Microsoft.Xna.Framework;

namespace ZA6.Models
{
    public class PlayerStateSwordHit : PlayerState
    {
        public const float HitTime = 0.4f;
        private float _elapsedHitTime = 0f;
        public PlayerStateSwordHit(Player player)
            : base(player)
        {
            CanReEnter = true;
        }

        public override void Enter(StateArgs _)
        {
            SFX.Sword1.Play();
            Player.AnimatedSprite.SetAnimation(null);
            Player.AnimatedSprite.SetAnimation("SwordHit" + Player.Facing);
            Player.SwordHitbox.StartHit(Player.Hitbox.Rectangle, Player.Facing);
            _elapsedHitTime = 0f;
            Player.Hitting = false;
        }

        public override void Update(GameTime gameTime)
        {
            _elapsedHitTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_elapsedHitTime > HitTime / 2)
            {
                Player.DetermineHitInput();

                if (Player.Hitting)
                {
                    stateMachine.TransitionTo("SwordHit");
                    return;
                }
            }

            if (_elapsedHitTime > HitTime)
            {
                Player.SwordHitbox.EndHit();
                stateMachine.TransitionTo("Idle");
            }
        }

        public override void Exit() {}
    } 
}