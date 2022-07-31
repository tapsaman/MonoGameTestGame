using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTestGame.Managers;

namespace MonoGameTestGame.Models
{
    public abstract class RenderState : State
    {
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}