using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZA6.Models;

namespace ZA6.Animations
{
    public class BushSlash : SAnimationEffect
    {
        public BushSlash(Vector2 position): base(position) {}
        
        protected override SAnimation Load()
        {
            var texture = Static.Content.Load<Texture2D>("linktothepast/animation-bush-slash");
            Position -= new Vector2(15, 21);
            return new SAnimation(texture, 8, 29, 43, 0.1f);
        }
    }
}