using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTestGame.Managers;

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
        }

        public override void Update(GameTime gameTime)
        {
            _elapsedHitTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_elapsedHitTime > HitTime)
            {
                _elapsedHitTime = 0f;
                Player.Hit = false;
                stateMachine.TransitionTo("Idle");
            }
        }

        public override void Exit() {}
    } 
}