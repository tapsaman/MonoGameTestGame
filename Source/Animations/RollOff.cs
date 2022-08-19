using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TapsasEngine.Enums;
using ZA6.Managers;

namespace ZA6.Animations
{
    public class RollOff : Animation
    {
        public RollOff(Character target)
        {
            Stages = new AnimationStage[]
            {
                new RollStage(target),
                new RollOffStage(target)
            };
        }

        private class RollStage : AnimationStage
        {
            public float Time = 0.5f;
            private Character _target;
            private float _elapsedTime = 0;
            private int _turns;
            
            public RollStage(Character target)
            {
                _target = target;
            }

            public override void Enter()
            {
                _turns = 0;
                _elapsedTime = 0;
            }
            public override void Update(GameTime gameTime)
            {
                _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (_elapsedTime > 0.15f)
                {
                    _elapsedTime = 0;
                    _target.Facing = _target.Facing.Next();
                    _turns++;

                    if (_turns > 16)
                    {
                        IsDone = true;
                    }
                }
            }
            public override void Draw(SpriteBatch spriteBatch) {}
        }

        private class RollOffStage : AnimationStage
        {
            public float Time = 0.5f;
            private Character _target;
            private float _elapsedTurnTime = 0;
            private float _elapsedTime = 0;
            private int _turns;
            
            public RollOffStage(Character target)
            {
                _target = target;
            }
            public override void Update(GameTime gameTime)
            {
                IsDone = true;
                _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                _elapsedTurnTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (_elapsedTurnTime > 0.15f)
                {
                    _elapsedTurnTime = 0;
                    _target.Facing = _target.Facing.Next();
                    _turns++;
                }
                
                _target.Position -= new Vector2((float)gameTime.ElapsedGameTime.TotalSeconds * 70f, 0f);
            }
            public override void Draw(SpriteBatch spriteBatch) {}
        }
    }
}