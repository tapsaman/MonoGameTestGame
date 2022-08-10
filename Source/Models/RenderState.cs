using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZA6.Managers;

namespace ZA6.Models
{
    public abstract class RenderState : State
    {
        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);
    }
}