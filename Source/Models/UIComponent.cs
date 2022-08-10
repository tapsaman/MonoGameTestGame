using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZA6
{
    public abstract class UIComponent
    {
        public static Color ActiveColor = new Color(207, 55, 55);
        public bool Focusable { get; protected set; } = true;
        public bool Disabled { get; set; }
        public bool? IsFocused { get; set; } = null;
        public abstract int Width { get; }
        public abstract int Height { get; }
        public Vector2 Position;
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
    } 
}