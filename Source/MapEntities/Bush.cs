using Microsoft.Xna.Framework;

namespace MonoGameTestGame
{
    public class Bush : MapObject
    {
        public Bush()
        {
            Hitbox.Load(16, 16);
            Interactable = false;
            Hittable = true;
            Colliding = true;
            Sprite.SetTexture(StaticData.ObjectTexture, new Rectangle(0, 0, 16, 16));
            Trigger += Destroy;
        }

        private void Destroy()
        {
            StaticData.Scene.Add(new Animations.BushSlash(Position + new Vector2(8)));
            StaticData.Scene.SetToRemove(this);
        }
    }
}