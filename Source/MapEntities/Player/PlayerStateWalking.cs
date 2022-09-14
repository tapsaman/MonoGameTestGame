using Microsoft.Xna.Framework;

namespace ZA6.Models
{
    public class PlayerStateWalking : PlayerState
    {
        public PlayerStateWalking(Player player) : base(player) {}

        public override void Enter(StateArgs _)
        {
            Player.AnimatedSprite.SetAnimation("Walk" + Player.Facing);
        }

        public override void Update(GameTime gameTime)
        {
            Player.DetermineInputVelocity();
            Player.DetermineHitInput();
            Player.FaceToVelocity();

            if (Player.Hitting)
            {
                StateMachine.TransitionTo("SwordHit");
            }
            else if (Player.Velocity == Vector2.Zero)
            {
                StateMachine.TransitionTo("Idle");
            }
            else
            {
                Player.AnimatedSprite.SetAnimation("Walk" + Player.Facing);
            }
        }

        public override void Exit() {}
    } 
}