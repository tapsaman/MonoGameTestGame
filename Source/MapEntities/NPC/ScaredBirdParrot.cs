using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ZA6.Models;
using TapsasEngine.Sprites;
using System;

namespace ZA6
{
    public class ScaredBirdParrot : ScaredBird
    {
        public ScaredBirdParrot()
        {
            var texture = Img.NPCSprites;
            var textureY = 3;

            SAnimation.DefaultFrameWidth = 20;
            SAnimation.DefaultFrameHeight = 30;

            Dictionary<string, SAnimation> animations = new Dictionary<string, SAnimation>()
            {
                { "IdleLeft", new SAnimation(texture, 0.15f, Vector2.Zero,
                        new Rectangle[]
                        {
                            new Rectangle(0, textureY * 30, 20, 30),
                            new Rectangle(0, textureY * 30, 20, 30),
                            new Rectangle(0, textureY * 30, 20, 30),
                            new Rectangle(0, textureY * 30, 20, 30),
                            new Rectangle(20, textureY * 30, 20, 30),
                        },
                        isLooping: true
                    )
                },
                { "IdleRight",  new SAnimation(texture, 2, textureY, 20, 30) },
                { "WalkLeft",   new SAnimation(texture, 2, 0.2f, true, textureY, 3) },
                { "WalkRight",  new SAnimation(texture, 2, 0.2f, true, textureY, 5) },
                { "WalkUp",     new SAnimation(texture, 2, 0.2f, true, textureY, 5) }
            };

            AnimatedSprite = new AnimatedSprite(animations, "IdleLeft");
            Hitbox.Load(1, 1);
            Hitbox.Color = Color.Red;
            SpriteOffset = new Vector2(-12, -22);
            Moving = true;
            DrawingShadow = false;
        }
    }
}
