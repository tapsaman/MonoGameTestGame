using Microsoft.Xna.Framework;
using MonoGameTestGame.Models;

namespace MonoGameTestGame
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
            Hitbox.Load(16, 16);
            Interactable = true;
            Hittable = false;
            Colliding = true;
            Sprite.SetTexture(StaticData.ObjectTexture, new Rectangle(16 * 4, 0, 16, 16));
            Trigger += Read;
        }

        private void Read()
        {
            StaticData.Scene.EventManager.Load(_readEvent);
        }
    }
}