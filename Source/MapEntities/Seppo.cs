using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TapsasEngine.Enums;
using ZA6.Models;
using ZA6.Sprites;

namespace ZA6
{
    public class Seppo : Character
    {       
        public Seppo()
        {
            var texture = Static.Content.Load<Texture2D>("linktothepast/npc-sprites");
            SAnimation.DefaultFrameWidth = 20;
            SAnimation.DefaultFrameHeight = 30;

            Dictionary<string, SAnimation> animations = new Dictionary<string, SAnimation>()
            {
                { "IdleLeft",       new SAnimation(texture, 2, 0.5f, true, 1, 0) },
                { "IdleDown",       new SAnimation(texture, 1, 0.5f, false, 1, 2) }
            };

            Sprite.SetAnimations(animations, "IdleLeft");
            Hitbox.Load(16, 16);
            SpriteOffset = new Vector2(-1, -12);
            Interactable = false;
            Hittable = false;
            Colliding = false;
            WalkingStill = false;
            Moving = false;
            Facing = Direction.Left;
        }
    }
}
