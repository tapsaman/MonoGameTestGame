using ZA6;
using ZA6.Sprites;

namespace TapsasEngine.Models
{
    public abstract class Item : MapObject
    {
        public string PickUpText { get; protected set; }

        public Item()
        {
            Colliding = false;
        }

        public virtual void OnPickUp() {}
    }
}