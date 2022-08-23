using Microsoft.Xna.Framework;
using TapsasEngine.Sprites;

namespace ZA6.Animations
{
    public class FadeSprite : Animation
    {
        public FadeSprite(Sprite target, float time = 1.5f)
        {
            Stages = new AnimationStage[]
            {
                new FadeSpriteStage() { Target = target, Time = time }
            };
        }

        private class FadeSpriteStage : AnimationStage
        {
            public float Time;
            public Sprite Target;

            public override void Update(float elapsedTime)
            {
                if (elapsedTime < Time)
                {
                    float changePercentage = elapsedTime / Time;
                    float value = 1f - changePercentage;
                    Target.Color = new Color(value, value, value, value);
                }
                else
                {
                    IsDone = true;
                }
            }
        }
    }
}