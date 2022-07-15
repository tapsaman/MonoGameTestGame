using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameTestGame
{
    public static class StaticData
    {
        public static SpriteFont Font;
        public static ContentManager Content;
        public static Scene Scene;
        public static GraphicsDeviceManager Graphics;
        public static int NativeWidth = 256;
        public static int NativeHeight = 224;
        public static int BackBufferWidth = NativeWidth * 3;
        public static int BackBufferHeight = NativeHeight * 3;
    }
}