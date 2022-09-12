using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZA6.Models;

namespace ZA6.Animations
{
    public class RainbowBushSlash : SAnimationEffect
    {
        public RainbowBushSlash(Vector2 position): base(position) {}
        
        protected override SAnimation Load()
        {
            var texture = Static.Content.Load<Texture2D>("Sprites/animation-bush-slash");
            Position += new Vector2(5, -5);
            return new SAnimation(texture, 8, 29, 43, 0.1f, 1);
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            Static.Renderer.ChangeToEffect(Shaders.Rainbow);
            base.Draw(spriteBatch, offset);
            Static.Renderer.ChangeToDefault();
        }
    }
}