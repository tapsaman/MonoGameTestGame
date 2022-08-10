using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZA6.Sprites;

namespace ZA6
{
    public abstract class MapObject : MapEntity //, IComparable<MapObject>
    {
        public Sprite Sprite;
        public bool Hittable { get; protected set; } = false;
        public bool Colliding = true;
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

        public MapObject()
        {
            Sprite = new Sprite();
        }

        public virtual void Update(GameTime gameTime)
        {
            Sprite.Update(gameTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            Sprite.Draw(spriteBatch, offset);
        }
    }
}
