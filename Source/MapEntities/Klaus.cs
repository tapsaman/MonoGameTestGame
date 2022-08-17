using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZA6.Models;
using ZA6.Sprites;

namespace ZA6
{
    public class Klaus : Character
    {       
        public Klaus()
        {
            var texture = Static.Content.Load<Texture2D>("linktothepast/npc-sprites");

            Dictionary<string, SAnimation> animations = new Dictionary<string, SAnimation>()
            {
                { "IdleLeft",       new SAnimation(texture, 4, 1, 20, 30) },
                { "IdleDown",       new SAnimation(texture, 5, 1, 20, 30) },
                { "IdleRight",       new SAnimation(texture, 6, 1, 20, 30) }
            };

            Sprite.SetAnimations(animations, "IdleDown");
            Hitbox.Load(16, 16);
            SpriteOffset = new Vector2(-3, -12);
            Interactable = true;
            Hittable = false;
            Colliding = true;
            Direction = Direction.Down;
            Trigger += TalkTo;
            Moving = true;
        }

        private void TalkTo(Character _)
        {
            Static.EventSystem.Load(new Event[]
            {
                new FaceEvent(this, Static.Player),
                new TextEvent(new Dialog("This is the pain room"), this),
                new AnimateEvent(new Animations.FadeSprite(Sprite)),
                new RemoveEvent(this),
                new SaveValueEvent(DataStoreType.Session, "spoken to klaus", true),
            });
        }
    }
}
