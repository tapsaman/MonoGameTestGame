using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZA6.Managers;
using ZA6.Models;
using ZA6.Sprites;

namespace ZA6
{
    public class Bat : Enemy
    {
        public bool DidHitPlayer = false;

        public Bat()
        {
            Health = 2;
            Colliding = false;
            Moving = true;
            
            var texture = Static.Content.Load<Texture2D>("linktothepast/enemy-sprites");
            SAnimation.DefaultFrameWidth = SAnimation.DefaultFrameHeight = 34;

            Dictionary<string, SAnimation> animations = new Dictionary<string, SAnimation>()
            {
                { "Waiting", new SAnimation(texture, 4, 0, 34, 34) },
                { "Default", new SAnimation(texture, 3, 0.1f, true) },
                { "TakenDamage", new SAnimation(texture, 1, 0.1f, false) }
            };

            Sprite.SetAnimations(animations, "Waiting");
            Hitbox.Load(24, 10);
            SpriteOffset = new Vector2(-5, -14);

            Dictionary<string, State> states = new Dictionary<string, State>()
            {
                { "Waiting", new BatStateWaiting(this) },
                { "Default", new BatStateDefault(this) },
                { "TakenDamage", new BatStateTakenDamage(this) }
            };

            StateMachine = new StateMachine(states, "Waiting");
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
                DidHitPlayer = true;
                Static.Scene.Player.TakeDamage(Hitbox.Rectangle.Center);
            }
        }
    }
}
