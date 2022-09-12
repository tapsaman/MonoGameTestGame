using Microsoft.Xna.Framework;
using TapsasEngine.Enums;
using TapsasEngine.Utilities;

namespace ZA6.Animations
{
    public class RunAround : Animation
    {
        public override bool Looping { get => true; }

        public RunAround(Character target)
        {
            Stages = new AnimationStage[]
            {
                new RunAroundStage(target),
            };
        }

        public class RunAroundStage : Walk.WalkUntilBlockedStage
        {
            private bool _toLeft;

            public RunAroundStage(Character target)
                : base(target, Vector2.Zero, 110f) {}

            public override void Enter()
            {
                _distance = Utility.RandomDirection().ToVector() * 16;

                base.Enter();
            }
        }
    }
}