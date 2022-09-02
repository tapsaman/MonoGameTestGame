using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TapsasEngine.Utilities;
using ZA6.Managers;

namespace ZA6.Animations
{
    public class Intro : Animation
    {
        public Intro(string text1, string text2)
        {
            Stages = new AnimationStage[]
            {
                new WaitStage(1f),
                new TextFade(text1, false),
                new WaitStage(3f),
                new TextFade(text2, true),
            };
        }

        private class TextFade : AnimationStage
        {
            public override bool DrawAfterDone { get => true; }
            public string Text;
            public bool IsLower;
            private Point _size;
            private Color _color;

            public TextFade(string text, bool isLower)
            {
                Text = text;
                IsLower = isLower;
                _size = BitmapFontRenderer.CalculateSize(text);
                StageTime = 3f;
            }
            
            public override void Update(float elapsedTime)
            {
                float v = Utility.RoundTo(elapsedTime / (float)StageTime, 0.10f);
                _color = new Color(v, v, v, v);
            }
            
            public override void Draw(SpriteBatch spriteBatch)
            {
                Vector2 position = !IsLower
                    ? new Vector2(40, Static.NativeHeight / 2 - 20 - _size.Y)
                    : new Vector2(40, Static.NativeHeight / 2 + 20);
                
                BitmapFontRenderer.DrawString(spriteBatch, Text, position, color: _color);
            }
        }
    }
}