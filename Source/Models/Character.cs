using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTestGame.Managers;
using MonoGameTestGame.Sprites;

namespace MonoGameTestGame
{
    public abstract class Character : MapObject
    {
        public Vector2 Velocity;
        public float WalkSpeed;
        public int Health { get; protected set; }
        public bool IsInvincible;
        public Direction Direction = Direction.Down;
        public Direction CollidingX = Direction.None;
        public Direction CollidingY = Direction.None;
        public Direction MapBorder = Direction.None;
        public bool Moving = false;
        public bool Walking = false;
        public bool WalkingStill = true;
        protected StateMachine StateMachine;

        public Character()
        {
            Sprite = new Sprite();
        }

        public override void Update(GameTime gameTime)
        {
            if (StateMachine == null)
            {
                if (WalkingStill || Velocity != Vector2.Zero) {
                    Sprite.SetAnimation("Walk" + Direction);
                }
                else {
                    Sprite.SetAnimation("Idle" + Direction);
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
            /*if (Velocity.Y < 0)
                Direction = Direction.Up;
            if (Velocity.X > 0)
                Direction = Direction.Right;
            if (Velocity.Y > 0)
                Direction = Direction.Down;
            if (Velocity.X < 0)
                Direction = Direction.Left;*/
            //FaceToVelocity();

            Velocity *= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Colliding)
            {
                foreach (var mapEntity in StaticData.Scene.TouchTriggers)
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
                
                MapBorder = Direction.None;
                CollidingX = DetermineHorizontalCollision(StaticData.Scene.TileMap, StaticData.Scene.CollidingEntities);
                CollidingY = DetermineVerticalCollision(StaticData.Scene.TileMap, StaticData.Scene.CollidingEntities);

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
