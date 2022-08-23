using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TapsasEngine.Utilities;

namespace ZA6.UI
{
    public abstract class UIContainer : FocusableComponent
    {
        private const float _INPUT_WAIT_TIME = 0.2f;
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
            }
        }
        protected UIComponent[] _components;
        protected FocusableComponent[] _focusableComponents;
        public int Padding = 5;
        public int? FocusIndex
        {
            get => _focusIndex;
            private set
            {
                if (_focusIndex != value)
                {
                    _focusIndex = value;

                    for (int i = 0; i < _focusableComponents.Length; i++)
                    {
                        _focusableComponents[i].IsFocused = i == _focusIndex;
                    }
                }
            }
        }

        protected bool _disabled;
        private int? _focusIndex = null;

        protected abstract void CalculateSize();
        
        public override void Update(GameTime gameTime)
        {
            if (_disabled)
                return;

            if (IsFocused)
            {
                _elapsedInputWaitTime += gameTime.GetSeconds();

                if (_elapsedInputWaitTime > _INPUT_WAIT_TIME)
                {
                    if (DetermineKeyInput())
                    {
                        _elapsedInputWaitTime = 0f;
                        return;
                    }
                    else
                    {
                        _elapsedInputWaitTime = _INPUT_WAIT_TIME;
                    }
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
            if (FocusIndex > 0)
            {
                FocusIndex -= 1;
            }
            else
            {
                if (goOutsideContainer && Container != null)
                {
                    Container.FocusPrevious();
                }
                else
                {
                    FocusIndex = _focusableComponents.Length - 1;
                }
            }
        }

        public void FocusNext(bool goOutsideContainer = false)
        {
            if (FocusIndex < _focusableComponents.Length - 1)
            {
                FocusIndex += 1;
            }
            else
            {
                if (goOutsideContainer && Container != null)
                {
                    Container.FocusPrevious();
                }
                else
                {
                    FocusIndex = 0;
                }
            }
        }

        public void FocusOn(UIComponent component)
        {
            if (Container != null)
                Container.FocusOn(this);

            if (!IsFocused)
                IsFocused = true;
            
            FocusIndex = Array.IndexOf(_focusableComponents, component);
        }
    }
}