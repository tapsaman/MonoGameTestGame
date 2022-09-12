using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TapsasEngine.Enums;
using ZA6.Models;
using TapsasEngine.Sprites;

namespace ZA6
{
    public class VoidSeppo : Character
    {       
        public override MapLevel Level { get => MapLevel.Character; }

        public VoidSeppo()
        {
            var texture = Img.NPCSprites;
            SAnimation.DefaultFrameWidth = 20;
            SAnimation.DefaultFrameHeight = 30;

            Dictionary<string, SAnimation> animations = new Dictionary<string, SAnimation>()
            {
                { "Idle",           new SAnimation(texture, 0, 8) },
                { "Laughing",       new SAnimation(texture, 2, 0.04f, true, 8, 0) },
            };

            AnimatedSprite = new AnimatedSprite(animations, "Laughing");
            Hitbox.Load(16, 16);
            SpriteOffset = new Vector2(-1, -12);
            Interactable = true;
        }
    }
}
