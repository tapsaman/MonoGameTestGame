using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TapsasEngine;
using TapsasEngine.Sprites;

namespace ZA6.UI
{
    public class Slider : Button
    {
        public static SoundEffect ChangeSound;
        public Vector2 Padding = new Vector2(16, 3);
        public event Action<float> OnChange;
        public float Value {
            get { return _value; }
            set
            {
                _value = value;
                _stepIndex = -1;

                for (int i = 0; i < _steps.Length; i++)
                {
                    if (_value == _steps[i])
                    {
                        _stepIndex = i;
                        break;
                    }
                }
                
                if (_stepIndex == -1)
                {
                    Sys.LogError("Given slider input value not in steps");
                }
            }
        }
        protected bool _isDragging;
        private float _value;
        private int _stepIndex;
        private float[] _steps;
        private Texture2D _handleTexture;
        private const float _INPUT_WAIT_TIME = 0.2f;
        private float _elapsedInputWaitTime;

        public Slider(SpriteFont font, float minValue, float maxValue, float stepInterval)
            : base(font)
        {
            _handleTexture = Static.Content.Load<Texture2D>("slider-handle");
            int stepCount = (int)Math.Floor((maxValue - minValue) / stepInterval);
            _steps = new float[stepCount];

            for (int i = 0; i < stepCount; i++)
            {
                _steps[i] = minValue + i * stepInterval;
            }
        }
        
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Disabled)
                return;
            
            _elapsedInputWaitTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (!_isDragging && _isHovering && Input.P1.JustPressedMouseLeft())
            {
                _isDragging = true;
            }
            else if (_isDragging)
            {
                if (Input.P1.JustReleasedMouseLeft())
                {
                    _isDragging = false;
                }
                else
                {
                    _isActive = true;
                    var mouse = Input.P1.GetMouseRectangle();
                    float startX = Rectangle.Left + Padding.X;
                    float stepDrawInterval = (Width - (Padding.X * 2)) / (_steps.Length - 1);
                    int newStepIndex = (int)Math.Round((double)(mouse.X - startX) / stepDrawInterval);
                    newStepIndex = Math.Max(0, Math.Min(_steps.Length - 1, newStepIndex));

                    if (newStepIndex != _stepIndex)
                    {
                        ChangeSound?.Play();
                        _stepIndex = newStepIndex;
                        OnChange?.Invoke(_steps[_stepIndex]);
                    }
                }
            }
            else if (IsFocused == true)
            {
                var dir = Input.P1.GetDirectionVector();

                if (dir.X < 0)
                {
                    _isActive = true;

                    if (_stepIndex > 0 && _elapsedInputWaitTime > _INPUT_WAIT_TIME)
                    {
                        _elapsedInputWaitTime = 0;
                        ChangeSound?.Play();
                        _stepIndex -= 1;
                        OnChange?.Invoke(_steps[_stepIndex]);
                    }
                }
                else if (dir.X > 0)
                {
                    _isActive = true;

                    if (_stepIndex + 1 < _steps.Length && _elapsedInputWaitTime > _INPUT_WAIT_TIME)
                    {
                        _elapsedInputWaitTime = 0;
                        ChangeSound?.Play();
                        _stepIndex += 1;
                        OnChange?.Invoke(_steps[_stepIndex]);
                    }
                }
            }
        }
    
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            float startX = (int)Position.X + Padding.X;
            float stepDrawInterval = ((float)Width - (Padding.X * 2)) / (float)(_steps.Length - 1);

            spriteBatch.Draw(
                _handleTexture,
                new Vector2(
                    startX + (_stepIndex * stepDrawInterval) - (_handleTexture.Width / 2),
                    Rectangle.Bottom - Padding.Y - _handleTexture.Height - 1
                ),
                Color.White
            );
        }

        public class Step<T>
        {
            public T Value;
        }
    }
}