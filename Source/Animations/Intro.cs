using System;
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
                //new TextFade() { Text = text1 },
                new WaitStage(3f),
                new TextFade(text2, true),
            };
        }

        private class TextFade : AnimationStage
        {
            public string Text;
            public bool IsLower;
            private float _elapsedTime = 0;

            public TextFade(string text, bool isLower)
            {
                Text = text;
                IsLower = isLower;
            }
            
            public override void Update(GameTime gameTime)
            {
                _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            
            public override void Draw(SpriteBatch spriteBatch)
            {
                
            }
        }

        private class TextStack : AnimationStage
        {
            public static string Text = "GAMEOVER";
            private int _y = 60;
            private int _xPadding = 40;
            private int _nextXPadding = 85;
            private float _time = 0.4f;
            private float _elapsedTime = 0;

            
            public override void Update(GameTime gameTime)
            {
                _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (_elapsedTime > _time)
                    IsDone = true;
            }

            public override void Draw(SpriteBatch spriteBatch)
            {
                Utility.DrawOverlay(spriteBatch, Color.Black);
                Static.Scene.DrawPlayerOnly(spriteBatch);

                int letterSpace = 20; //Static.NativeWidth - xPadding * 2 / Text.Length;
                Vector2 endPosition = new Vector2(_nextXPadding + letterSpace / 2, _y);
                float donePercentage = Math.Min(1f, _elapsedTime / _time);
                
                for (int i = Text.Length - 1; i >= 0; i--)
                {
                    string t = Text.Substring(i, 1);    
                    float startX = _xPadding + i * letterSpace + letterSpace / 2;
                    var position = Vector2.Lerp(new Vector2(startX, _y), endPosition, donePercentage);

                    BitmapFontRenderer.DrawString(spriteBatch, t, position);
                }
            }
        }

        private class TextFinish : AnimationStage
        {
            public static string Text = "GAME OVER";
            private int _y = 60;
            private int _xPadding = 85;
            private float _time = 0.2f;
            private float _elapsedTime = 0;

            public override void Enter()
            {
                SFX.LargeBeam.Play();
            }

            public override void Update(GameTime gameTime)
            {
                _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (_elapsedTime > _time)
                    IsDone = true;
            }

            public override void Draw(SpriteBatch spriteBatch)
            {
                Utility.DrawOverlay(spriteBatch, Color.Black);
                Static.Scene.DrawPlayerOnly(spriteBatch);

                int letterSpace = 8; //Static.NativeWidth - xPadding * 2 / Text.Length;
                float donePercentage = Math.Min(1f, _elapsedTime / _time);
                Vector2 startPosition = new Vector2(_xPadding + letterSpace / 2, _y);
                
                for (int i = Text.Length - 1; i >= 0; i--)
                {
                    string t = Text.Substring(i, 1);
                    float endX = _xPadding + i * letterSpace + letterSpace / 2;
                    Vector2 endPosition = new Vector2(endX, _y);
                    var position = Vector2.Lerp(startPosition, endPosition, donePercentage);

                    BitmapFontRenderer.DrawString(spriteBatch, t, position);
                }
            }
        }

        private class WhiteFlash : AnimationStage
        {
            public static string Text = "GAME OVER";
            private int _y = 60;
            private int _xPadding = 85;
            private float _time = 0.4f;
            private float _elapsedTime = 0;

            public override void Update(GameTime gameTime)
            {
                _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                
                if (_elapsedTime > _time)
                    IsDone = true;
            }

            public override void Draw(SpriteBatch spriteBatch)
            {
                float reverseDonePercentage = 1f - Math.Min(1f, _elapsedTime / _time);
                Utility.DrawOverlay(spriteBatch, new Color(reverseDonePercentage, reverseDonePercentage, reverseDonePercentage));
                int letterSpace = 8; //Static.NativeWidth - xPadding * 2 / Text.Length;

                for (int i = Text.Length - 1; i >= 0; i--)
                {
                    string t = Text.Substring(i, 1);
                    float x = _xPadding + i * letterSpace + letterSpace / 2;
                    var position = new Vector2(x, _y);

                    BitmapFontRenderer.DrawString(spriteBatch, t, position);
                }
            }
        }
    }
}