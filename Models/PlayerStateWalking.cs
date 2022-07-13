using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTestGame.Managers;

namespace MonoGameTestGame.Models
{
    public class PlayerStateWalking : PlayerState
    {
        public PlayerStateWalking(Player player) : base(player) {}

        public override void Enter()
        {
            Player.MapEntity.Sprite.SetAnimation("Walk" + Player.MapEntity.Direction);
        }

        public override void Update(GameTime gameTime)
        {
            Player.DetermineInputVelocity(gameTime);
            Player.DetermineHitInput();
            

            if (Player.Hit)
            {
                stateMachine.TransitionTo("SwordHit");
            }
            else if (Player.MapEntity.Velocity.X == 0 && Player.MapEntity.Velocity.Y == 0)
            {
                stateMachine.TransitionTo("Idle");
            }
            else
            {
                Player.MapEntity.Sprite.SetAnimation("Walk" + Player.MapEntity.Direction);
            }
        }

        public override void Exit() {}
    } 
}