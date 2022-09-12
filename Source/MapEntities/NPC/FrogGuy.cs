using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZA6.Managers;
using ZA6.Models;
using TapsasEngine.Sprites;
using TapsasEngine;

namespace ZA6
{
    public class FrogGuy : Character
    {
        public FrogGuy()
        {
            Moving = true;
            WalkingStill = true;
            WalkSpeed = 120f;
            Interactable = true;
            Trigger += TalkTo;
            
            var texture = Img.NPCSprites;
            SAnimation.DefaultFrameWidth = 20;
            SAnimation.DefaultFrameHeight = 30;

            Dictionary<string, SAnimation> animations = new Dictionary<string, SAnimation>()
            {
                { "WalkDown", new SAnimation(texture, 3, 0.04f, true, 10, 0) },
                { "WalkUp", new SAnimation(texture, 2, 0.04f, true, 9, 0) },
                { "WalkRight", new SAnimation(texture, 2, 0.04f, true, 9, 2) },
                { "WalkLeft", new SAnimation(texture, 2, 0.04f, true, 9, 4) },
            };

            AnimatedSprite = new AnimatedSprite(animations, "WalkDown");
            Hitbox.Load(10, 10);
            SpriteOffset = new Vector2(-6, -20);
        }

        private void TalkTo(Character _)
        {
            Static.EventSystem.Load(new Event[]
            {
                new FaceEvent(this, Static.Player),
                new TextEvent(new Dialog("OMG! OMG!!! It's... the\n\f02T H E B O N E R A T T L E R S\f00 !!!!"), this)
            });
        }
    }
}
