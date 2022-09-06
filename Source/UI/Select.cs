using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using TapsasEngine;

namespace ZA6.UI
{
    public class Select<T> : Button
    {
        public static SoundEffect ChangeSound;
        public Vector2 Padding = new Vector2(4, 1);
        public event Action<T> OnChange;
        public T Value {
            get { return _value; }
            set
            {
                _value = value;
                _optionIndex = -1;

                for (int i = 0; i < _options.Length; i++)
                {
                    if (EqualityComparer<T>.Default.Equals(_value, _options[i]))
                    {
                        _optionIndex = i;
                        break;
                    }
                }
                
                if (_optionIndex == -1)
                {
                    Sys.LogError("Given select input value not in options");
                }
            }
        }
        private T _value;
        private T[] _options;
        private int _optionIndex;
        private Texture2D _handleTexture;
        private const float _INPUT_WAIT_TIME = 0.2f;
        private float _elapsedInputWaitTime;

        public Select(SpriteFont font, T[] options)
            : base(font)
        {
            _handleTexture = Static.Content.Load<Texture2D>("UI/slider-handle");
            _options = options;
        }
        
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Disabled)
                return;
            
            _elapsedInputWaitTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_isHovering && Input.P1.JustPressedMouseLeft())
            {
                var mouse = Input.P1.GetMouseRectangle();
                
                if (mouse.X < Rectangle.Center.X)
                    _optionIndex = (_optionIndex != 0 ? _optionIndex : _options.Length) - 1;
                else
                    _optionIndex = _optionIndex < _options.Length - 1 ? _optionIndex + 1: 0;
                
                ChangeSound?.Play();
                OnChange?.Invoke(_options[_optionIndex]);
            }
            else if (IsFocused == true)
            {
                var dir = Input.P1.GetDirectionVector();

                if (dir.X < 0)
                {
                    _isActive = true;

                    if (_elapsedInputWaitTime > _INPUT_WAIT_TIME)
                    {
                        _elapsedInputWaitTime = 0;
                        _optionIndex = (_optionIndex != 0 ? _optionIndex : _options.Length) - 1;
                        ChangeSound?.Play();
                        OnChange?.Invoke(_options[_optionIndex]);
                    }
                }
                else if (dir.X > 0)
                {
                    _isActive = true;

                    if (_elapsedInputWaitTime > _INPUT_WAIT_TIME)
                    {
                        _elapsedInputWaitTime = 0;
                        _optionIndex = _optionIndex < _options.Length - 1 ? _optionIndex + 1: 0;
                        ChangeSound?.Play();
                        OnChange?.Invoke(_options[_optionIndex]);
                    }
                }
            }
        }
    
        public override void Draw(SpriteBatch spriteBatch)
        {
            TextMargin = new Vector2(
                (Rectangle.Width / 2) - (_font.MeasureString(Text).X / 2),
                (Padding.Y)
            );

            base.Draw(spriteBatch);

            DrawLeftHandle(spriteBatch);
            DrawRightHandle(spriteBatch);

            {
                var text = Value.ToString();
                var size = _font.MeasureString(text);
                var x = (Rectangle.X + (Rectangle.Width / 2)) - (size.X / 2);
                var y = (Rectangle.Bottom - Padding.Y - size.Y);

                spriteBatch.DrawString(_font, Value.ToString(), new Vector2(x, y), PenColor);
            }
        }

        private void DrawLeftHandle(SpriteBatch spriteBatch)
        {
            var origin = new Vector2(
                _handleTexture.Width / 2,
                _handleTexture.Height / 2
            );
            Vector2 pos =  new Vector2(
                origin.X + Rectangle.Left + Padding.X,
                origin.Y + Rectangle.Bottom - Padding.Y - _handleTexture.Width - 1
            );

            spriteBatch.Draw(
                _handleTexture,
               pos,
                null,
                Color.White,
                (float)Math.PI / 2,
                origin,
                Vector2.One,
                SpriteEffects.None,
                0
            );
        }

        private void DrawRightHandle(SpriteBatch spriteBatch)
        {
            var origin = new Vector2(
                _handleTexture.Width / 2,
                _handleTexture.Height / 2
            );
            var pos =  new Vector2(
                origin.X + Rectangle.Right - Padding.X - _handleTexture.Width,
                origin.Y + Rectangle.Bottom - Padding.Y - _handleTexture.Width - 1
            );

            spriteBatch.Draw(
                _handleTexture,
               pos,
                null,
                Color.White,
                (float)Math.PI * 1.5f,
                origin,
                Vector2.One,
                SpriteEffects.None,
                0
            );
        }
    }
}