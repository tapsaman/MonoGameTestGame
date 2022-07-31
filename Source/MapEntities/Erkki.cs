using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTestGame.Models;
using MonoGameTestGame.Sprites;

namespace MonoGameTestGame
{
    public class Erkki : Character
    {       
        public Erkki()
        {
            var texture = StaticData.Content.Load<Texture2D>("linktothepast/npc-sprites");
            Animation.DefaultFrameWidth = 20;
            Animation.DefaultFrameHeight = 30;

            Dictionary<string, Animation> animations = new Dictionary<string, Animation>()
            {
                { "IdleDown",       new Animation(texture, 1, 0.04f, false, 0, 3) },
                { "IdleUp",         new Animation(texture, 1, 0.04f, false, 0, 2) },
                { "IdleLeft",       new Animation(texture, 1, 0.04f, false, 0, 1) },
                { "IdleRight",      new Animation(texture, 1, 0.04f, false, 0, 0) },
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
