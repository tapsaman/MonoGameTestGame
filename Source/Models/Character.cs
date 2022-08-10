using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZA6.Managers;
using ZA6.Sprites;

namespace ZA6
{
    public abstract class Character : MapObject
    {
        public Vector2 ElementalVelocity;
        public Vector2 Velocity;
        public float WalkSpeed = 10f;
        public int Health { get; protected set; }
        public bool IsInvincible;
        public Direction Direction = Direction.Down;
        public Direction CollidingX = Direction.None;
        public Direction CollidingY = Direction.None;
        public Direction MapBorder = Direction.None;
        public bool Moving = false;
        public bool DrawingShadow = true;
        public bool Walking = false;
        public bool WalkingStill = true;
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
                    Sprite.SafeSetAnimation("Walk" + Direction);
                }
                else {
                    Sprite.SafeSetAnimation("Idle" + Direction);
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
            Direction = Utility.GetDirectionBetween(Position, position);
        }

        public void FaceToVelocity()
        {
            Direction velDirection = Velocity.ToDirection();

            if (velDirection != Direction.None)
            {
                Direction = velDirection;
            }
        }

        public void Move(GameTime gameTime)
        {
            ElementalVelocity = Vector2.Zero;
            Velocity *= (float)gameTime.ElapsedGameTime.TotalSeconds;

            // First go through touch triggers to get elemental velocity
            if (Colliding)
            {
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

                MapBorder = Direction.None;
                CollidingX = DetermineHorizontalCollision(Static.Scene.TileMap, Static.Scene.CollidingEntities);
                CollidingY = DetermineVerticalCollision(Static.Scene.TileMap, Static.Scene.CollidingEntities);

                if (CollidingX != Direction.None)
                    Velocity.X = 0;
                if (CollidingY != Direction.None)
                    Velocity.Y = 0;
            }

            Position += Velocity;
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
        private Direction DetermineHorizontalCollision(TileMap tileMap, IEnumerable<MapEntity> collidingEntities)
        {
            if (Velocity.X < 0)
            {
                int currentTileLeft = tileMap.ConvertX(Hitbox.Rectangle.Left);
                int newTileLeft = tileMap.ConvertX(Hitbox.Rectangle.Left + Velocity.X);

                if (newTileLeft < currentTileLeft)
                {
                    if (tileMap.CheckHorizontalCollision(
                        newTileLeft,
                        tileMap.ConvertY(Hitbox.Rectangle.Top),
                        tileMap.ConvertY(Hitbox.Rectangle.Bottom),
                        ref MapBorder
                    ))
                    {
                        return Direction.Left;
                    }
                }

                foreach (var mapEntity in collidingEntities)
                {
                    if (LeftIsTouching(mapEntity))
                    {
                        return Direction.Left;
                    }
                }
            }
            else if (Velocity.X > 0)
            {
                int currentTileRight = tileMap.ConvertX(Hitbox.Rectangle.Right);
                int newTileRight = tileMap.ConvertX(Hitbox.Rectangle.Right + Velocity.X);

                if (newTileRight > currentTileRight)
                {
                    if (tileMap.CheckHorizontalCollision(
                        newTileRight,
                        tileMap.ConvertY(Hitbox.Rectangle.Top),
                        tileMap.ConvertY(Hitbox.Rectangle.Bottom),
                        ref MapBorder
                    ))
                    {
                        return Direction.Right;
                    }
                }

                foreach (var mapEntity in collidingEntities)
                {
                    if (RightIsTouching(mapEntity))
                    {
                        return Direction.Right;
                    }
                }
            }

            return Direction.None;
        }

        private Direction DetermineVerticalCollision(TileMap tileMap, IEnumerable<MapEntity> collidingEntities)
        {
            if (Velocity.Y < 0)
            {
                int currentTileTop = tileMap.ConvertY(Hitbox.Rectangle.Top);
                int newTileTop = tileMap.ConvertY(Hitbox.Rectangle.Top + Velocity.Y);

                if (newTileTop < currentTileTop)
                {
                    if (tileMap.CheckVerticalCollision(
                        newTileTop,
                        tileMap.ConvertX(Hitbox.Rectangle.Left),
                        tileMap.ConvertX(Hitbox.Rectangle.Right),
                        ref MapBorder
                    ))
                    {
                        return Direction.Up;
                    }
                }

                foreach (var mapEntity in collidingEntities)
                {
                    if (TopIsTouching(mapEntity))
                    {
                        return Direction.Up;
                    }
                }
            }
            else if (Velocity.Y > 0)
            {
                int currentTileBottom = tileMap.ConvertY(Hitbox.Rectangle.Bottom);
                int newTileBottom = tileMap.ConvertY(Hitbox.Rectangle.Bottom + Velocity.Y);

                if (newTileBottom > currentTileBottom)
                {
                    if (tileMap.CheckVerticalCollision(
                        newTileBottom,
                        tileMap.ConvertX(Hitbox.Rectangle.Left),
                        tileMap.ConvertX(Hitbox.Rectangle.Right),
                        ref MapBorder
                    ))
                    {
                        return Direction.Down;
                    }
                }

                foreach (var mapEntity in collidingEntities)
                {
                    if (BottomIsTouching(mapEntity))
                    {
                        return Direction.Down;
                    }
                }
            }

            return Direction.None;
        }
    }
}
