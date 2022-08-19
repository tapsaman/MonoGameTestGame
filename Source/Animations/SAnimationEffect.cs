using Microsoft.Xna.Framework;
using TapsasEngine;
using ZA6.Managers;
using ZA6.Models;

namespace ZA6
{
    public abstract class SAnimationEffect : SAnimationManager, IAnimationEffect
    {
        protected abstract SAnimation Load();

        public SAnimationEffect(Vector2 position)
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