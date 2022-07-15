using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTestGame.Sprites;

namespace MonoGameTestGame
{
    public abstract class MapEntity
    {
        public Vector2 Velocity;
        public Sprite Sprite;
        public Direction Direction = Direction.Down;
        public Hitbox Hitbox;
        public bool Interactable { get; protected set; } = false;
        public bool Hittable { get; protected set; } = false;
        public bool Colliding { get; protected set; } = true;
        
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
            Hitbox = new Hitbox();
            Position = position;
        }
        public MapEntity(Sprite sprite, Hitbox hitbox)
        {
            Sprite = sprite;
            Hitbox = hitbox;
        }

        public virtual void Update(GameTime gameTime)
        {
            if (Moving) {
                Move(gameTime);
            }
            Sprite.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Hitbox.Draw(spriteBatch);
            Sprite.Draw(spriteBatch);
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
            Velocity = Vector2.Zero;
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
