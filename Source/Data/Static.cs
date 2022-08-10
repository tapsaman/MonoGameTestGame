using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ZA6.Managers;
using ZA6.Manangers;

namespace ZA6
{
    public static class Static
    {
        public static ZeldaAdventure666 Game;
        public static Player Player;
        public static bool GameStarted;
        public static int NativeWidth = 256;
        public static int NativeHeight = 224;
        public static Vector2 NativeSize = new Vector2(NativeWidth, NativeHeight);
        //public static int NativeSizeMultiplier = 4;
        //public static int BackBufferWidth = (NativeWidth * NativeSizeMultiplier);
        //public static int BackBufferHeight = (NativeHeight * NativeSizeMultiplier);
        public static bool RenderHitboxes = true;
        public static bool GamePadEnabled;
        public static SpriteFont Font;
        public static ContentManager Content;
        public static GraphicsDeviceManager Graphics;
        public static SpriteBatch SpriteBatch;
        public static SceneManager SceneManager;
        public static DialogManager DialogManager;
        public static Renderer Renderer;
        public static EventSystem EventSystem;
        public static Scene Scene;
        public static string TiledProjectDirectory;
    }
}