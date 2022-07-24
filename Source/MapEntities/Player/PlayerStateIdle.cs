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
            Player.Sprite.SetAnimation("Idle" + Player.Direction);
        }

        public override void Update(GameTime gameTime)
        {
            Player.DetermineInputVelocity();
            Player.DetermineHitInput();

            if (Player.Hitting)
            {
                stateMachine.TransitionTo("SwordHit");
            }
            else if (Player.Velocity.X != 0 || Player.Velocity.Y != 0)
            {
                stateMachine.TransitionTo("Walking");
            }
        }

        public override void Exit() {}
    } 
}