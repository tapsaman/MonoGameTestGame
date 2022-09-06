using Microsoft.Xna.Framework;
using TapsasEngine.Sprites;

namespace ZA6
{
    public class Hole : MapObject
    {
        public override MapLevel Level { get => MapLevel.Ground; }
        
        public Hole(Vector2 position)
        {
            Position = position;
            
            Sprite = new Sprite(Img.ObjectTexture, new Rectangle(48, 0, 16, 16));
            Hitbox.Load(16, 16);
            TriggeredOnTouch = true;
            Trigger += PullToHole;
            
           /* var innerHoleEventTrigger = new TouchEventTrigger(Position + new Vector2(6, 0), 4, 16);
            innerHoleEventTrigger.Trigger += PullToHole;

            Static.Scene.Add(innerHoleEventTrigger);*/
        }

        private void PullToHole(Character character)
        {
            character.ElementalVelocity = Hitbox.Rectangle.Center - character.Hitbox.Rectangle.Center;
            character.ElementalVelocity.Normalize();
            character.ElementalVelocity *= 33f;

            if (Hitbox.Rectangle.Contains(character.Hitbox.Rectangle))
            {
                character.MakeFall();
            }
        }
    }
}