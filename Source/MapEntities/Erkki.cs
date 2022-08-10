using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZA6.Models;
using ZA6.Sprites;

namespace ZA6
{
    public class Erkki : Character
    {       
        public Erkki()
        {
            var texture = Static.Content.Load<Texture2D>("linktothepast/npc-sprites");
            SAnimation.DefaultFrameWidth = 20;
            SAnimation.DefaultFrameHeight = 30;

            Dictionary<string, SAnimation> animations = new Dictionary<string, SAnimation>()
            {
                { "IdleDown",       new SAnimation(texture, 1, 0.04f, false, 0, 3) },
                { "IdleUp",         new SAnimation(texture, 1, 0.04f, false, 0, 2) },
                { "IdleLeft",       new SAnimation(texture, 1, 0.04f, false, 0, 1) },
                { "IdleRight",      new SAnimation(texture, 1, 0.04f, false, 0, 0) },
            };

            Sprite.SetAnimations(animations);
            Hitbox.Load (14, 10);
            Hitbox.Color = Color.Red;
            SpriteOffset = new Vector2(-3, -18);
            Trigger += TriggerTest;
            Interactable = true;
            Hittable = false;
            Colliding = true;
            WalkingStill = false;
            Direction = Direction.Left;
        }

        private void TriggerTest(Character _)
        {
            Sys.Log("triggered");
        }
    }
}
