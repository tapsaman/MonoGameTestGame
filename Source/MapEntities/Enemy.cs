using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTestGame.Models;
using MonoGameTestGame.Sprites;

namespace MonoGameTestGame
{
    public class Enemy : Character
    {       
        public Enemy()
        {
            /*
            var texture = StaticData.Content.Load<Texture2D>("linktothepast-spritesheet-og");
            Animation.DefaultFrameWidth = Animation.DefaultFrameHeight = 30;

            Dictionary<string, Animation> animations = new Dictionary<string, Animation>()
            {
                { "IdleUp", new Animation(texture, 1, 4, 3) },
                { "IdleRight", new Animation(texture, 1, 4, 12) },
                { "IdleDown", new Animation(texture, 1, 0, 1) },
                { "IdleLeft", new Animation(texture, 1, 1, 12) },
                { "WalkUp", new Animation(texture, 8, 4) },
                { "WalkRight", new Animation(texture, 6, 4, 8) },
                { "WalkDown", new Animation(texture, 8, 1) },
                { "WalkLeft", new Animation(texture, 6, 1, 8) },
                { "SwordHitUp", new Animation(texture, 5, 6, 0, false, 0.04f) },
                { "SwordHitRight", new Animation(texture, 5, 6, 8, false, 0.04f) },
                { "SwordHitDown", new Animation(texture, 6, 3, 0, false, 0.04f) },
                { "SwordHitLeft", new Animation(texture, 5, 3, 8, false, 0.04f) },
            };
            SpriteOffset = new Vector2(-2, -2);
            */
            var texture = StaticData.Content.Load<Texture2D>("linktothepast-spritesheet-red");

            Dictionary<string, Animation> animations = new Dictionary<string, Animation>()
            {
                { "IdleDown",       new Animation(texture, 1, 0, 1) },
                { "IdleUp",         new Animation(texture, 1, 1, 3) },
                { "IdleLeft",       new Animation(texture, 1, 2, 3) },
                { "IdleRight",      new Animation(texture, 1, 3, 3) },
                { "WalkDown",       new Animation(texture, 8, 0) },
                { "WalkUp",         new Animation(texture, 8, 1) },
                { "WalkLeft",       new Animation(texture, 6, 2) },
                { "WalkRight",      new Animation(texture, 6, 3) },
                { "SwordHitDown",   new Animation(texture, 6, 4, 0, false, 0.04f) },
                { "SwordHitUp",     new Animation(texture, 5, 5, 0, false, 0.04f) },
                { "SwordHitLeft",   new Animation(texture, 5, 6, 0, false, 0.04f) },
                { "SwordHitRight",  new Animation(texture, 5, 7, 0, false, 0.04f) },
            };

            Sprite.SetAnimations(animations, "WalkRight");
            Hitbox.Load (14, 14);
            Hitbox.Color = Color.Red;
            SpriteOffset = new Vector2(-13, -24);
            Trigger += TriggerTest;
            Interactable = true;
            Hittable = false;
            Colliding = true;
            WalkingStill = true;
            Direction = Direction.Left;
        }

        private void TriggerTest()
        {
            Sys.Log("triggered");
        }
    }
}
