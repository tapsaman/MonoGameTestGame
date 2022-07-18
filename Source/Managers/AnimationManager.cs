using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTestGame.Models;

namespace MonoGameTestGame.Managers
{
    public class AnimationManager
    {
        private Models.Animation _animation;

        private float _timer;

        public Vector2 Position;

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
            _timer = 0f;
        }

        public void Stop()
        {
            _animation.CurrentFrame = 0;
            _timer = 0f;
        }

        public void Update(GameTime gameTime) {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_timer > _animation.FrameDuration)
            {
                _timer = 0f;

                if (_animation.CurrentFrame >= _animation.FrameCount - 1) {
                    // On last frame
                    if (_animation.IsLooping)
                        _animation.CurrentFrame = 0;
                }
                else {
                    _animation.CurrentFrame++;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 offset)
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
                Color.White
            );
        }
    }
}