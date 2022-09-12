using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TapsasEngine.Enums;
using ZA6.Models;
using TapsasEngine.Sprites;

namespace ZA6
{
    public class Taalasmaa : Character
    {       
        public override MapLevel Level { get => MapLevel.Air; }

        public Taalasmaa()
        {
            var texture = Static.Content.Load<Texture2D>("Sprites/taalasmaa-146x135");
            SAnimation.DefaultFrameWidth = 146;
            SAnimation.DefaultFrameHeight = 135;

            Dictionary<string, SAnimation> animations = new Dictionary<string, SAnimation>()
            {
                { "Idle",           new SAnimation(texture, 0, 0) },
                { "Talk",           new SAnimation(texture, 2, 0.1f, true, 0, 1) },
                { "AngryIdle",      new SAnimation(texture, 0, 1) },
                { "AngryTalk",      new SAnimation(texture, 2, 0.1f, true, 1, 1) },
                { "AngryOpenMouth", new SAnimation(texture, 2, 1) }
            };

            AnimatedSprite = new AnimatedSprite(animations, "Idle");
            Hitbox.Load(16, 16);
            SpriteOffset = new Vector2(0, 0);
            Moving = false;
            DrawingShadow = false;
        }
    }
}
