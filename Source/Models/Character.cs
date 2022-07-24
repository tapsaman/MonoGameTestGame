using System;
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

        public void Move(GameTime gameTime)
        {
            if (Velocity.Y < 0)
                Direction = Direction.Up;
            if (Velocity.X > 0)
                Direction = Direction.Right;
            if (Velocity.Y > 0)
                Direction = Direction.Down;
            if (Velocity.X < 0)
                Direction = Direction.Left;

            Velocity *= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Colliding)
            {
                foreach (var mapEntity in StaticData.Scene.CollidingEntities)
                {
                    if (mapEntity != this)
                    {
                        DetermineCollision(mapEntity);
                    }
                }

                MapBorder = Direction.None;
                CollidingX = DetermineHorizontalMapCollision(StaticData.Scene.TileMap);
                CollidingY = DetermineVerticalMapCollision(StaticData.Scene.TileMap);

                if (CollidingX != Direction.None)
                    Velocity.X = 0;
                if (CollidingY != Direction.None)
                    Velocity.Y = 0;
            }

            Position += Velocity;
        }

        private bool IsTouchingLeft(MapEntity mapEntity)
        {
            return Hitbox.Rectangle.Right + Velocity.X > mapEntity.Hitbox.Rectangle.Left &&
                Hitbox.Rectangle.Left < mapEntity.Hitbox.Rectangle.Left &&
                Hitbox.Rectangle.Bottom > mapEntity.Hitbox.Rectangle.Top &&
                Hitbox.Rectangle.Top < mapEntity.Hitbox.Rectangle.Bottom;
        }
        private bool IsTouchingRight(MapEntity mapEntity)
        {
        return Hitbox.Rectangle.Left + Velocity.X < mapEntity.Hitbox.Rectangle.Right &&
            Hitbox.Rectangle.Right > mapEntity.Hitbox.Rectangle.Right &&
            Hitbox.Rectangle.Bottom > mapEntity.Hitbox.Rectangle.Top &&
            Hitbox.Rectangle.Top < mapEntity.Hitbox.Rectangle.Bottom;
        }
        private bool IsTouchingTop(MapEntity mapEntity) 
        {
        return Hitbox.Rectangle.Bottom + Velocity.Y > mapEntity.Hitbox.Rectangle.Top &&
            Hitbox.Rectangle.Top < mapEntity.Hitbox.Rectangle.Top &&
            Hitbox.Rectangle.Right > mapEntity.Hitbox.Rectangle.Left &&
            Hitbox.Rectangle.Left < mapEntity.Hitbox.Rectangle.Right;
        }
        private bool IsTouchingBottom(MapEntity mapEntity) 
        {
        return Hitbox.Rectangle.Top + Velocity.Y < mapEntity.Hitbox.Rectangle.Bottom &&
            Hitbox.Rectangle.Bottom > mapEntity.Hitbox.Rectangle.Bottom &&
            Hitbox.Rectangle.Right > mapEntity.Hitbox.Rectangle.Left &&
            Hitbox.Rectangle.Left < mapEntity.Hitbox.Rectangle.Right;
        }
        private void DetermineCollision(MapEntity mapEntity)
        {
            if (Velocity.Y > 0 && IsTouchingTop(mapEntity))
                Velocity.Y = 0; 
            if (Velocity.X < 0 && IsTouchingRight(mapEntity))
                Velocity.X = 0;
            if (Velocity.Y < 0 && IsTouchingBottom(mapEntity))
                Velocity.Y = 0;
            if (Velocity.X > 0 && IsTouchingLeft(mapEntity))
                Velocity.X = 0;
        }
        private Direction DetermineHorizontalMapCollision(TileMap tileMap)
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
            }

            return Direction.None;
        }

        private Direction DetermineVerticalMapCollision(TileMap tileMap)
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
            }

            return Direction.None;
        }
    }
}
