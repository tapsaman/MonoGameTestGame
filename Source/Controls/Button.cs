using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ZA6.Controls
{
    public class Button : UIComponent
    {
        #region Fields
        protected SpriteFont _font;
        protected bool _isHovering;
        protected bool _isActive;
        protected Texture2D _texture;
        #endregion

        #region Properties
        public static SoundEffect ClickSound;
        public event EventHandler Click;
        public bool Clicked { get; private set; }
        public Color PenColor = Color.Black;
        public override int Width { get { return _texture.Width; } }
        public override int Height { get { return _texture.Height; } }
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
        }

        public override void Update(GameTime gameTime)
        {
            if (Disabled)
                return;

            _isActive = false;
            _isHovering = Input.P1.GetMouseRectangle().Intersects(Rectangle);

            if (_isHovering)
            {
                if (Input.P1.IsMouseLeftPressed())
                {
                    _isActive = true;
                }
                else if (Input.P1.JustReleasedMouseLeft())
                {
                    if (ClickSound != null) ClickSound.Play();

                    Click?.Invoke(this, new EventArgs());
                    return;
                }
            }

            if (IsFocused == true)
            {
                if (Input.P1.IsPressed(Input.P1.A) || Input.P1.IsPressed(Input.P1.Start))
                {
                    _isActive = true;
                }
                else if (Input.P1.JustReleased(Input.P1.A) || Input.P1.JustReleased(Input.P1.Start))
                {
                    if (ClickSound != null) ClickSound.Play();
                    
                    Click?.Invoke(this, new EventArgs());
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

            if (!string.IsNullOrEmpty(Text)) {
                var x = (Rectangle.X + (Rectangle.Width / 2)) - (_font.MeasureString(Text).X / 2);
                var y = (Rectangle.Y + (Rectangle.Height / 2)) - (_font.MeasureString(Text).Y / 2);

                spriteBatch.DrawString(_font, Text, new Vector2(x, y), PenColor);
            }
        }

        #endregion
    }
}