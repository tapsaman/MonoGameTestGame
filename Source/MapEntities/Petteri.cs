using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TapsasEngine.Enums;
using ZA6.Models;
using TapsasEngine.Sprites;

namespace ZA6
{
    public class Petteri : Character
    {       
        public Petteri()
        {
            var texture = Static.Content.Load<Texture2D>("linktothepast/link-sprites-red");
            SAnimation.DefaultFrameWidth = 40;
            SAnimation.DefaultFrameHeight = 50;

            Dictionary<string, SAnimation> animations = new Dictionary<string, SAnimation>()
            {
                { "IdleDown",       new SAnimation(texture, 0, 1) },
                { "IdleUp",         new SAnimation(texture, 1, 3) },
                { "IdleLeft",       new SAnimation(texture, 2, 3) },
                { "IdleRight",      new SAnimation(texture, 3, 3) },
                { "WalkDown",       new SAnimation(texture, 8, 0.04f, true, 0) },
                { "WalkUp",         new SAnimation(texture, 8, 0.04f, true, 1) },
                { "WalkLeft",       new SAnimation(texture, 6, 0.04f, true, 2) },
                { "WalkRight",      new SAnimation(texture, 6, 0.04f, true, 3) },
                { "SwordHitDown",   new SAnimation(texture, 6, 0.04f, false, 4, 0) },
                { "SwordHitUp",     new SAnimation(texture, 5, 0.04f, false, 5, 0) },
                { "SwordHitLeft",   new SAnimation(texture, 5, 0.04f, false, 6, 0) },
                { "SwordHitRight",  new SAnimation(texture, 5, 0.04f, false, 7, 0) },
            };

            AnimatedSprite = new AnimatedSprite(animations, "IdleDown");
            Hitbox.Load(14, 14);
            Hitbox.Color = Color.Red;
            SpriteOffset = new Vector2(-13, -24);
            Interactable = true;
            Hittable = false;
            WalkingStill = true;
            Facing = Direction.Down;
        }
    }
}
