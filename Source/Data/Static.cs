using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TapsasEngine.Utilities;
using ZA6.Managers;
using ZA6.Manangers;
using ZA6.Utilities;

namespace ZA6
{
    public static class Static
    {
        public const bool Debug = true;
        public static ZeldaAdventure666 Game;
        public static Player Player;
        public static SaveData LoadedGame;
        public static TiledWorld World;
        public static bool GameStarted;
        public static int NativeWidth = 256;
        public static int NativeHeight = 224;
        public static Vector2 NativeSize = new Vector2(NativeWidth, NativeHeight);
        //public static int NativeSizeMultiplier = 4;
        //public static int BackBufferWidth = (NativeWidth * NativeSizeMultiplier);
        //public static int BackBufferHeight = (NativeHeight * NativeSizeMultiplier);
        public static bool RenderHitboxes = false;
        public static bool RenderCollisionMap = false;
        public static bool GamePadEnabled;
        public static SpriteFont Font;
        public static SpriteFont FontSmall;
        public static ContentManager Content;
        public static SpriteBatch SpriteBatch;
        public static SceneManager  SceneManager;
        public static DialogManager DialogManager;
        public static GameRenderer Renderer;
        public static EventSystem EventSystem;
        public static Scene Scene;
        public static DevUtils DevUtils;
        public static DataStore SessionData;
        public static DataStore GameData;
        public static Timer PlayTimeTimer = new Timer();

        public static RenderResolution[] ResolutionOptions = new RenderResolution[]
        {
            new RenderResolution(256, 224, "Native"),
            new RenderResolution(512, 448, "Native x2"),
            new RenderResolution(768, 672, "Native x3"),
            new RenderResolution(1024, 896, "Native x4"),
            new RenderResolution(1234, 1080), // Native fitted for 1080 height
            new RenderResolution(0, 0) { IsFullscreen = true }
        };

        public static RenderResolution DefaultResolution = ResolutionOptions[3];

        public static DataStore GetStoreByType(DataStoreType type)
        {
            switch (type)
            {
                case DataStoreType.Session:
                    return Static.SessionData;
                case DataStoreType.Scene:
                default:
                    return Static.Scene.SceneData;
                
            }
        }
    }
}