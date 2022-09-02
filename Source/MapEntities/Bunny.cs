using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZA6.Models;
using TapsasEngine.Sprites;
using System;

namespace ZA6
{
    public class Bunny : Character
    {
        public override MapLevel Level { get => MapLevel.Character; }
        public bool RunningOff = false;

        public Bunny()
        {
            var texture = Img.NPCSprites;

            SAnimation.DefaultFrameWidth = 20;
            SAnimation.DefaultFrameHeight = 30;

            Dictionary<string, SAnimation> animations = new Dictionary<string, SAnimation>()
            {
                { "IdleLeft",   new SAnimation(texture, 0, 2, 20, 30) },
                { "IdleRight",   new SAnimation(texture, 3, 2, 20, 30) },
                { "WalkLeft",   new SAnimation(texture, 2, 0.1f , true, 2, 1) },
                { "WalkRight",  new SAnimation(texture, 2, 0.1f , true, 2, 4) },
                { "WalkDown",   new SAnimation(texture, 2, 0.1f , true, 2, 4) }
            };

            AnimatedSprite = new AnimatedSprite(animations, "IdleLeft");
            Hitbox.Load(1, 1);
            Hitbox.Color = Color.Red;
            SpriteOffset = new Vector2(-14, -23);
            DrawingShadow = false;
            Moving = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (RunningOff)
            {
                Velocity = new Vector2(5, 50);
            }

            base.Update(gameTime);
        }
    }
}
