using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using ZA6;
using ZA6.Managers;
using ZA6.Models;

namespace TapsasEngine.Sprites
{
    public class AnimatedSprite : Sprite
    {
        public Effect Effect;
        protected SAnimationManager _animationManager;

        protected Dictionary<string, SAnimation> _animations;

        public AnimatedSprite(Dictionary<string, SAnimation> animations, string initialAnimationName)
        {
            _animations = animations;
            _animationManager = new SAnimationManager(_animations[initialAnimationName]);
        }

        public virtual void Update(GameTime gameTime)
        {
            _animationManager.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            if (Effect == null)
            {
                _animationManager.Draw(spriteBatch, position, Color);
            }
            else
            {
                Static.Renderer.ChangeToEffect(Shaders.Evaporate);
                _animationManager.Draw(spriteBatch, position, Color);
                Static.Renderer.ChangeToDefault();
            }
        }

        public void SetAnimation(string animationName = null)
        {
            if (animationName == null)
            {
                _animationManager.Stop();
            }
            else
            {
                _animationManager.Play(_animations[animationName]);
            }
        }

        public void SafeSetAnimation(string animationName)
        {
            if (_animations.ContainsKey(animationName))
                _animationManager.Play(_animations[animationName]);
        }
    }
}