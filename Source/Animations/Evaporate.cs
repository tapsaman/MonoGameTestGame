using Microsoft.Xna.Framework.Graphics;
using TapsasEngine.Sprites;

namespace ZA6.Animations
{
    public class Evaporate : Animation
    {
        public Evaporate(AnimatedSprite target, bool disappear, float time = 3f)
        {
            Stages = new AnimationStage[]
            {
                new EvaporateStage() { Target = target, Disappear = disappear, StageTime = time }
            };
        }

        private class EvaporateStage : AnimationStage
        {
            public AnimatedSprite Target;
            public bool Disappear;
            private Shaders.EvaporateShader _effect = Shaders.Evaporate;

            public override void Enter()
            {
                _effect.Reset(Disappear);
                _effect.SetParameters(330, 0, 30);
                _effect.Time = (float)StageTime;
                Target.Effect = _effect;
            }

            public override void Update(float elapsedTime)
            {
                if (elapsedTime > StageTime)
                {
                    IsDone = true;
                    Target.Effect = null;
                }
            }
        }
    }
}