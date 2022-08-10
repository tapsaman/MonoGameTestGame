using Microsoft.Xna.Framework;
using ZA6.Managers;

namespace ZA6.Models
{
    public abstract class AnimationEffect : SAnimationManager
    {
        protected abstract SAnimation Load();

        public AnimationEffect(Vector2 position)
        {
            Position = position;
            SAnimation animation = Load();
            Play(animation);
        }

        public override void OnDone()
        {
            Static.Scene.Remove(this);
        }
    }
}