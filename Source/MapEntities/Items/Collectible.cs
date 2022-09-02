using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace ZA6
{
    public abstract class Collectible : MapObject
    {
        public override MapLevel Level { get => MapLevel.Ground; }
        public SoundEffect PickUpSound { get; protected set; } = SFX.Heart; 

        public Collectible()
        {
            Hitbox.Color = Color.Orange;
            Interactable = false;
            TriggeredOnTouch = true;
            Trigger += OnCollect;
        }

        public void OnCollect(Character character)
        {
            if (character == Static.Player)
            {
                Collect();
            }
        }

        public virtual void Collect()
        {
            PickUpSound.Play();
            Static.Scene.Remove(this);
        }
    }
}
