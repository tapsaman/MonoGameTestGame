using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZA6.Managers;
using ZA6.Models;
using TapsasEngine.Sprites;

namespace ZA6
{
    public class BigBird : Character
    {
        public BigBird()
        {
            Moving = true;
            
            var texture = Img.EnemySprites;
            SAnimation.DefaultFrameWidth = SAnimation.DefaultFrameHeight = 34;

            Dictionary<string, SAnimation> animations = new Dictionary<string, SAnimation>()
            {
                { "Default", new SAnimation(texture, 0.3f, Vector2.Zero,
                    new Rectangle[]
                    {
                        new Rectangle(102, 68, 34, 34),
                        new Rectangle(136, 68, 34, 34),
                        new Rectangle(102, 68, 34, 34),
                        new Rectangle(136, 68, 34, 34),

                        new Rectangle(102, 102, 34, 34),
                        new Rectangle(136, 102, 34, 34),
                        new Rectangle(102, 102, 34, 34),
                        new Rectangle(136, 102, 34, 34),
                    },
                    isLooping: true
                )}
            };

            AnimatedSprite = new AnimatedSprite(animations, "Default");
            Hitbox.Load(1, 1);
            SpriteOffset = new Vector2(-18, -33);
        }
    }
}
