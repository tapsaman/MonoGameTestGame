using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZA6.Managers;

namespace ZA6.Models
{
    public class BubbleStateDefault : CharacterState
    {
        private const float _SPEED = 100f;
        private Vector2 _velocity;

        public BubbleStateDefault(Bubble bubble) : base(bubble) {}

        public override void Enter(StateArgs _)
        {
            Character.Sprite.SetAnimation("Default");
            _velocity = Utility.RandomDiagonal() * _SPEED;
        }

        public override void Update(GameTime gameTime)
        {
            if (Character.CollisionX != CollisionType.None)
            {
                _velocity.X = -_velocity.X;
            }
            if (Character.CollisionY != CollisionType.None)
            {
                _velocity.Y = -_velocity.Y;
            }

            Character.Velocity = _velocity;
        }

        public override void Exit() {}
    } 
}