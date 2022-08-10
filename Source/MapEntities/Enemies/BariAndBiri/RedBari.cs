using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTestGame.Managers;
using MonoGameTestGame.Models;
using MonoGameTestGame.Sprites;

namespace MonoGameTestGame
{
    public class RedBari : Bari
    {   
        public RedBari()
        {
            var texture = Img.EnemySprites;
            SAnimation.DefaultFrameWidth = SAnimation.DefaultFrameHeight = 34;

            Dictionary<string, SAnimation> animations = new Dictionary<string, SAnimation>()
            {
                { "Default", new SAnimation(texture, 2, 0.4f, true, 7) },
                { "Attacking", new SAnimation(texture, 1, 0.4f, false, 7, 2) },
                { "TakenDamage", new SAnimation(texture, 1, 0.4f, false, 7) }
            };
            
            Sprite.SetAnimations(animations, "Default");
        }

        public override void Die()
        {
            base.Die();
            Static.Scene.Add(new Biri() { Position = Position });
        }
    }
}
