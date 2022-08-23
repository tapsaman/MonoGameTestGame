using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using TapsasEngine;
using TapsasEngine.Sprites;
using TapsasEngine.Utilities;

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
        private UIComponent[] _buttons;
        private DropdownMenu _menu;
        private int _optionIndex;
        private static Sprite _buttonBackground = new Sprite(
            Utility.CreateColorTexture(1, 1, Color.White)
        );

        public Dropdown(SpriteFont font, T[] options)
            : base(font)
        {
            Click += OpenDropdownMenu;
            _options = options;
            _buttons = new UIComponent[_options.Length];

            for (int i = 0; i < _options.Length; i++)
            {
                _buttons[i] = new Button(_buttonBackground, Static.FontSmall, MenuButtonClick)
                {
                    Text = _options[i].ToString()
                };
            }
        }

        private void OpenDropdownMenu(object sender, EventArgs e)
        {
            _menu = new DropdownMenu()
            {
                Position = new Vector2(Position.X, Position.Y + Height - 2),
                Width = Width,
                Components = _buttons
            };

            UIManager.Add(_menu);
            _menu.FocusOn(_buttons[_optionIndex]);
        }

        private void MenuButtonClick(object sender, EventArgs e)
        {
            int optionIndex = Array.IndexOf(_buttons, sender);

            if (optionIndex >= 0 && optionIndex < _options.Length)
            {
                OnChange?.Invoke(_options[optionIndex]);
            }
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