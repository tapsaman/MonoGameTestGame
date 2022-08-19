using Microsoft.Xna.Framework;

namespace ZA6.Models
{
    public class PlayerStateStopped : PlayerState
    {
        public PlayerStateStopped(Player player) : base(player) {}

        public override void Enter(StateArgs _)
        {
            //Player.Sprite.SetAnimation("Idle" + Player.Direction);
        }

        public override void Update(GameTime gameTime)
        {
            if (!Player.Moving)
                return;
            
            Player.FaceToVelocity();

            if (Player.Velocity == Vector2.Zero)
            {
                Player.Sprite.SetAnimation("Idle" + Player.Facing);
            }
            else
            {
                Player.Sprite.SetAnimation("Walk" + Player.Facing);
            }
        }

        public override void Exit() {}
    } 
}