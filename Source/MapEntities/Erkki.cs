using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TapsasEngine.Enums;
using ZA6.Models;
using TapsasEngine.Sprites;

namespace ZA6
{
    public class Erkki : Character
    {
        public Erkki()
        {
            var texture = Img.NPCSprites;
            SAnimation.DefaultFrameWidth = 20;
            SAnimation.DefaultFrameHeight = 30;

            Dictionary<string, SAnimation> animations = new Dictionary<string, SAnimation>()
            {
                { "IdleDown",       new SAnimation(texture, 1, 0.04f, false, 0, 3) },
                { "IdleUp",         new SAnimation(texture, 1, 0.04f, false, 0, 2) },
                { "IdleLeft",       new SAnimation(texture, 1, 0.04f, false, 0, 1) },
                { "IdleRight",      new SAnimation(texture, 1, 0.04f, false, 0, 0) },
            };

            AnimatedSprite = new AnimatedSprite(animations, "IdleLeft");
            Facing = Direction.Left;
            Hitbox.Load (14, 10);
            Hitbox.Color = Color.Red;
            SpriteOffset = new Vector2(-3, -18);
            Interactable = true;
            Hittable = false;
            WalkingStill = false;
        }
    }
}
