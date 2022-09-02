using Microsoft.Xna.Framework;

namespace ZA6.Animations
{
    public class ShiftAround : Animation
    {
        public override bool Looping { get => true; }

        public ShiftAround(MapEntity target, float interval = 0.6f)
        {
            Stages = new AnimationStage[]
            {
                new Move.MoveStage(target, new Vector2(-16, 0), 0.3f),
                new WaitStage(interval),
                new Move.MoveStage(target, new Vector2(16, 0), 0.3f),
                new WaitStage(interval)
            };
        }
    }
}