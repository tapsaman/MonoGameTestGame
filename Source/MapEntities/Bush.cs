using Microsoft.Xna.Framework;

namespace MonoGameTestGame
{
    public class Bush : MapObject
    {
        public Bush()
        {
            Hitbox.Load(14, 14);
            Interactable = true;
            Hittable = true;
            Colliding = true;
            Sprite.SetTexture(StaticData.ObjectTexture, new Rectangle(0, 0, 16, 16));
            Trigger += Destroy;
        }

        private void Destroy()
        {
            StaticData.Scene.SetToRemove(this);
        }
    }
}