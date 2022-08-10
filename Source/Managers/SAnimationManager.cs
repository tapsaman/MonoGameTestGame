using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZA6.Models;

namespace ZA6.Managers
{
    public class SAnimationManager
    {
        private SAnimation _animation;

        private float _elapsedtime;

        public Vector2 Position;

        public SAnimationManager() {}

        public SAnimationManager(SAnimation animation)
        {
            _animation = animation;
        }

        public void Play(SAnimation animation)
        {
            if (_animation == animation) {
                return;
            }

            _animation = animation;
            _animation.CurrentFrame = 0;
            _elapsedtime = 0f;
        }

        public void Stop()
        {
            _animation.CurrentFrame = 0;
            _elapsedtime = 0f;
        }

        public virtual void OnDone() {}

        public void Update(GameTime gameTime) {
            _elapsedtime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_elapsedtime > _animation.FrameDuration)
            {
                _elapsedtime = 0f;

                if (_animation.CurrentFrame >= _animation.FrameCount - 1) {
                    // On last frame
                    if (_animation.IsLooping)
                        _animation.CurrentFrame = 0;
                    else
                        OnDone();
                }
                else {
                    _animation.CurrentFrame++;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            Vector2 position = (Position + offset + _animation.Offset);
            position.Round();

            spriteBatch.Draw(
                _animation.Texture,
                position,
                _animation.FrameRectangles[_animation.CurrentFrame],
                Color.White
            );
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 offset, Color color)
        {
            spriteBatch.Draw(
                _animation.Texture,
                Position + offset,
                _animation.FrameRectangles[_animation.CurrentFrame],
                color
            );
        }
    }
}