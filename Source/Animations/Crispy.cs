using Microsoft.Xna.Framework.Graphics;

namespace ZA6.Animations
{
    public class Crispy : Animation
    {
        public Crispy()
        {
            Stages = new AnimationStage[]
            {
                new CrispyStage()
            };
        }

        private class CrispyStage : AnimationStage
        {
            public float Time = 5f;
            private Effect _effect = Shaders.Crispy;

            public override void Enter()
            {
                Music.Play(Songs.CrispyWorld);
                _effect.Parameters["time"].SetValue(0f);
                Static.Renderer.ApplyPostEffect(_effect);
                Static.Scene.Locked = true;
                Static.Scene.LockedCamera = true;
            }

            public override void Update(float elapsedTime)
            {
                if (elapsedTime < Time)
                {
                    _effect.Parameters["time"].SetValue(elapsedTime * 3f);
                }
                else
                {
                    Static.Renderer.ApplyPostEffect(null);
                    _effect.Parameters["time"].SetValue(0f);
                    Music.Play(Songs.Mario);
                    IsDone = true;
                    Static.Scene.Locked = false;
                    Static.Scene.LockedCamera = false;
                    Static.Scene.UpdateCamera(Static.Player.Position);
                }
            }
        }
    }
}