using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZA6.Models;
using TapsasEngine.Sprites;

namespace ZA6
{
    public class Moogle : Character
    {
        public Moogle()
        {
            WalkSpeed = 70f;
            var texture = Static.Content.Load<Texture2D>("Sprites/moogle");
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

            AnimatedSprite = new AnimatedSprite(animations, "IdleUp");
            Hitbox.Load (14, 10);
            Hitbox.Color = Color.Red;
            SpriteOffset = new Vector2(-5, -22);
            Interactable = true;
            WalkingStill = false;
            Moving = true;
        }
    }
}
