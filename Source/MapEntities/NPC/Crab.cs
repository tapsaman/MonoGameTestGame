using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZA6.Managers;
using ZA6.Models;
using TapsasEngine.Sprites;

namespace ZA6
{
    public class Crab : Character
    {
        public Crab()
        {
            Moving = true;
            
            var texture = Img.EnemySprites;
            SAnimation.DefaultFrameWidth = SAnimation.DefaultFrameHeight = 34;

            Dictionary<string, SAnimation> animations = new Dictionary<string, SAnimation>()
            {
                { "Default", new SAnimation(texture, 2, 0.3f, true, 6, 3) }
            };

            AnimatedSprite = new AnimatedSprite(animations, "Default");
            Hitbox.Load(1, 1);
            SpriteOffset = new Vector2(-17, -25);
        }
    }
}
