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

        public override void Update(GameTime gameTime) {}

        public override void Exit() {}
    } 
}