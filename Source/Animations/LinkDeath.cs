using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTestGame.Managers;

namespace MonoGameTestGame.Animations
{
    public class LinkDeath : Animation
    {
        public LinkDeath(Character target)
        {
            Stages = new AnimationStage[]
            {
                new TurnAroundStage(target, 0.13f),
                new FallStage(target, 0.13f)
            };
        }

        private class TurnAroundStage : AnimationStage
        {
            private Character _target;
            private float _elapsedTime = 0;
            private float _frameDuration = 0;
            private const int _TURNS = 12;
            private int _turn_count;
            
            public TurnAroundStage(Character target, float frameDuration)
            {
                _target = target;
                _frameDuration = frameDuration;
            }

            public override void Enter()
            {
                _turn_count = 0;
                _elapsedTime = 0;
            }
            public override void Update(GameTime gameTime)
            {
                _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (_elapsedTime > _frameDuration)
                {
                    _elapsedTime = 0;
                    _target.Direction = _target.Direction.Next();
                    _turn_count++;

                    if (_turn_count > _TURNS)
                    {
                        IsDone = true;
                    }
                }
            }
            public override void Draw(SpriteBatch spriteBatch) {}
        }

        private class FallStage : AnimationStage
        {
            private Character _target;
            private float _elapsedTime = 0;
            private float _frameDuration = 0;
            
            public FallStage(Character target, float frameDuration)
            {
                _target = target;
                _frameDuration = frameDuration;
            }
            public override void Enter()
            {
                _elapsedTime = 0;
            }
            public override void Update(GameTime gameTime)
            {
                IsDone = true;
                _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                
                _target.Position -= new Vector2((float)gameTime.ElapsedGameTime.TotalSeconds * 70f, 0f);
            }
            public override void Draw(SpriteBatch spriteBatch) {}
        }
    }
}