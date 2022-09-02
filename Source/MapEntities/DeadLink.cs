using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TapsasEngine.Sprites;
using ZA6.Models;

namespace ZA6
{
    public class DeadLink : Character
    {
        public override MapLevel Level => MapLevel.Air;

        public DeadLink()
        {
            Hitbox.Load(16, 10);
            AnimatedSprite = new AnimatedSprite(
                new Dictionary<string, SAnimation>() 
                    {{ "Default", new SAnimation(Img.NPCSprites, 3, 6, 20, 30) }},
                "Default"
            );
            SpriteOffset = new Vector2(-2, -18);
            //Sprite.Color = new Color(.5f, .5f, .5f, .5f);
        }
    }
}