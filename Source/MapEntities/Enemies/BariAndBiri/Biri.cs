using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZA6.Managers;
using ZA6.Models;
using ZA6.Sprites;

namespace ZA6
{
    public class Biri : Bari
    {
        public Biri()
        {
            Health = 1;

            var texture = Img.EnemySprites;
            SAnimation.DefaultFrameWidth = SAnimation.DefaultFrameHeight = 34;

            Dictionary<string, SAnimation> animations = new Dictionary<string, SAnimation>()
            {
                { "Default", new SAnimation(texture, 2, 0.4f, true, 8) },
                { "Attacking", new SAnimation(texture, 1, 0.4f, false, 8, 2) },
                { "TakenDamage", new SAnimation(texture, 1, 0.4f, false, 8) }
            };

            Sprite.SetAnimations(animations, "Default");
            Hitbox.Load(5, 5);
            DamageHitbox1.Load(8, 8);
            SpriteOffset = new Vector2(-14, -16);

            Dictionary<string, State> states = new Dictionary<string, State>()
            {
                { "Default", new BariStateDefault(this) },
                { "Attacking", new BariStateAttacking(this) },
                { "TakenDamage", new BariStateTakenDamage(this) }
            };

            StateMachine = new StateMachine(states, "Default");
        }

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
