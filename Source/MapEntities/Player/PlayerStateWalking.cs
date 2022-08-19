using Microsoft.Xna.Framework;

namespace ZA6.Models
{
    public class PlayerStateWalking : PlayerState
    {
        public PlayerStateWalking(Player player) : base(player) {}

        public override void Enter(StateArgs _)
        {
            Player.Sprite.SetAnimation("Walk" + Player.Facing);
        }

        public override void Update(GameTime gameTime)
        {
            Player.DetermineInputVelocity();
            Player.DetermineHitInput();
            Player.FaceToVelocity();

            if (Player.Hitting)
            {
                stateMachine.TransitionTo("SwordHit");
            }
            else if (Player.Velocity == Vector2.Zero)
            {
                stateMachine.TransitionTo("Idle");
            }
            else
            {
                Player.Sprite.SetAnimation("Walk" + Player.Facing);
            }
        }

        public override void Exit() {}
    } 
}