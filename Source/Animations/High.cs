using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZA6.Managers;

namespace ZA6.Animations
{
    public class High : Animation
    {
        public High()
        {
            Stages = new AnimationStage[]
            {
                new HighStage()
            };
        }

        private class HighStage : AnimationStage
        {
            private Effect _effect = Shaders.Wavy;

            public override void Enter()
            {
                Shaders.Wavy.Parameters["waveWidth"].SetValue(0f);
                Shaders.Wavy.Parameters["waveHeight"].SetValue(0f);
                Shaders.Wavy.Parameters["yOffset"].SetValue(0f);
                Static.Renderer.ApplyPostEffect(_effect);
            }

            public override void Update(float elapsedTime)
            {
                //float value = elapsedTime / 4f * (elapsedTime * 0.01f);
                Shaders.Wavy.Parameters["waveWidth"].SetValue((float)Math.Min(elapsedTime * 0.01f, 0.1f));
                Shaders.Wavy.Parameters["waveHeight"].SetValue((float)Math.Min(elapsedTime * 0.5f, 30f));
                Shaders.Wavy.Parameters["yOffset"].SetValue(elapsedTime / 6f);
            }
        }
    }
}