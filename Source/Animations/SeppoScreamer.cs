using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TapsasEngine.Utilities;

namespace ZA6.Animations
{
    public class SeppoScreamer : Animation
    {
        public SeppoScreamer()
        {
            Stages = new AnimationStage[]
            {
                new WaitStage(3f),
                new SeppoScreamerStage(),
                new KillPlayerStage()
            };
        }

        private class SeppoScreamerStage : AnimationStage
        {
            public float Time = 5f;
            public Texture2D Image = Img.SeppoFace;
            private Vector2 _position = Vector2.Zero;

            public override void Enter()
            {
                Music.Play(Songs.Screamer);
                Static.Scene.Locked = true;
            }

            public override void Update(float elapsedTime)
            {
                if (elapsedTime < Time)
                {
                    _position = new Vector2((float)Utility.RandomBetween(-15, 15), (float)Utility.RandomBetween(-30, 0));
                }
                else
                {
                    IsDone = true;
                    Static.Player.Position = new Vector2((float)double.NaN, (float)double.NaN);
                    Music.Play(Songs.Forest);
                    Static.GameData.Save("scenario", "mushroom");
                }
            }

            public override void Draw(SpriteBatch spriteBatch)
            {
                spriteBatch.Draw(Image, _position, Color.White);
            }
        }

        private class KillPlayerStage : AnimationStage
        {
            public float Time = 1.6f;

            public override void Update(float elapsedTime)
            {
                if (Static.Player.Health == 0)
                {
                    IsDone = true;
                }
                else if (elapsedTime < Time)
                {
                    Static.Player.TakeDamage(Vector2.Zero, 2);
                }
            }
        }
    }
}