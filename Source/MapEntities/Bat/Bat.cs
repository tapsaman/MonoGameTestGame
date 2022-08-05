using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTestGame.Managers;
using MonoGameTestGame.Models;
using MonoGameTestGame.Sprites;

namespace MonoGameTestGame
{
    public class Bat : Enemy
    {
        public Bat()
        {
            Health = 2;
            Colliding = false;
            Moving = true;
            
            var texture = Static.Content.Load<Texture2D>("linktothepast/enemy-sprites");
            SAnimation.DefaultFrameWidth = SAnimation.DefaultFrameHeight = 34;

            Dictionary<string, SAnimation> animations = new Dictionary<string, SAnimation>()
            {
                { "Default", new SAnimation(texture, 3, 0.1f, true) },
                { "TakenDamage", new SAnimation(texture, 1, 0.1f, false) }
            };

            Sprite.SetAnimations(animations, "Default");
            Hitbox.Load(24, 10);
            SpriteOffset = new Vector2(-5, -14);

            Dictionary<string, State> states = new Dictionary<string, State>()
            {
                { "Default", new BatStateDefault(this) },
                { "TakenDamage", new BatStateTakenDamage(this) }
            };

            StateMachine = new StateMachine(states, "Default");
        }

        /*public override void Update(GameTime gameTime)
        {
            Sprite.Update(gameTime);
            StateMachine.Update(gameTime);
        }*/

        public override void DeterminePlayerDamage()
        {
            var playerRectangle = Static.Scene.Player.Hitbox.Rectangle;

            if (Hitbox.Rectangle.Intersects(playerRectangle))
            {
                Static.Scene.Player.TakeDamage(Hitbox.Rectangle.Center);
            }
        }
    }
}
