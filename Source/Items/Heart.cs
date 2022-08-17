using Microsoft.Xna.Framework;
using TapsasEngine.Models;

namespace ZA6.Items
{
    public class Heart : Item
    {
        public Heart()
        {
            PickUpText = "You got +1 HEART !";
            Sprite.SetTexture(Img.ObjectTexture, new Rectangle(32, 16, 8, 8));
            Hitbox.Load(8, 8);
        }

        public override void OnPickUp()
        {
            SFX.Heart.Play();
            Static.Player.ReplenishHealth(2);
        }
    }
}