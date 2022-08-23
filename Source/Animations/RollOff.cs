using Microsoft.Xna.Framework;
using TapsasEngine.Enums;

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
            private int _turns;
            
            public RollStage(Character target)
            {
                _target = target;
            }

            public override void Enter()
            {
                _turns = 0;
            }

            public override void Update(float elapsedTime)
            {
                if (elapsedTime > 0.15f)
                {
                    elapsedTime = 0;
                    _target.Facing = _target.Facing.Next();
                    _turns++;

                    if (_turns > 16)
                    {
                        IsDone = true;
                    }
                }
            }
        }

        private class RollOffStage : AnimationStage
        {
            public float Time = 0.5f;
            private Character _target;
            private float _elapsedTurnTime;
            private int _turns;
            
            public RollOffStage(Character target)
            {
                _target = target;
            }

            public override void Update(float elapsedTime)
            {
                IsDone = true;
                _elapsedTurnTime += Animation.Delta;

                if (_elapsedTurnTime > 0.15f)
                {
                    _elapsedTurnTime = 0;
                    _target.Facing = _target.Facing.Next();
                    _turns++;
                }
                
                _target.Position -= new Vector2(Animation.Delta * 70f, 0f);
            }
        }
    }
}