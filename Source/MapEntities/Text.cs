using ZA6.Models;

namespace ZA6
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

        private void Read(Character _)
        {
            Static.EventSystem.Load(_readEvent);
        }
    }
}
