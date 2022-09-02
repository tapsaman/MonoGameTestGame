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
        public static Texture2D NPCSprites;
        public static Texture2D EnemySprites;
        public static Texture2D Shadow;
        public static Texture2D LifeHUD;
        public static Texture2D RupeeHUD;
        public static Texture2D SeppoFace;
        
        public static void Load()
        {
            GameTitle = Static.Content.Load<Texture2D>("zelda_adventure_title");
            GameNumberTitle = Static.Content.Load<Texture2D>("zelda_adventure_666_title");
            TileTexture = Static.Content.Load<Texture2D>("TiledProject/linktothepast-tiles");
            CaveTileTexture = Static.Content.Load<Texture2D>("TiledProject/linktothepast-cave-tiles");
            ObjectTexture = Static.Content.Load<Texture2D>("TiledProject/linktothepast-objects");
            NPCSprites = Static.Content.Load<Texture2D>("linktothepast/npc-sprites");
            EnemySprites = Static.Content.Load<Texture2D>("linktothepast/enemy-sprites");
            Shadow = Static.Content.Load<Texture2D>("linktothepast/new-link-sprite-main");
            LifeHUD = Static.Content.Load<Texture2D>("linktothepast/life-hud");
            RupeeHUD = Static.Content.Load<Texture2D>("linktothepast/rupee-hud");
            SeppoFace = Static.Content.Load<Texture2D>("seppo_face");
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