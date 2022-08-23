using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TapsasEngine.Enums;
using ZA6.Managers;

namespace ZA6.Animations
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
                _turn_count = -1;
            }

            public override void Update(float elapsedTime)
            {
                if (elapsedTime > _frameDuration * _turn_count)
                {
                    _target.Facing = _target.Facing.Next();
                    _turn_count++;

                    if (_turn_count > _TURNS)
                    {
                        IsDone = true;
                    }
                }
            }
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

            public override void Update(float elapsedTime)
            {
                IsDone = true;
                _target.Position -= new Vector2(Animation.Delta * 70f, 0f);
            }
        }
    }
}