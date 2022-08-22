using Microsoft.Xna.Framework;
using TapsasEngine.Sprites;

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
            Sprite = new Sprite(Img.ObjectTexture, new Rectangle(0, 0, 16, 16));
        }

        public override void TakeHit(Character _)
        {
            Static.Scene.Add(new Animations.BushSlash(Position + new Vector2(8)));
            Static.Scene.Remove(this);

            if (!OverHole)
            {
                Static.Scene.Add(new BushStump() { Position = Position });
            }
            else
            {
                Static.Scene.Add(new Hole(Position));
            }
        }
    }
}