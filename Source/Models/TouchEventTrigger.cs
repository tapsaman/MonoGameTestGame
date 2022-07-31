using Microsoft.Xna.Framework;

namespace MonoGameTestGame
{
    public class TouchEventTrigger : MapEntity
    {
        public TouchEventTrigger(Vector2 position, int width, int height)
        {
            Position = position;
            Hitbox.Load(width, height);
            Interactable = false;
            TriggeredOnTouch = true;
        }
    }
}
