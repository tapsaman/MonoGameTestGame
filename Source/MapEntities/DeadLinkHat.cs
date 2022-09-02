using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TapsasEngine.Sprites;
using ZA6.Models;

namespace ZA6
{
    public class DeadLinkHat : Character
    {
        public override MapLevel Level => MapLevel.Air;

        public DeadLinkHat()
        {
            Hitbox.Load(20, 14);
            SAnimation.DefaultFrameWidth = 20;
            SAnimation.DefaultFrameHeight = 15;

            var animations = new Dictionary<string, SAnimation>()
            {
                { "Falling", new SAnimation(Img.NPCSprites, 2, 0.1f, true, 12, 4) },
                { "Dropped", new SAnimation(Img.NPCSprites, 2, 0.1f, false, 13, 4) }
            };

            AnimatedSprite = new AnimatedSprite(animations, "Falling");
            DrawingShadow = false;
        }
    }
}