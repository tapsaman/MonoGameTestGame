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
                new WaitStage(2.5f),
                new TitleTextStage()
            };
        }

        private class TitleTextStage : AnimationStage
        {
            private const string _TEXT = "zeldan seikkailut mikä mikä maassa vittu";
            public float Time = 26f;
            private string _text = null;
            private float _x;
            private float _y;
            private float _distance;
            private float _elapsedTime;
            
            public TitleTextStage() {}

            public override void Enter()
            {
                _text = _TEXT;
                var textSize = BitmapFontRenderer.CalculateSize(_text);
                _x = Static.NativeWidth;
                _y = Static.NativeHeight / 2 - textSize.Y / 2;
                _distance = _x - -textSize.X;
            }
            public override void Update(GameTime gameTime)
            {
                if (_text == null)
                    return;
                
                _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (_elapsedTime <= Time)
                {
                    _x = Static.NativeWidth - _distance * (_elapsedTime / Time);
                    
                }
                else
                {
                    IsDone = true;
                }
            }

            public override void Draw(SpriteBatch spriteBatch)
            {
                if (_text == null || IsDone)
                    return;
                
                BitmapFontRenderer.DrawString(spriteBatch, _text, new Vector2(_x, _y));
            }
        }
    }
}