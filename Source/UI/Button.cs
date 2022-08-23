using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using TapsasEngine.Sprites;

namespace ZA6.UI
{
    public class Button : FocusableComponent
    {
        #region Fields
        protected Sprite _background;
        protected SpriteFont _font;
        protected bool _isHovering;
        protected bool _isActive;
        #endregion

        #region Properties
        public Vector2? TextMargin;
        public static SoundEffect ClickSound;
        public event EventHandler Click;
        public override bool IsFocused
        {
            get => base.IsFocused;
            set
            {
                if (!base.IsFocused && value)
                {
                    SFX.Cursor.Play();
                }

                base.IsFocused = value;
            }
        }
        public Color PenColor = Color.Black;
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
            }
        }
        public string Text;
        #endregion

        #region Methods

        public Button(SpriteFont font, EventHandler onClick = null)
            : this(DefaultBackground, font, onClick) {}

        public Button(Sprite background, SpriteFont font, EventHandler onClick = null)
        {
            _background = background;
            _font = font;
            Click += onClick;
            
            // Default size
            Width = background.Texture.Width;
            Height = background.Texture.Height;
        }

        public override void Update(GameTime gameTime)
        {
            _isActive = false;
            _isHovering = Input.P1.GetMouseRectangle().Intersects(Rectangle);

            if (!IsFocused)
            {
                if (_isHovering && Input.P1.IsMouseMoving())
                {
                    Container.FocusOn(this);
                }
            }
            else if (!Disabled)
            {
                if (_isHovering && Input.P1.IsMouseLeftPressed())
                {
                    _isActive = true;
                }
                else if (_isHovering && Input.P1.JustReleasedMouseLeft())
                {
                    DoClick();
                }
                else if (Input.P1.IsPressed(Input.P1.A) || Input.P1.IsPressed(Input.P1.Start))
                {
                    _isActive = true;
                }
                else if (Input.P1.JustReleased(Input.P1.A) || Input.P1.JustReleased(Input.P1.Start))
                {
                    DoClick();
                }
            }
        }

        public void DoClick()
        {
            if (ClickSound != null)
                ClickSound.Play();
                    
            Click?.Invoke(this, new EventArgs());
        }
    
        public override void Draw(SpriteBatch spriteBatch)
        {
            var color = Color.White;
            var penColor = PenColor;
            
            if (_isActive)
                color = ActiveColor;
            else if (IsFocused)
                color = Color.Gray;

            if (Disabled)
            {
                penColor = new Color(127, 127, 127);
                DefaultDisabledBackground.Color = color;
                DefaultDisabledBackground.Draw(spriteBatch, Position, Width, Height);
            }
            else
            {
                _background.Color = color;
                _background.Draw(spriteBatch, Position, Width, Height);
            }

            if (!string.IsNullOrEmpty(Text)) {
                Vector2 textPosition;
                
                if (TextMargin != null)
                {
                    textPosition = Position + (Vector2)TextMargin;
                    if (_isActive) textPosition.Y += 1;
                }
                else
                {
                    var textSize = _font.MeasureString(Text);
                    textPosition = new Vector2(
                        (Rectangle.X + (Rectangle.Width / 2)) - (textSize.X / 2),
                        (Rectangle.Y + (Rectangle.Height / 2)) - (textSize.Y / 2) + (_isActive ? 1 : 0)
                    );
                }

                spriteBatch.DrawString(_font, Text, textPosition, penColor);
            }
        }

        #endregion
    }
}