using Microsoft.Xna.Framework;

namespace MonoGameTestGame
{
    public class Bush : MapEntity
    {
        public Bush(Vector2 position)
        {
            Hitbox.Load(14, 14);
        }
    }
}