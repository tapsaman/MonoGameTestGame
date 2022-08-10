using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace ZA6.Controls
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
                    Sys.LogError("Given slider input value not in steps");
                }
            }
        }
        private T _value;
        private T[] _options;
        private int _optionIndex;
        private Texture2D _handleTexture;
        private const float _INPUT_WAIT_TIME = 0.2f;
        private float _elapsedInputWaitTime;

        public Select(Texture2D texture, SpriteFont font, T[] options)
            : base(texture, font)
        {
            _handleTexture = Static.Content.Load<Texture2D>("slider-handle");
            _options = options;
        }
        
        public override void Update(GameTime gameTime)
        {
            if (Disabled)
                return;
            
            _elapsedInputWaitTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            var mouse = Input.P1.GetMouseRectangle();

            _isActive = false;
            _isHovering = mouse.Intersects(Rectangle);

            if (_isHovering && Input.P1.JustPressedMouseLeft())
            {
                _isActive = true;

                if (_elapsedInputWaitTime > _INPUT_WAIT_TIME)
                {
                    _elapsedInputWaitTime = 0;
                    
                    if (mouse.X < Rectangle.Center.X)
                        _optionIndex = (_optionIndex != 0 ? _optionIndex : _options.Length) - 1;
                    else
                        _optionIndex = _optionIndex < _options.Length - 1 ? _optionIndex + 1: 0;
                    
                    ChangeSound?.Play();
                    OnChange?.Invoke(_options[_optionIndex]);
                }

                return;
            }

            if (IsFocused == true)
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
            var color = Color.White;
            
            if (_isActive)
                color = ActiveColor;
            else if (_isHovering || IsFocused == true)
                color = Color.Gray;

            spriteBatch.Draw(_texture, Rectangle, color);

            DrawLeftHandle(spriteBatch);
            DrawRightHandle(spriteBatch);

            {
                var text = Value.ToString();
                var size = _font.MeasureString(text);
                var x = (Rectangle.X + (Rectangle.Width / 2)) - (size.X / 2);
                var y = (Rectangle.Bottom - Padding.Y - size.Y);

                spriteBatch.DrawString(_font, Value.ToString(), new Vector2(x, y), PenColor);
            }

            if (!string.IsNullOrEmpty(Text)) {
                var x = (Rectangle.X + (Rectangle.Width / 2)) - (_font.MeasureString(Text).X / 2);
                var y = (Rectangle.Y + Padding.Y);

                spriteBatch.DrawString(_font, Text, new Vector2(x, y), PenColor);
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