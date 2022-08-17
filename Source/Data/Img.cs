using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace ZA6
{
    public static class Img
    {
        public static Texture2D GameTitle;
        public static Texture2D GameNumberTitle;
        public static Texture2D TileTexture;
        public static Texture2D CaveTileTexture;
        public static Texture2D ObjectTexture;
        public static Texture2D EnemySprites;
        public static Texture2D Shadow;
        
        public static void Load()
        {
            GameTitle = Static.Content.Load<Texture2D>("zelda_adventure_title");
            GameNumberTitle = Static.Content.Load<Texture2D>("zelda_adventure_666_title");
            TileTexture = Static.Content.Load<Texture2D>("TiledProjects/testmap/linktothepast-tiles");
            CaveTileTexture = Static.Content.Load<Texture2D>("TiledProjects/testmap/linktothepast-cave-tiles");
            ObjectTexture = Static.Content.Load<Texture2D>("TiledProjects/testmap/linktothepast-objects");
            EnemySprites = Static.Content.Load<Texture2D>("linktothepast/enemy-sprites");
            Shadow = Static.Content.Load<Texture2D>("linktothepast/new-link-sprite-main");
        }

        public static Texture2D TilesetImageToTexture(TiledCS.TiledImage image)
        {
            switch (image.source)
            {
                case "linktothepast-cave-tiles.png":
                    return CaveTileTexture;
                default:
                    return TileTexture;
            }
        }
    }
}