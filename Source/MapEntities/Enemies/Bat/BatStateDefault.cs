using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZA6.Managers;

namespace ZA6.Models
{
    public class BatStateDefault : CharacterState
    {
        private const float _FLY_SPEED = 70f;
        private Vector2 _velocity;

        public BatStateDefault(Bat bat) : base(bat) {}

        public override void Enter(StateArgs _)
        {
            Character.Sprite.SetAnimation("Default");
            _velocity = Utility.RandomDirection().ToVector() * 40f;
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
            
            
            /*var vel = (Static.Scene.Player.Position - Character.Position);
            vel.Normalize();
            vel *= _FLY_SPEED;*/

            var angle = GetAngleBetween(Character.Position, Static.Scene.Player.Position);

            //var transformed = Vector2.Transform(_velocity, Matrix.CreateRotationY((float)angle));
            
            //direction = new Point((int) dir.X, (int) dir.Y);


            //_velocity = Rotate(_velocity, -0.01f);
            _velocity = RotateVectorAround(_velocity, Character.Position, angle);
            Character.Velocity = _velocity;
            
            //Character.Position -= Character.Velocity * 0.001f;
        }



        private static double GetAngleBetween(Vector2 a, Vector2 b)
        {
            return Math.Atan2(b.Y - a.Y, b.X - a.X);
        }

        public static Vector2 Rotate(Vector2 v, float delta)
        {
            return new Vector2(
                (float)(v.X * Math.Cos(delta) - v.Y * Math.Sin(delta)),
                (float)(v.X * Math.Sin(delta) + v.Y * Math.Cos(delta))
            );
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