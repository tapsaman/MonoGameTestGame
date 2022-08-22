using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TapsasEngine.Enums;
using TapsasEngine.Utilities;
using ZA6.Managers;
using ZA6.Sprites;

namespace ZA6
{
    public abstract class Character : MapObject
    {
        public Vector2 ElementalVelocity;
        public Vector2 Velocity;
        public float WalkSpeed = 10f;
        public int Health;
        public bool IsInvincible;
        public Direction Facing = Direction.Down;
        public Direction MapBorder = Direction.None;
        //public Direction CollidingX = Direction.None;
        //public Direction CollidingY = Direction.None;
        public CollisionType CollisionX;
        public CollisionType CollisionY;
        public bool NoClip = false;
        public bool Moving = false;
        public bool DrawingShadow = true;
        public bool Walking = false;
        public bool WalkingStill = false;
        public StateMachine StateMachine { get; protected set; }

        public Character()
        {
            Sprite = new Sprite();
        }

        public virtual void MakeFall() {}

        public override void Update(GameTime gameTime)
        {
            if (StateMachine == null)
            {
                FaceToVelocity();
                
                if (WalkingStill || Velocity != Vector2.Zero) {
                    Sprite.SafeSetAnimation("Walk" + Facing);
                }
                else {
                    Sprite.SafeSetAnimation("Idle" + Facing);
                }
            }
            else
            {
                StateMachine.Update(gameTime);
            }

            if (Moving) {
                Move(gameTime);
            }
            
            Sprite.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            if (DrawingShadow)
            {
                var shadowPos = new Vector2(Hitbox.Rectangle.Left - 5, Hitbox.Rectangle.Bottom - 6 - 20) + offset;
                shadowPos.Round();

                spriteBatch.Draw(Img.Shadow, shadowPos, new Rectangle(168, 308, 24, 28), Color.White);
            }
            
            base.Draw(spriteBatch, offset);
        }

        public void FaceTowards(Vector2 position)
        {
            Facing = Utility.GetDirectionBetween(Position, position);
        }

        public void FaceToVelocity()
        {
            Direction velDirection = Velocity.ToDirection();

            if (velDirection != Direction.None)
            {
                Facing = velDirection;
            }
        }

        public void Move(GameTime gameTime)
        {
            ElementalVelocity = Vector2.Zero;
            Velocity *= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Colliding)
            {
                if (Static.Game.StateMachine.CurrentStateKey == "Default")
                {
                    // First go through touch triggers to get elemental velocity
                    foreach (var mapEntity in Static.Scene.TouchTriggers)
                    {
                        if (RightIsTouching(mapEntity) 
                        || LeftIsTouching(mapEntity)
                        || TopIsTouching(mapEntity)
                        || BottomIsTouching(mapEntity)
                        )
                        {
                            mapEntity.InvokeTrigger(this);
                        }
                    }

                    Velocity += ElementalVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }

                MapBorder = Direction.None;
                CollisionX = DetermineHorizontalCollision(Static.Scene.TileMap, Static.Scene.CollidingEntities);
                CollisionY = DetermineVerticalCollision(Static.Scene.TileMap, Static.Scene.CollidingEntities);
                //Sys.Log("CollisionX " + CollisionX + " : CollisionY " + CollisionY);

                if (!NoClip)
                {
                    ApplyCollisions();
                }

                /*
                if (CollidingX != Direction.None)
                    Velocity.X = 0;
                if (CollidingY != Direction.None)
                    Velocity.Y = 0;
                */
            }

            Position += Velocity;
        }

        private void ApplyCollisions()
        {
            var ogVel = Velocity;
            // Diagonal collision velocity multiplier
            var dcvm = 0.5f;
            var dcvm2 = 1.5f;

            switch (CollisionX)
            {
                case CollisionType.None:
                    break;
                case CollisionType.Full:
                    Velocity.X = 0;
                    break;
                case CollisionType.NorthEast:
                    Velocity.X *= dcvm;
                    Velocity.Y = ogVel.X * dcvm2;
                    break;
                case CollisionType.SouthEast:
                    /*if (CollisionY == CollisionType.SouthEast)
                    {
                        Velocity = Vector2.Zero;
                    }*/
                    Velocity.X *= dcvm;
                    Velocity.Y = -ogVel.X * dcvm2;
                    break;
                case CollisionType.SouthWest:
                if (CollisionY == CollisionType.SouthWest)
                    /*{
                        Velocity = Vector2.Zero;
                    }*/
                    Velocity.X *= dcvm;
                    Velocity.Y = ogVel.X * dcvm2;
                    break;
                case CollisionType.NorthWest:
                    Velocity.X *= dcvm;
                    Velocity.Y = -ogVel.X * dcvm2;
                    break;
            }
            switch (CollisionY)
            {
                case CollisionType.None:
                    break;
                case CollisionType.Full:
                    Velocity.Y = 0;
                    break;
                case CollisionType.NorthEast:
                    Velocity.Y *= dcvm;
                    Velocity.X = ogVel.Y * dcvm2;
                    break;
                case CollisionType.SouthEast:
                    Velocity.Y *= dcvm;
                    Velocity.X = -ogVel.Y * dcvm2;
                    break;
                case CollisionType.SouthWest:
                    Velocity.Y *= dcvm;
                    Velocity.X = Velocity.Y * dcvm2;
                    break;
                case CollisionType.NorthWest:
                    Velocity.Y *= dcvm;
                    Velocity.X = -ogVel.Y * dcvm2;
                    break;
            }
        }

        private bool RightIsTouching(MapEntity mapEntity)
        {
            return Hitbox.Rectangle.Right + Velocity.X > mapEntity.Hitbox.Rectangle.Left &&
                Hitbox.Rectangle.Left < mapEntity.Hitbox.Rectangle.Left &&
                Hitbox.Rectangle.Bottom > mapEntity.Hitbox.Rectangle.Top &&
                Hitbox.Rectangle.Top < mapEntity.Hitbox.Rectangle.Bottom;
        }
        private bool LeftIsTouching(MapEntity mapEntity)
        {
        return Hitbox.Rectangle.Left + Velocity.X < mapEntity.Hitbox.Rectangle.Right &&
            Hitbox.Rectangle.Right > mapEntity.Hitbox.Rectangle.Right &&
            Hitbox.Rectangle.Bottom > mapEntity.Hitbox.Rectangle.Top &&
            Hitbox.Rectangle.Top < mapEntity.Hitbox.Rectangle.Bottom;
        }
        private bool BottomIsTouching(MapEntity mapEntity)
        {
        return Hitbox.Rectangle.Bottom + Velocity.Y > mapEntity.Hitbox.Rectangle.Top &&
            Hitbox.Rectangle.Top < mapEntity.Hitbox.Rectangle.Top &&
            Hitbox.Rectangle.Right > mapEntity.Hitbox.Rectangle.Left &&
            Hitbox.Rectangle.Left < mapEntity.Hitbox.Rectangle.Right;
        }
        private bool TopIsTouching(MapEntity mapEntity) 
        {
        return Hitbox.Rectangle.Top + Velocity.Y < mapEntity.Hitbox.Rectangle.Bottom &&
            Hitbox.Rectangle.Bottom > mapEntity.Hitbox.Rectangle.Bottom &&
            Hitbox.Rectangle.Right > mapEntity.Hitbox.Rectangle.Left &&
            Hitbox.Rectangle.Left < mapEntity.Hitbox.Rectangle.Right;
        }

        private CollisionType DetermineHorizontalCollision(TileMap tileMap, IEnumerable<MapEntity> collidingEntities)
        {
            if (Velocity.X < 0)
            {
                foreach (var mapEntity in collidingEntities)
                {
                    if (LeftIsTouching(mapEntity))
                    {
                        return CollisionType.Full;
                    }
                }

                return tileMap.GetLeftCollision(Hitbox.Rectangle, Velocity, ref MapBorder);            
            }
            else if (Velocity.X > 0)
            {
                foreach (var mapEntity in collidingEntities)
                {
                    if (RightIsTouching(mapEntity))
                    {
                        return CollisionType.Full;
                    }
                }

                return tileMap.GetRightCollision(Hitbox.Rectangle, Velocity, ref MapBorder);
            }

            return CollisionType.None;
        }

        private CollisionType DetermineVerticalCollision(TileMap tileMap, IEnumerable<MapEntity> collidingEntities)
        {
            if (Velocity.Y < 0)
            {
                foreach (var mapEntity in collidingEntities)
                {
                    if (TopIsTouching(mapEntity))
                    {
                        return CollisionType.Full;
                    }
                }

                return tileMap.GetTopCollision(Hitbox.Rectangle, Velocity, ref MapBorder);            
            }
            else if (Velocity.Y > 0)
            {
                foreach (var mapEntity in collidingEntities)
                {
                    if (BottomIsTouching(mapEntity))
                    {
                        return CollisionType.Full;
                    }
                }

                return tileMap.GetBottomCollision(Hitbox.Rectangle, Velocity, ref MapBorder);
            }

            return CollisionType.None;
        }
    }
}
