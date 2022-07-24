using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTestGame.Managers;

namespace MonoGameTestGame.Models
{
    public class GuardStateDefault : CharacterState
    {
        private Hitbox _detectionHitbox;

        public GuardStateDefault(Guard Guard) : base(Guard) {}

        public override void Enter()
        {
            Character.Sprite.SetAnimation("Walk" + Character.Direction);
            Character.Velocity = Utility.DirectionToVector(Character.Direction) * Character.WalkSpeed;
            _detectionHitbox = new Hitbox();
            _detectionHitbox.Load(60, 100);
            _detectionHitbox.Position = Character.Position - new Vector2(23, 80);
        }

        public override void Update(GameTime gameTime)
        {
            if (Character.CollidingX == Character.Direction || Character.CollidingY == Character.Direction)
            {
                // Hitting obstacle, turn around
                Character.Direction = Utility.ToOpposite(Character.Direction);
            }

            Character.Sprite.SetAnimation("Walk" + Character.Direction);
            Character.Velocity = Utility.DirectionToVector(Character.Direction) * Character.WalkSpeed;
            _detectionHitbox.Position = Character.Position - new Vector2(23, 80);
        }

        public override void Exit() {}
    } 
}