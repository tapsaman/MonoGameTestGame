using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TapsasEngine.Utilities;

namespace ZA6.UI
{
    public abstract class UIContainer : FocusableComponent
    {
        private const float _INPUT_WAIT_TIME = 0.24f;
        private float _elapsedInputWaitTime;

        public UIComponent[] Components
        {
            get => _components;
            set
            {
                _components = value;
                List<FocusableComponent> focusableList = new List<FocusableComponent>();

                foreach (var item in _components)
                {
                    if (item is FocusableComponent fc)
                    {
                        focusableList.Add(fc);
                        fc.Container = this;
                    }
                }
                
                _focusableComponents = focusableList.ToArray();

                CalculateSize();
            }
        }
        public override bool IsFocused
        {
            get => base.IsFocused;
            set
            {
                base.IsFocused = value;

                if (!base.IsFocused)
                    FocusIndex = null;
                else if (FocusIndex == null)
                    FocusIndex = 0;
                
                UpdateFocus();
            }
        }
        protected UIComponent[] _components;
        protected FocusableComponent[] _focusableComponents;
        public int Padding = 5;
        public int? FocusIndex { get; private set; } = null;

        protected bool _disabled;

        protected abstract void CalculateSize();
        
        public override void Update(GameTime gameTime)
        {
            if (_disabled)
                return;

            if (IsFocused)
            {
                _elapsedInputWaitTime += gameTime.GetSeconds();

                if (_elapsedInputWaitTime > _INPUT_WAIT_TIME && DetermineKeyInput())
                {
                    _elapsedInputWaitTime = 0f;
                    return;
                }
            }

            foreach (var item in Components)
            {
                item.Update(gameTime);
            }
        }

        public virtual bool DetermineKeyInput()
        {
            var dir = Input.P1.GetDirectionVector();
            
            if (dir.Y < 0)
            {
                FocusPrevious(true);
                return true;
            }
            else if (dir.Y > 0)
            {
                FocusNext(true);
                return true;
            }

            return false;
        }

        public void FocusPrevious(bool goOutsideContainer = false)
        {
            FocusIndex -= 1;

            if (FocusIndex < 0)
            {
                if (goOutsideContainer && Container != null)
                {
                    Container.FocusPrevious();
                }
                else
                {
                    FocusIndex = _focusableComponents.Length - 1;
                    UpdateFocus();
                }
            }
            else
            {
                UpdateFocus();
            }
        }

        public void FocusNext(bool goOutsideContainer = false)
        {
            FocusIndex += 1;

            if (FocusIndex >= _focusableComponents.Length)
            {
                if (goOutsideContainer && Container != null)
                {
                    Container.FocusPrevious();
                }
                else
                {
                    FocusIndex = 0;
                    UpdateFocus();
                }
            }
            else
            {
                UpdateFocus();
            }
        }

        public void FocusOn(UIComponent component)
        {
            if (Container != null)
                Container.FocusOn(this);

            IsFocused = true;
            FocusIndex = Array.IndexOf(_focusableComponents, component);
            UpdateFocus();
        }

        private void UpdateFocus()
        {
            SFX.Cursor.Play();

            for (int i = 0; i < _focusableComponents.Length; i++)
            {
                _focusableComponents[i].IsFocused = i == FocusIndex;
            }
        }
    }
}