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
            
            var texture = StaticData.Content.Load<Texture2D>("linktothepast/enemy-sprites");
            Animation.DefaultFrameWidth = Animation.DefaultFrameHeight = 34;

            Dictionary<string, Animation> animations = new Dictionary<string, Animation>()
            {
                { "Default", new Animation(texture, 3, 0.1f, true) },
                { "TakenDamage", new Animation(texture, 1, 0.1f, false) }
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
            var playerRectangle = StaticData.Scene.Player.Hitbox.Rectangle;

            if (Hitbox.Rectangle.Intersects(playerRectangle))
            {
                StaticData.Scene.Player.TakeDamage(Hitbox.Rectangle.Center);
            }
        }
    }
}
