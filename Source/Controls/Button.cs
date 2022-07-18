using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameTestGame.Controls
{
    public class Button : Component
    {
        #region Fields
        private MouseState _currentMouse;
        private MouseState _previousMouse;
        private SpriteFont _font;
        private bool _isHovering;
        private Texture2D _texture;
        #endregion

        #region Properties
        public event EventHandler Click;
        public bool Clicked { get; private set; }
        public Color PenColor = Color.Black;
        public Vector2 Position;
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
            }
        }
        public string Text;
        #endregion

        #region Methods
        public Button(Texture2D texture, SpriteFont font)
        {
            _texture = texture;
            _font = font;
            Position = new Vector2(StaticData.NativeWidth / 2 - _texture.Width / 2, StaticData.NativeHeight / 2 - _texture.Height / 2);
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var color = Color.White;
            
            if (_isHovering)
                color = Color.Gray;

            spriteBatch.Draw(_texture, Rectangle, color);

            if (!string.IsNullOrEmpty(Text)) {
                var x = (Rectangle.X + (Rectangle.Width / 2)) - (_font.MeasureString(Text).X / 2);
                var y = (Rectangle.Y + (Rectangle.Height / 2)) - (_font.MeasureString(Text).Y / 2);

                spriteBatch.DrawString(_font, Text, new Vector2(x, y), PenColor);
            }
        }

        public override void Update(GameTime gameTime)
        {
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();

            var mouseRectangle = new Rectangle(_currentMouse.X / 3, _currentMouse.Y / 3, 1, 1);

            _isHovering = false;

            if (mouseRectangle.Intersects(Rectangle)) {
                _isHovering = true;

                if (_currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed) {
                    Click?.Invoke(this, new EventArgs());
                }
            }
        }
        #endregion
    }
}