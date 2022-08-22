using Microsoft.Xna.Framework;
using ZA6.Sprites;

namespace ZA6
{
    public class Bush : MapObject
    {
        public bool OverHole;

        public Bush()
        {
            Hitbox.Load(16, 16);
            Interactable = false;
            Hittable = true;
            Colliding = true;
            Sprite.SetTexture(Img.ObjectTexture, new Rectangle(0, 0, 16, 16));
        }

        public override void TakeHit(Character _)
        {
            Static.Scene.Add(new Animations.BushSlash(Position + new Vector2(8)));
            Static.Scene.Remove(this);

            if (!OverHole)
            {
                var stumpSprite = new Sprite();
                stumpSprite.SetTexture(Img.ObjectTexture, new Rectangle(16, 0, 16, 16));
                stumpSprite.Position = Position;

                Static.Scene.Add(stumpSprite);
            }
            else
            {
                var hole = new Hole(Position);

                Static.Scene.Add(hole);
            }
        }
    }
}