using Microsoft.Xna.Framework;
using TapsasEngine.Sprites;

namespace ZA6
{
    public class BushStump : MapObject
    {
        public override MapLevel Level { get => MapLevel.Ground; }

        public BushStump()
        {
            Sprite = new Sprite(Img.ObjectTexture, new Rectangle(16, 0, 16, 16));
        }
    }
}