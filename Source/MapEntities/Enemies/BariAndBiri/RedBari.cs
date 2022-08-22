using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TapsasEngine.Enums;
using ZA6.Managers;
using ZA6.Models;
using TapsasEngine.Sprites;

namespace ZA6
{
    public class RedBari : Bari
    {   
        public RedBari()
        {
            Health = 1;
            var texture = Img.EnemySprites;
            SAnimation.DefaultFrameWidth = SAnimation.DefaultFrameHeight = 34;

            Dictionary<string, SAnimation> animations = new Dictionary<string, SAnimation>()
            {
                { "Default", new SAnimation(texture, 2, 0.4f, true, 7) },
                { "Attacking", new SAnimation(texture, 2, 0.1f, true, 7, 1) },
                { "TakenDamage", new SAnimation(texture, 0, 7) }
            };
            
            AnimatedSprite = new AnimatedSprite(animations, "Default");
        }

        public override void Die()
        {
            base.Die();
            Static.Scene.Add(new Biri() { Position = Position, PushVelocity = Direction.Right.ToVector() });
            Static.Scene.Add(new Biri() { Position = Position, PushVelocity = Direction.Left.ToVector() });
        }
    }
}
