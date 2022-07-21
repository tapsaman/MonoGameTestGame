using Microsoft.Xna.Framework;
using MonoGameTestGame.Managers;

namespace MonoGameTestGame.Models
{
    public abstract class AnimationEffect : AnimationManager
    {
        protected abstract Animation Load();

        public AnimationEffect(Vector2 position)
        {
            Position = position;
            Animation animation = Load();
            Play(animation);
        }

        public override void OnDone()
        {
            StaticData.Scene.SetToRemove(this);
        }
    }
}