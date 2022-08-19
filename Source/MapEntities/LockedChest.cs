using Microsoft.Xna.Framework;
using TapsasEngine;
using TapsasEngine.Models;
using ZA6.Models;
using ZA6.Sprites;

namespace ZA6
{
    public class LockedChest : MapObject
    {
        public LockedChest()
        {
            Hitbox.Load(16, 16);
            Hittable = false;
            Colliding = true;
            Interactable = true;
            Trigger += TryToOpen;
            Sprite.SetTexture(Img.ObjectTexture, new Rectangle(0, 16, 16, 16));
        }

        private void TryToOpen(Character _)
        {
            SFX.WalkGrass.Play();
            Static.EventSystem.Load(new Event[]
            {
                new WaitEvent(0.6f),
                new TextEvent(new Dialog("It's locked!"), this)
            });
        }
    }
}