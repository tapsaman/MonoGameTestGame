using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TapsasEngine;
using TapsasEngine.Sprites;

namespace ZA6.UI
{
    public abstract class UIComponent : IUpdate, IDraw
    {
        public static SectionedSprite DefaultBackground;
        public static SectionedSprite DefaultDisabledBackground;
        public static Color ActiveColor = new Color(207, 55, 55);
        public UIContainer Container;
        //public bool Focusable { get; protected set; } = true;
        public int Width { get; set; }
        public int Height { get; set; }
        public Vector2 Position;
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}