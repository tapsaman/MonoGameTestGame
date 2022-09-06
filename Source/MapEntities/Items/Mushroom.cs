using Microsoft.Xna.Framework;
using TapsasEngine;
using TapsasEngine.Sprites;
using ZA6.Managers;
using ZA6.Models;

namespace ZA6.Items
{
    public class Mushroom : Collectible
    {
        
        public override string ItemID => "mushroom";

        public override string CollectText => "You foun mushroom!";

        public Mushroom()
        {
            Sprite = new Sprite(Img.ObjectTexture, new Rectangle(64, 16, 16, 16));
            Hitbox.Load(16, 16);
        }

        public override void Collect()
        {
            base.Collect();
            Static.SessionData.Save("eaten mushroom", true);
        }
    }
}
