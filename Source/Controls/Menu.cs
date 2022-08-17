using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZA6.Controls;

namespace ZA6
{
    public class Menu
    {
        private List<UIComponent> Components;
        private List<UIComponent> FocusableComponents;
        public int Padding = 5;
        public Sides Margin;
        public int? FocusIndex { get; private set; } = null;
        public bool Disabled
        {
            get { return _disabled; }
            set
            {
                _disabled = value;

                foreach (var component in FocusableComponents)
                {
                    FocusIndex = null;
                    component.Disabled = _disabled;
                    component.IsFocused = false;
                }
            }
        }
        public Color OverlayColor
        {
            set
            {
                _overlay = Utility.CreateColorTexture(Static.NativeWidth, Static.NativeHeight, value);
            }
        }
        protected Texture2D _bgTexture;
        protected Texture2D _overlay;
        private bool _disabled;
        private Rectangle _contentRectangle;
        private Rectangle _outerRectangle;
        private const float _INPUT_WAIT_TIME = 0.2f;
        private float _elapsedInputWaitTime;

        public Menu()
        {
            Components = new List<UIComponent>();
            FocusableComponents = new List<UIComponent>();
        }

        public void Add(UIComponent component)
        {
            Components.Add(component);

            if (component.Focusable)
            {
                FocusableComponents.Add(component);
            }

            if (_disabled)
            {
                component.Disabled = true;
            }
        }

        public void Remove(UIComponent component)
        {
            Components.Remove(component);

            if (component.Focusable)
            {
                FocusableComponents.Remove(component);
            }
        }

        protected void CalculateSize()
        {
            int greatestWidth = 0;
            int combinedHeight = 0;
            int combinedPadding = Padding * Components.Count - 1;

            foreach (var item in Components)
            {
                if (greatestWidth < item.Width)
                    greatestWidth = item.Width;
                
                combinedHeight += item.Height;
            }

            int contentWidth = greatestWidth;
            int contentHeight = combinedHeight + combinedPadding;

            _contentRectangle = new Rectangle(
                Static.NativeWidth / 2 - contentWidth / 2 + Margin.Left,
                Static.NativeHeight / 2 - contentHeight / 2 + Margin.Top,
                contentWidth,
                contentHeight
            );

            _outerRectangle = new Rectangle(
                _contentRectangle.X - Padding,
                _contentRectangle.Y - Padding,
                _contentRectangle.Width + Padding * 2,
                _contentRectangle.Height + Padding * 2
            );
        }

        public virtual void Update(GameTime gameTime)
        {
            if (!_disabled)
            {
                DetermineKeyInput(gameTime);
            }

            foreach (var item in Components)
            {
                item.Update(gameTime);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (_overlay != null)
            {
                spriteBatch.Draw(_overlay, Vector2.Zero, new Color(50, 50, 50, 100));
            }

            if (_bgTexture != null)
            {
                spriteBatch.Draw(
                    _bgTexture,
                    _outerRectangle.Location.ToVector2(),
                    null,
                    Color.White,
                    0,
                    Vector2.Zero,
                    _outerRectangle.Size.ToVector2() / _bgTexture.Bounds.Size.ToVector2(), //_outerRectangle.Size.ToVector2(),
                    SpriteEffects.None,
                    0
                );
            }

            float y = _contentRectangle.Y;

            foreach (var item in Components)
            {
                // Center items 
                int x = _contentRectangle.X + _contentRectangle.Width / 2 - item.Width / 2;
                item.Position = new Vector2(x, y);
                item.Draw(spriteBatch);
                y += item.Height + Padding;
            }
        }

        private void DetermineKeyInput(GameTime gameTime)
        {
            _elapsedInputWaitTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_elapsedInputWaitTime < _INPUT_WAIT_TIME)
            {
                return;
            }

            if (FocusIndex == null)
            {
                if (Input.P1.IsAnyKeyPressed())
                {
                    FocusIndex = 0;
                    UpdateFocus();
                } 
                return;
            }

            var dir = Input.P1.GetDirectionVector();
            
            if (dir.Y < 0)
            {
                FocusIndex -= 1;

                if (FocusIndex < 0)
                    FocusIndex = FocusableComponents.Count - 1;

                UpdateFocus();
            }
            else if (dir.Y > 0)
            {
                FocusIndex += 1;
                
                if (FocusIndex >= FocusableComponents.Count)
                    FocusIndex = 0;

                UpdateFocus();
            }
        }

        private void UpdateFocus()
        {
            SFX.Cursor.Play();
            _elapsedInputWaitTime = 0;

            for (int i = 0; i < FocusableComponents.Count; i++)
            {
                FocusableComponents[i].IsFocused = i == FocusIndex;
            }
        }
    }
}