using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameTestGame.Controls;
using MonoGameTestGame.Managers;
using MonoGameTestGame.Models;
using MonoGameTestGame.Sprites;

namespace MonoGameTestGame
{
    public class Enemy : MapEntity
    {       
        public Enemy(Vector2 position)
            : base(position)
        {
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

            Sprite.SetAnimations(animations, "WalkRight");
            Hitbox.Load (18, 22);
            Hitbox.Color = Color.Red;
            SpriteOffset = new Vector2(-2, -2);
            Trigger += TriggerTest;
            Interactable = true;
            Hittable = false;
            Colliding = true;
        }

        private void TriggerTest()
        {
            Sys.Log("triggered");
        }
    }
}
