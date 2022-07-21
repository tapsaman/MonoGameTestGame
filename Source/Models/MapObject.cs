using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTestGame.Managers;
using MonoGameTestGame.Sprites;

namespace MonoGameTestGame
{
    public abstract class MapObject : MapEntity //, IComparable<MapObject>
    {
        public Sprite Sprite;
        public bool Hittable { get; protected set; } = false;
        public bool Colliding { get; protected set; } = true;
        public override Vector2 Position
        {
            get { return _position; }
            set
            {
                _position = value;
                Sprite.Position = _position + _spriteOffset;
                Hitbox.Position = _position;
            }
        }
        public Vector2 SpriteOffset
        {
            get { return _spriteOffset; }
            set
            {
                _spriteOffset = value;
                Sprite.Position = _position + _spriteOffset;
            }
        }
        private Vector2 _position;
        private Vector2 _spriteOffset = Vector2.Zero;
       
        // Comparison method to sort entities by Y position
        /*public int CompareTo(MapObject mapEntity)
        {
            if (Position.Y < mapEntity.Position.Y)
                return 1;
            if (Position.Y > mapEntity.Position.Y)
                return 0;
            if (Index < mapEntity.Index)
                return 0;
            
            return 1;
        }*/

        public MapObject()
        {
            Sprite = new Sprite();
        }

        public virtual void Update(GameTime gameTime)
        {
            Sprite.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            Sprite.Draw(spriteBatch, offset);
        }
    }
}
