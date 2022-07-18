using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTestGame.Manangers;

namespace MonoGameTestGame
{
    public static class StaticData
    {
        public static int NativeWidth = 256;
        public static int NativeHeight = 224;
        public static int BackBufferWidth = NativeWidth * 3;
        public static int BackBufferHeight = NativeHeight * 3;
        public static SpriteFont Font;
        public static ContentManager Content;
        public static GraphicsDeviceManager Graphics;
        public static SpriteBatch SpriteBatch;
        public static SceneManager SceneManager;
        public static Scene Scene;
    }
}