using Microsoft.Xna.Framework;
using TapsasEngine.Utilities;

namespace ZA6.Animations
{
    public class DanceAround : Animation
    {
        public override bool Looping { get => true; }

        public DanceAround(Character target, float interval = 0.6f)
        {
            Vector2 startPosition = target.Position;
            bool firstMoveLeft = Utility.RandomBetween(0, 1) == 0;

            Stages = new AnimationStage[]
            {
                new DanceAroundStage(target, firstMoveLeft, startPosition),
                new WaitStage(interval),
                new DanceAroundStage(target, !firstMoveLeft, startPosition),
                new WaitStage(interval),
            };
        }

        public class DanceAroundStage : Walk.WalkUntilBlockedStage
        {
            private bool _toLeft;

            public DanceAroundStage(Character target, bool toLeft, Vector2 startPosition)
                : base(target, Vector2.Zero, 25f)
            {
                _toLeft = toLeft;
            }

            public override void Enter()
            {
                var distance = new Vector2(
                    Utility.RandomBetween(10, 16),
                    Utility.RandomBetween(-8, 8)
                );

                if (_toLeft)
                    distance.X *= -1;

                _distance = distance;

                base.Enter();
            }
        }
    }
}