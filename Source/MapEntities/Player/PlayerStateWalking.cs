using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZA6.Managers;

namespace ZA6.Models
{
    public class PlayerStateWalking : PlayerState
    {
        public PlayerStateWalking(Player player) : base(player) {}

        public override void Enter(StateArgs _)
        {
            Player.Sprite.SetAnimation("Walk" + Player.Direction);
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
                Player.Sprite.SetAnimation("Walk" + Player.Direction);
            }
        }

        public override void Exit() {}
    } 
}