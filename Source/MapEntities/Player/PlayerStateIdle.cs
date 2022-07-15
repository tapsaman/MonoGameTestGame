using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTestGame.Managers;

namespace MonoGameTestGame.Models
{
    public class PlayerStateIdle : PlayerState
    {
        public PlayerStateIdle(Player player) : base(player) {}

        public override void Enter()
        {
            Player.MapEntity.Sprite.SetAnimation("Idle" + Player.MapEntity.Direction);
        }

        public override void Update(GameTime gameTime)
        {
            Player.DetermineInputVelocity(gameTime);
            Player.DetermineHitInput();

            if (Player.Hit)
            {
                stateMachine.TransitionTo("SwordHit");
                Player.Hit = false;
            }
            else if (Player.MapEntity.Velocity.X != 0 || Player.MapEntity.Velocity.Y != 0)
            {
                stateMachine.TransitionTo("Walking");
            }
        }

        public override void Exit() {}
    } 
}