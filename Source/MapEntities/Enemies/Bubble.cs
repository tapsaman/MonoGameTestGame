using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZA6.Managers;
using ZA6.Models;
using TapsasEngine.Sprites;

namespace ZA6
{
    public class Bubble : Enemy
    {
        public Hitbox DamageHitbox1;
        
        public Bubble()
        {
            Hittable = false;
            Moving = true;
            
            var texture = Img.EnemySprites;
            SAnimation.DefaultFrameWidth = SAnimation.DefaultFrameHeight = 34;

            Dictionary<string, SAnimation> animations = new Dictionary<string, SAnimation>()
            {
                { "Default", new SAnimation(texture, 5, 0.14f, true, 5) }
            };

            AnimatedSprite = new AnimatedSprite(animations, "Default");
            Hitbox.Load(10, 10);
            DamageHitbox1 = new Hitbox();
            DamageHitbox1.Load(12, 12);
            SpriteOffset = new Vector2(-13, -14);

            Dictionary<string, State> states = new Dictionary<string, State>()
            {
                { "Default", new BubbleStateDefault(this) }
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
            DamageHitbox1.Position = Position + new Vector2(-1, -1);

            if (DamageHitbox1.Rectangle.Intersects(playerRectangle))
            {
                Static.Scene.Player.TakeDamage(DamageHitbox1.Rectangle.Center);
            }
        }
    }
}
