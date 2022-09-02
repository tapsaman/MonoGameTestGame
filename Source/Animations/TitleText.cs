using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TapsasEngine.Utilities;
using ZA6.Managers;

namespace ZA6.Animations
{
    public class TitleText : Animation
    {
        public TitleText()
        {
            Stages = new AnimationStage[]
            {
                new WaitForEnterStage(),
                new WaitStage(2.5f),
                new TitleTextStage("zeldan seikkailut mikä mikä maassa vittu")
            };
        }

        private class TitleTextStage : AnimationStage
        {
            public float Time = 30f;
            public string Text = null;
            private float _x;
            private float _y;
            private float _distance;
            
            public TitleTextStage(string text)
            {
                Text = text;
            }

            public override void Enter()
            {
                var textSize = BitmapFontRenderer.CalculateSize(Text);
                _x = Static.NativeWidth;
                _y = Static.NativeHeight / 2 - textSize.Y / 2;
                _distance = _x - -textSize.X;
            }
            public override void Update(float elapsedTime)
            {
                //if (Text == null)
                 //   return;
                
                if (elapsedTime <= Time)
                {
                    _x = Static.NativeWidth - _distance * (elapsedTime / Time);
                }
                else
                {
                    IsDone = true;
                }
            }

            public override void Draw(SpriteBatch spriteBatch)
            {
                if (IsDone/* || Text == null*/)
                    return;
                
                BitmapFontRenderer.DrawString(spriteBatch, Text, new Vector2(_x, _y));
            }
        }
    }
}