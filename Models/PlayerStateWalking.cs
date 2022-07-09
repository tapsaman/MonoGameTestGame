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
            Console.WriteLine("Enter Walk" + Player.Direction);
            Player.Sprite.SetAnimation("Walk" + Player.Direction);
        }

        public override void Update(GameTime gameTime)
        {
            Player.DetermineInputVelocity();
            Player.DetermineHitInput();
            

            if (Player.Hit)
            {
                stateMachine.TransitionTo("SwordHit");
            }
            else if (Player.Velocity.X == 0 && Player.Velocity.Y == 0)
            {
                stateMachine.TransitionTo("Idle");
            }
            else
            {
                Player.Move(gameTime);
                Player.Sprite.SetAnimation("Walk" + Player.Direction);
            }
        }

        public override void Exit() {}
    } 
}