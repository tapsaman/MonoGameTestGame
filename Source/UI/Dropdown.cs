using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using TapsasEngine;

namespace ZA6.UI
{
    public class Dropdown<T> : Button
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
                    Sys.LogError("Given dropdown input value not in options");
                }
            }
        }
        private T _value;
        private T[] _options;
        private int _optionIndex;
        private const float _INPUT_WAIT_TIME = 0.2f;
        private float _elapsedInputWaitTime;

        public Dropdown(SpriteFont font, T[] options)
            : base(font)
        {
            _options = options;
            Click += OpenDropdownMenu;
        }

        private void OpenDropdownMenu(object sender, EventArgs e)
        {
            var buttons = new UIComponent[_options.Length];

            for (int i = 0; i < _options.Length; i++)
            {
                buttons[i] = new Button(Static.FontSmall)
                {
                    Text = _options[i].ToString()
                };
            }

            var menu = new DropdownMenu()
            {
                Position = new Vector2(Position.X, Position.Y + Height),
                Width = Width,
                Components = buttons
            };
        }
    
        public override void Draw(SpriteBatch spriteBatch)
        {
            TextMargin = new Vector2(
                (Rectangle.Width / 2) - (_font.MeasureString(Text).X / 2),
                (Padding.Y)
            );

            base.Draw(spriteBatch);

            var text = Value.ToString();
            var size = _font.MeasureString(text);
            var x = Rectangle.X + Rectangle.Width / 2 - size.X / 2;
            var y = Rectangle.Bottom - Padding.Y - size.Y + (_isActive ? 1 : 0);

            spriteBatch.DrawString(_font, Value.ToString(), new Vector2(x, y), PenColor);
        }

    }
}