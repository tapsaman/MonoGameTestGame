using Microsoft.Xna.Framework;
using TapsasEngine;
using TapsasEngine.Sprites;
using ZA6.Models;

namespace ZA6
{
    public class Sign : MapObject
    {
        public string Text
        {
            set { _readEvent = new TextEvent(new Dialog(value), this); }
        }
        private TextEvent _readEvent;

        public Sign()
        {
            Hittable = false;
            Sprite = new Sprite(Img.ObjectTexture, new Rectangle(16 * 4, 0, 16, 16));
            Hitbox.Load(16, 16);
            Interactable = true;
            Trigger += Read;
        }

        private void Read(Character _)
        {
            Static.EventSystem.Load(_readEvent);
        }
    }
}