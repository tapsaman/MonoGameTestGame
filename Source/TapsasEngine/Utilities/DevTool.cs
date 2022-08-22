using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ZA6;

namespace TapsasEngine.Utilities
{
    public abstract class DevTool : IUpdate, IDraw
    {
        public bool Enabled = true;
        public Color TextColor = new Color(255, 255, 255);
        public SpriteFont Font = Static.Font;
        private string _message = null;
        private float _elapsedMessageTime;
        protected float _messageTime = 3f;
        public DevToolAction[] Actions;

        public virtual void Update(GameTime gameTime)
        {
            if (_message != null)
            {
                _elapsedMessageTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (_elapsedMessageTime > _messageTime)
                {
                    _message = null;
                }
            }

            if (Input.P1.JustPressed(Keys.F1))
            {
                Reset();
                DescribeDevTools();
            }

            foreach (var item in Actions)
            {
                if (Input.P1.JustPressed(item.Key))
                {
                    Reset();
                    SetMessage("(" + item.Key + ") " + item.Action.Invoke());
                    break;
                }
            }
        }

        public virtual void Reset() {}

        private void DescribeDevTools()
        {
            string txt = "Dev tools\n(F1) Show descriptions";

            foreach (var item in Actions)
            {
                txt += "\n(" + item.Key + ") " + item.Description;
            }

            SetMessage(txt);
        }

        public void SetMessage(string message)
        {
            _message = message;
            _elapsedMessageTime = 0;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (_message != null)
            {
                var size = Font.MeasureString(_message);

                spriteBatch.DrawString(
                    Font,
                    _message,
                    new Vector2(1, Static.Renderer.DeviceManager.PreferredBackBufferHeight - size.Y),
                    TextColor
                );
            }
        }
    }

    public class DevToolAction
    {
        public Keys Key;
        public string Description;
        public Func<string> Action;

        public DevToolAction(Keys key, string description, Func<string> action)
        {
            Key = key;
            Description = description;
            Action = action;
        }
    }
}