using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameTestGame
{
    public abstract class UIComponent
    {
        public abstract int Width { get; }
        public abstract int Height { get; }
        public Vector2 Position;
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
    } 
}