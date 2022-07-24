using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTestGame.Managers;
using MonoGameTestGame.Models;
using MonoGameTestGame.Sprites;

namespace MonoGameTestGame
{
    public class Guard : Character
    {       
        public Guard()
        {
            var texture = StaticData.Content.Load<Texture2D>("linktothepast/guard-sprites");
            Animation.DefaultFrameWidth = 20;
            Animation.DefaultFrameHeight = 28;

            Dictionary<string, Animation> animations = new Dictionary<string, Animation>()
            {
                { "IdleDown",           new Animation(texture, 1, 0.04f, false, 1, 0) },
                { "IdleUp",             new Animation(texture, 1, 0.04f, false, 7, 0) },
                { "IdleLeft",           new Animation(texture, 1, 0.04f, false, 5, 0) },
                { "IdleRight",          new Animation(texture, 1, 0.04f, false, 3, 0) },
                { "WalkDown",           new Animation(texture, 2, 0.2f,  true,  0, 0) },
                { "WalkUp",             new Animation(texture, 2, 0.2f,  true,  6, 0) },
                { "WalkLeft",           new Animation(texture, 2, 0.2f,  true,  4, 0) },
                { "WalkRight",          new Animation(texture, 2, 0.2f,  true,  2, 0) },
                { "LookAroundDown",     new Animation(texture, 1, 0.2f,  true,  1, 0) },
                { "LookAroundUp",       new Animation(texture, 1, 0.2f,  true,  7, 0) },
                { "LookAroundLeft",     new Animation(texture, 1, 0.2f,  true,  5, 0) },
                { "LookAroundRight",    new Animation(texture, 1, 0.2f,  true,  3, 0) },
                { "IdleDownLookLeft",   new Animation(texture, 1, 0.04f, false, 1, 3) },
                { "IdleDownLookRight",  new Animation(texture, 1, 0.04f, false, 1, 1) },
                { "IdleUpLookLeft",     new Animation(texture, 1, 0.04f, false, 7, 1) },
                { "IdleUpLookRight",    new Animation(texture, 1, 0.04f, false, 7, 3) },
                { "IdleLeftLookUp",     new Animation(texture, 1, 0.04f, false, 5, 3) },
                { "IdleLeftLookDown",   new Animation(texture, 1, 0.04f, false, 5, 1) },
                { "IdleRightLookUp",    new Animation(texture, 1, 0.04f, false, 3, 1) },
                { "IdleRightLookDown",  new Animation(texture, 1, 0.04f, false, 3, 3) },
            };

            Sprite.SetAnimations(animations);
            Hitbox.Load (14, 14);
            Hitbox.Color = Color.Red;
            SpriteOffset = new Vector2(-3, -14);
            WalkSpeed = 30f;
            Trigger += TakeDamage;
            Hittable = true;
            Colliding = true;
            Moving = true;
            Direction = Direction.Down; // TODO make random or take as parameter 
            Dictionary<string, State> states = new Dictionary<string, State>()
            {
                { "Default", new GuardStateDefault(this) }
            };

            StateMachine = new StateMachine(states, "Default");
        }

        private void TakeDamage()
        {
            Sys.Log("triggered");
        }
    }
}
