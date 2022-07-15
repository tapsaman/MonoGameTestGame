using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameTestGame.Controls;
using MonoGameTestGame.Managers;
using MonoGameTestGame.Models;
using MonoGameTestGame.Sprites;

namespace MonoGameTestGame
{
    public class MapEntity
    {
        public Vector2 Velocity;
        public Sprite Sprite;
        public Direction Direction = Direction.Down;
        public Hitbox Hitbox;
        public bool Interactable { get; protected set; } = false;
        public bool Hittable { get; protected set; } = false;
        public bool BlockedByTerrain { get; protected set; } = false;
        
        public event Action Trigger;
        public bool HasTrigger()
        {
            return Trigger != null;
        }
        public void InvokeTrigger()
        {
            Trigger.Invoke();
        }
        private Vector2 _position;
        public Vector2 Position
        {
            get { return _position; }
            set
            {
                _position = value;
                Sprite.Position = _position + _spriteOffset;

                if (Hitbox != null)
                Hitbox.Position = _position;
            }
        }
        private Vector2 _spriteOffset = Vector2.Zero;
        public Vector2 SpriteOffset
        {
            get { return _spriteOffset; }
            set
            {
                _spriteOffset = value;
                Sprite.Position = _position + _spriteOffset;
            }
        }
        public bool Moving = false;

        public MapEntity(Vector2 position)
        {
            Sprite = new Sprite();
            Position = position;
        }
        public MapEntity(Sprite sprite)
        {
            Sprite = sprite;
        }
        public MapEntity(Sprite sprite, Hitbox hitbox)
        {
            Sprite = sprite;
            Hitbox = hitbox;
        }

        public void Update(GameTime gameTime, MapEntity[] mapEntities, TileMap tileMap)
        {
            if (Moving) {
                Move(gameTime, mapEntities, tileMap);
            }
            Sprite.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Hitbox != null) {
                Hitbox.Draw(spriteBatch);
            }
            Sprite.Draw(spriteBatch);
        }

        public void Move(GameTime gameTime, MapEntity[] mapEntities, TileMap tileMap)
        {
            if (Velocity.Y < 0)
                Direction = Direction.Up;
            if (Velocity.X > 0)
                Direction = Direction.Right;
            if (Velocity.Y > 0)
                Direction = Direction.Down;
            if (Velocity.X < 0)
                Direction = Direction.Left;

            if (Hitbox != null)
            {
                foreach (var mapEntity in mapEntities)
                {
                    if (mapEntity != this && mapEntity.Hitbox != null)
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
                }
                if (tileMap != null)
                {
                    DetermineCollision(tileMap);
                }
            }

            Position += Velocity;
            Velocity = Vector2.Zero;
        }

        public Rectangle GetTouchArea()
        {
            if (Direction == Direction.Up)
                return new Rectangle(Hitbox.Rectangle.Left, Hitbox.Rectangle.Top - 5, Hitbox.Rectangle.Width, 5);
            if (Direction == Direction.Right)
                return new Rectangle(Hitbox.Rectangle.Right, Hitbox.Rectangle.Top, 5, Hitbox.Rectangle.Height);
            if (Direction == Direction.Down)
                return new Rectangle(Hitbox.Rectangle.Left, Hitbox.Rectangle.Bottom, Hitbox.Rectangle.Width, 5);
            
            return new Rectangle(Hitbox.Rectangle.Left - 5, Hitbox.Rectangle.Top, 5, Hitbox.Rectangle.Height);
        }

        protected bool IsTouchingLeft(MapEntity mapEntity)
        {
            return Hitbox.Rectangle.Right + Velocity.X > mapEntity.Hitbox.Rectangle.Left &&
                Hitbox.Rectangle.Left < mapEntity.Hitbox.Rectangle.Left &&
                Hitbox.Rectangle.Bottom > mapEntity.Hitbox.Rectangle.Top &&
                Hitbox.Rectangle.Top < mapEntity.Hitbox.Rectangle.Bottom;
        }
        protected bool IsTouchingRight(MapEntity mapEntity)
        {
        return Hitbox.Rectangle.Left + Velocity.X < mapEntity.Hitbox.Rectangle.Right &&
            Hitbox.Rectangle.Right > mapEntity.Hitbox.Rectangle.Right &&
            Hitbox.Rectangle.Bottom > mapEntity.Hitbox.Rectangle.Top &&
            Hitbox.Rectangle.Top < mapEntity.Hitbox.Rectangle.Bottom;
        }
        protected bool IsTouchingTop(MapEntity mapEntity) 
        {
        return Hitbox.Rectangle.Bottom + Velocity.Y > mapEntity.Hitbox.Rectangle.Top &&
            Hitbox.Rectangle.Top < mapEntity.Hitbox.Rectangle.Top &&
            Hitbox.Rectangle.Right > mapEntity.Hitbox.Rectangle.Left &&
            Hitbox.Rectangle.Left < mapEntity.Hitbox.Rectangle.Right;
        }
        protected bool IsTouchingBottom(MapEntity mapEntity) 
        {
        return Hitbox.Rectangle.Top + Velocity.Y < mapEntity.Hitbox.Rectangle.Bottom &&
            Hitbox.Rectangle.Bottom > mapEntity.Hitbox.Rectangle.Bottom &&
            Hitbox.Rectangle.Right > mapEntity.Hitbox.Rectangle.Left &&
            Hitbox.Rectangle.Left < mapEntity.Hitbox.Rectangle.Right;
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
        protected bool IsTouchingLeftWall(TileMap tileMap)
        {
            float newX = Position.X + Velocity.X;
            int currentTileX = tileMap.ConvertX(Position.X);
            int newTileX = tileMap.ConvertX(newX);
            if (newX < currentTileX) {
                //return tileMap.CheckCollision(new Rectangle((int)newX, Hitbox.Rectangle.Top, 1, Hitbox.Rectangle.Height));
            }
            return false;
            Vector2 movingTo = new Vector2(Hitbox.Rectangle.Left, Hitbox.Rectangle.Top) + Velocity;
            return tileMap.CheckCollision(movingTo);
        }
        protected bool IsTouchingTopWall(TileMap tileMap)
        {
            Vector2 movingTo = new Vector2(Hitbox.Rectangle.Left, Hitbox.Rectangle.Top) + Velocity;
            return tileMap.CheckCollision(movingTo);
        }
        protected bool IsTouchingRightWall(TileMap tileMap)
        {
            Vector2 movingTo = new Vector2(Hitbox.Rectangle.Right, Hitbox.Rectangle.Bottom) + Velocity;
            return tileMap.CheckCollision(movingTo);
        }
        protected bool IsTouchingBottomWall(TileMap tileMap)
        {
            Vector2 movingTo = new Vector2(Hitbox.Rectangle.Right, Hitbox.Rectangle.Bottom) + Velocity;
            return tileMap.CheckCollision(movingTo);
        }

        protected bool TopIsColliding(MapEntity mapEntity) 
        {
            return Hitbox.Rectangle.Top + Velocity.Y < mapEntity.Hitbox.Rectangle.Bottom &&
                Hitbox.Rectangle.Bottom > mapEntity.Hitbox.Rectangle.Bottom &&
                Hitbox.Rectangle.Right > mapEntity.Hitbox.Rectangle.Left &&
                Hitbox.Rectangle.Left < mapEntity.Hitbox.Rectangle.Right;
        }

        protected bool TopIsColliding(TileMap tileMap) 
        {
            Vector2 movingTo = new Vector2(Hitbox.Rectangle.Left, Hitbox.Rectangle.Top) + Velocity;
            return tileMap.CheckCollision(movingTo);
        }
    }
}
