using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TapsasEngine;

namespace ZA6.Animations
{
    public abstract class AnimationStage
    {
        public Animation Animation { get; set; }
        public bool IsDone { get; set; }
        public float? StageTime = null;
        public virtual bool DrawAfterDone { get => false; }

        public virtual void Enter() {}
        public virtual void Update(float elapsedTime) {}
        public virtual void Draw(SpriteBatch spriteBatch) {}
    }

    public class WaitStage : AnimationStage
    {
        public WaitStage(float time)
        {
            StageTime = time;
        }
    }

    public class WaitForEnterStage : AnimationStage
    {
        public override void Enter()
        {
            IsDone = true;
        }
    }
}