using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameTestGame
{
    public static class Img
    {
        public static Texture2D TileTexture;
        public static Texture2D ObjectTexture;
        public static Texture2D EnemyDies;
        public static void Load()
        {
            TileTexture = Static.Content.Load<Texture2D>("TiledProjects/testmap/linktothepast-tiles");
            ObjectTexture = Static.Content.Load<Texture2D>("TiledProjects/testmap/linktothepast-objects");
        }
    }
}