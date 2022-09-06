using Microsoft.Xna.Framework;
using TapsasEngine;
using TapsasEngine.Sprites;
using ZA6.Managers;
using ZA6.Models;

namespace ZA6.Items
{
    public class Heart : Collectible
    {
        
        public override string ItemID => "heart";

        public override string CollectText => "You got 1+ HEART !";

        public Heart()
        {
            Sprite = new Sprite(Img.ObjectTexture, new Rectangle(32, 16, 8, 8));
            Hitbox.Load(8, 8);
        }

        public override void Collect()
        {
            base.Collect();
            Static.Player.ReplenishHealth(2);
        }
    }
}
