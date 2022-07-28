using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTestGame.Models;

namespace MonoGameTestGame.Animations
{
    public class EnemyDeath : AnimationEffect
    {
        public EnemyDeath(Vector2 position): base(position) {}
        
        protected override Animation Load()
        {
            var texture = StaticData.Content.Load<Texture2D>("linktothepast/animation-enemy-death");
            Position -= new Vector2(15, 21);
            return new Animation(texture, 7, 26, 32, 0.1f);
        }
    }
}