using MonoGameTestGame.Models;

namespace MonoGameTestGame
{
    public class Text : MapEntity
    {
        public string Message
        {
            set { _readEvent = new TextEvent(new Dialog(value), this); }
        }
        private TextEvent _readEvent;

        public Text()
        {
            Hitbox.Load(16, 16);
            Interactable = true;
            Trigger += Read;
        }

        private void Read()
        {
            StaticData.Scene.EventManager.Load(_readEvent);
        }
    }
}
