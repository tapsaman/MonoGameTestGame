using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTestGame.Models;
using MonoGameTestGame.Sprites;

namespace MonoGameTestGame
{
    public class Moogle : Character
    {       
        private Event[] _talkEvent;
        public Moogle()
        {
            var texture = Static.Content.Load<Texture2D>("moogle2");
            SAnimation.DefaultFrameWidth = 24;
            SAnimation.DefaultFrameHeight = 32;

            Dictionary<string, SAnimation> animations = new Dictionary<string, SAnimation>()
            {
                { "IdleUp",         new SAnimation(texture, 1, 0.04f, false, 0, 1 ) },
                { "IdleRight",      new SAnimation(texture, 1, 0.04f, false, 1, 1 ) },
                { "IdleDown",       new SAnimation(texture, 1, 0.04f, false, 2, 1 ) },
                { "IdleLeft",       new SAnimation(texture, 1, 0.04f, false, 3, 1 ) },
                { "WalkUp",         new SAnimation(texture, 3, 0.04f, true, 0) },
                { "WalkRight",      new SAnimation(texture, 3, 0.04f, true, 1) },
                { "WalkDown",       new SAnimation(texture, 3, 0.04f, true, 2) },
                { "WalkLeft",       new SAnimation(texture, 3, 0.04f, true, 3) }
            };

            Sprite.SetAnimations(animations);
            Hitbox.Load (14, 10);
            Hitbox.Color = Color.Red;
            SpriteOffset = new Vector2(-3, -18);
            Trigger += TalkTo;
            Interactable = true;
            Colliding = true;
            WalkingStill = false;
            Moving = true;

            _talkEvent = new Event[]
            {
                new ConditionEvent(EventStore.Scene, "moogle speak toggle")
                {
                    IfFalse = new Event[]
                    {
                        new TextEvent(new Dialog("Juat get a... get to the\notherside my guy."), this),
                        new SaveValueEvent(EventStore.Scene, "moogle speak toggle", true)
                    },
                    IfTrue = new Event[]
                    {
                        new TextEvent(new Dialog("Are you ok"), this),
                        new SaveValueEvent(EventStore.Scene, "moogle speak toggle", false)
                    }
                }
            };
        }

        private void TalkTo(Character _)
        {
            Static.EventSystem.Load(_talkEvent);
        }
    }
}
