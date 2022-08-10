using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameTestGame
{
    public static class Img
    {
        public static Texture2D TileTexture;
        public static Texture2D ObjectTexture;
        public static Texture2D EnemySprites;
        public static Texture2D Shadow;
        
        public static void Load()
        {
            TileTexture = Static.Content.Load<Texture2D>("TiledProjects/testmap/linktothepast-tiles");
            ObjectTexture = Static.Content.Load<Texture2D>("TiledProjects/testmap/linktothepast-objects");
            EnemySprites = Static.Content.Load<Texture2D>("linktothepast/enemy-sprites");
            Shadow = Static.Content.Load<Texture2D>("linktothepast/new-link-sprite-main");
        }
    }
}