using Microsoft.Xna.Framework;

namespace MonoGameTestGame.Models
{
    public class PlayerStateStopped : PlayerState
    {
        public PlayerStateStopped(Player player) : base(player) {}

        public override void Enter()
        {
            Player.Sprite.SetAnimation("Idle" + Player.Direction);
        }

        public override void Update(GameTime gameTime)
        {
            Player.FaceToVelocity();

            /*if (Player.Hitting)
            {
                stateMachine.TransitionTo("SwordHit");
            }*/
            if (Player.Velocity == Vector2.Zero)
            {
                Player.Sprite.SetAnimation("Idle" + Player.Direction);
            }
            else
            {
                Player.Sprite.SetAnimation("Walk" + Player.Direction);
            }
        }

        public override void Exit() {}
    } 
}