using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TapsasEngine;
using TapsasEngine.Sprites;

namespace ZA6
{
    public abstract class MapObject : MapEntity, IUpdate
    {
        public virtual Sprite Sprite { get; set; }
        public virtual MapLevel Level { get => MapLevel.Character; }
        
        public bool Hittable { get; protected set; } = false;
        public override Vector2 Position
        {
            get { return _position; }
            set
            {
                _position = value;
                Hitbox.Position = _position;
            }
        }
        public Vector2 SpriteOffset;
        private Vector2 _position;
        private Vector2 _spriteOffset = Vector2.Zero;

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            Sprite.Draw(spriteBatch, Position + SpriteOffset + offset);
        }

        public virtual void TakeHit(Character hitter) {}

        public virtual void Update(GameTime gameTime) {}
    }
}
