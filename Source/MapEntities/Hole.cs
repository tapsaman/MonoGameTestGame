using Microsoft.Xna.Framework;
using ZA6.Sprites;

namespace ZA6
{
    public class Hole : Sprite
    {
        private TouchEventTrigger _holeEventTrigger;

        public Hole(Vector2 position)
        {
            Position = position;

            SetTexture(Img.ObjectTexture, new Rectangle(48, 0, 16, 16));

            _holeEventTrigger = new TouchEventTrigger(Position, 16, 16);
            _holeEventTrigger.Trigger += PullToHole;
            var innerHoleEventTrigger = new TouchEventTrigger(Position + new Vector2(6, 0), 4, 16);
            innerHoleEventTrigger.Trigger += PullToHole;

            Static.Scene.Add(_holeEventTrigger);
            Static.Scene.Add(innerHoleEventTrigger);
        }

        private void PullToHole(Character character)
        {
            character.ElementalVelocity = _holeEventTrigger.Hitbox.Rectangle.Center - character.Hitbox.Rectangle.Center;
            character.ElementalVelocity.Normalize();
            character.ElementalVelocity *= 33f;

            if (_holeEventTrigger.Hitbox.Rectangle.Contains(character.Hitbox.Rectangle))
            {
                character.MakeFall();
            }
        }
    }
}