using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTestGame.Models;

namespace MonoGameTestGame.Managers
{
    public class AnimationManager
    {
        private Animation _animation;

        private float _elapsedtime;

        public Vector2 Position;

        public AnimationManager() {}

        public AnimationManager(Animation animation)
        {
            _animation = animation;
        }

        public void Play(Animation animation)
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
            Vector2 position = (Position + offset);
            position.Round();

            spriteBatch.Draw(
                _animation.Texture,
                position,
                new Rectangle(
                    _animation.CurrentFrame * _animation.FrameWidth + _animation.TextureXPos * _animation.FrameWidth,
                    _animation.TextureYPos * _animation.FrameHeight,
                    _animation.FrameWidth,
                    _animation.FrameHeight
                ),
                Color.White
            );
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 offset, Color color)
        {
            spriteBatch.Draw(
                _animation.Texture,
                Position + offset,
                new Rectangle(
                    _animation.CurrentFrame * _animation.FrameWidth + _animation.TextureXPos * _animation.FrameWidth,
                    _animation.TextureYPos * _animation.FrameHeight,
                    _animation.FrameWidth,
                    _animation.FrameHeight
                ),
                color
            );
        }
    }
}