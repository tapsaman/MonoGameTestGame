using Microsoft.Xna.Framework;
namespace MonoGameTestGame
{
    public class EventTrigger : MapEntity
    {
        public EventTrigger(Vector2 position, int width, int height)
        {
            Position = position;
            Hitbox.Load(width, height);
            Interactable = true;
        }
    }
}
