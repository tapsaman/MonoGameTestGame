using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TapsasEngine;

namespace ZA6.Models
{
    public class AnimationEffect : IAnimationEffect
    {
        private Animation _animation;

        public AnimationEffect(Animation animation)
        {
            _animation = animation;
            _animation.Enter();
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 drawOffset)
        {
            _animation.DrawOffset = drawOffset;
            _animation.Draw(spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            _animation.Update(gameTime);

            if (_animation.IsDone)
            {
                Static.Scene.Remove(this);
            }
        }
    }
}