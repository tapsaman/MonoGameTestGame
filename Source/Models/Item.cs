using ZA6;
using TapsasEngine.Sprites;

namespace TapsasEngine.Models
{
    public abstract class Item : MapObject
    {
        public override MapLevel Level { get => MapLevel.Air; }

        public string PickUpText { get; protected set; }

        public virtual void OnPickUp() {}
    }
}