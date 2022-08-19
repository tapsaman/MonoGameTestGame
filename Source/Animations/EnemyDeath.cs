using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZA6.Models;

namespace ZA6.Animations
{
    public class EnemyDeath : SAnimationEffect
    {
        public EnemyDeath(Vector2 position): base(position) {}
        
        protected override SAnimation Load()
        {
            var texture = Static.Content.Load<Texture2D>("linktothepast/animation-enemy-death");
            Position -= new Vector2(15, 21);
            return new SAnimation(texture, 7, 26, 32, 0.1f);
        }
    }
}