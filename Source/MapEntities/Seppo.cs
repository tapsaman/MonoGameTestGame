using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTestGame.Models;
using MonoGameTestGame.Sprites;

namespace MonoGameTestGame
{
    public class Seppo : Character
    {       
        public Seppo()
        {
            var texture = StaticData.Content.Load<Texture2D>("linktothepast/npc-sprites");
            Animation.DefaultFrameWidth = 20;
            Animation.DefaultFrameHeight = 30;

            Dictionary<string, Animation> animations = new Dictionary<string, Animation>()
            {
                { "IdleLeft",       new Animation(texture, 2, 0.5f, true, 1, 0) },
                { "IdleDown",       new Animation(texture, 1, 0.5f, false, 1, 2) }
            };

            Sprite.SetAnimations(animations);
            Hitbox.Load(16, 16);
            SpriteOffset = new Vector2(-1, -12);
            Interactable = false;
            Hittable = false;
            Colliding = false;
            WalkingStill = false;
            Moving = false;
            Direction = Direction.Left;
        }
    }
}
