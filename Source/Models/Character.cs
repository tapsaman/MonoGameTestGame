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
        public virtual Direction Direction { get; set; } = Direction.Down;
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

            if (Colliding)
            {
                foreach (var mapEntity in StaticData.Scene.CollidingEntities)
                {
                    if (mapEntity != this)
                    {
                        DetermineCollision(mapEntity);
                    }
                }
                DetermineCollision(StaticData.Scene.TileMap);
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
        private void DetermineCollision(TileMap tileMap)
        {
            if (Velocity.X < 0) {
                int currentTileLeft = tileMap.ConvertX(Hitbox.Rectangle.Left);
                int newTileLeft = tileMap.ConvertX(Hitbox.Rectangle.Left + Velocity.X);

                if (newTileLeft < currentTileLeft) {
                    if (tileMap.CheckHorizontalCollision(newTileLeft, tileMap.ConvertY(Hitbox.Rectangle.Top), tileMap.ConvertY(Hitbox.Rectangle.Bottom)))
                        Velocity.X = 0;
                }
            }
            else if (Velocity.X > 0) {
                int currentTileRight = tileMap.ConvertX(Hitbox.Rectangle.Right);
                int newTileRight = tileMap.ConvertX(Hitbox.Rectangle.Right + Velocity.X);
                if (newTileRight > currentTileRight) {
                    if (tileMap.CheckHorizontalCollision(newTileRight, tileMap.ConvertY(Hitbox.Rectangle.Top), tileMap.ConvertY(Hitbox.Rectangle.Bottom)))
                        Velocity.X = 0;
                }
            }
            if (Velocity.Y < 0) {
                int currentTileTop = tileMap.ConvertY(Hitbox.Rectangle.Top);
                int newTileTop = tileMap.ConvertY(Hitbox.Rectangle.Top + Velocity.Y);

                if (newTileTop < currentTileTop) {
                    if (tileMap.CheckVerticalCollision(newTileTop, tileMap.ConvertX(Hitbox.Rectangle.Left), tileMap.ConvertX(Hitbox.Rectangle.Right)))
                        Velocity.Y = 0;
                }
            }
            else if (Velocity.Y > 0) {
                int currentTileBottom = tileMap.ConvertY(Hitbox.Rectangle.Bottom);
                int newTileBottom = tileMap.ConvertY(Hitbox.Rectangle.Bottom + Velocity.Y);

                if (newTileBottom > currentTileBottom) {
                    if (tileMap.CheckVerticalCollision(newTileBottom, tileMap.ConvertX(Hitbox.Rectangle.Left), tileMap.ConvertX(Hitbox.Rectangle.Right)))
                        Velocity.Y = 0;
                }
            }
        }
    }
}
