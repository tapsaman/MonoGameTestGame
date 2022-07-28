using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTestGame.Managers;

namespace MonoGameTestGame.Models
{
    public class BatStateDefault : CharacterState
    {
        private const float _FLY_SPEED = 70f;

        public BatStateDefault(Bat bat) : base(bat) {}

        public override void Enter()
        {
            Character.Sprite.SetAnimation("Default");
            Character.Velocity = new Vector2(0, -0.1f);
        }

        public override void Update(GameTime gameTime)
        {
            /*float angle = MathHelper.Pi;
            direction.X = (int)((direction.X) * Math.Cos(angle) - direction.Y * Math.Sin(angle));
            direction.Y = (int)((direction.X) * Math.Sin(angle) + direction.Y * Math.Cos(angle));

            float angle = MathHelper.PiOver2;
            Vector2 dir = new Vector2(direction.X, direction.Y);
            Vector2.Transform(dir, Matrix.CreateRotationX(angle));
            direction = new Point((int)dir.X, (int)dir.Y);

            Character.Velocity = Character.Velocity.Transform();*/
            
            
            /*var direction = Character.Velocity;
            var angle = MathHelper.Pi;
            direction.X = (int)((direction.X) * Math.Cos(angle) - direction.Y * Math.Sin(angle));
            direction.Y = (int)((direction.X) * Math.Sin(angle) + direction.Y * Math.Cos(angle));*/

            /*var direction = Character.Velocity;
            float angle = MathHelper.PiOver2;
            Vector2 dir = new Vector2(direction.X, direction.Y);
            var transformed = Vector2.Transform(dir, Matrix.CreateRotationX(angle));
            direction = new Vector2((int) dir.X, (int) dir.Y);*/
            
            
            var vel = (StaticData.Scene.Player.Position - Character.Position);
            vel.Normalize();
            vel *= _FLY_SPEED;

            Character.Velocity = RotateVectorAround(vel, Character.Position, -0.25);
            
            //Character.Position -= Character.Velocity * 0.001f;
        }

        public static Vector2 RotateVectorAround(Vector2 vector, Vector2 pivot, double angle)
        {
            //Get the X and Y difference
            float xDiff = vector.X - pivot.X;
            float yDiff = vector.Y - pivot.Y;

            //Rotate the vector
            float x = (float)((Math.Cos(angle) * xDiff) - (Math.Sin(angle) * yDiff) + pivot.X);
            float y = (float)((Math.Sin(angle) * xDiff) + (Math.Cos(angle) * yDiff) + pivot.Y);

            return new Vector2(x, y);
        }

        public override void Exit() {}
    } 
}