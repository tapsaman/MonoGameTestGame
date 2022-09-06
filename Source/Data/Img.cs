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
        public static Texture2D NPCLargeSprites;
        public static Texture2D EnemySprites;
        public static Texture2D Shadow;
        public static Texture2D LifeHUD;
        public static Texture2D RupeeHUD;
        public static Texture2D SeppoFace;
        public static Texture2D MomFace;
        
        public static void Load()
        {
            GameTitle = Static.Content.Load<Texture2D>("Images/zelda_adventure_title");
            GameNumberTitle = Static.Content.Load<Texture2D>("Images/zelda_adventure_666_title");
            SeppoFace = Static.Content.Load<Texture2D>("Images/seppo_face");
            MomFace = Static.Content.Load<Texture2D>("Images/creepy_mom_face");
            TileTexture = Static.Content.Load<Texture2D>("TiledProject/linktothepast-tiles");
            CaveTileTexture = Static.Content.Load<Texture2D>("TiledProject/linktothepast-cave-tiles");
            ObjectTexture = Static.Content.Load<Texture2D>("TiledProject/linktothepast-objects");
            NPCSprites = Static.Content.Load<Texture2D>("Sprites/npcs-20x30");
            NPCLargeSprites = Static.Content.Load<Texture2D>("Sprites/npcs-30x40");
            EnemySprites = Static.Content.Load<Texture2D>("Sprites/enemy-sprites");
            Shadow = Static.Content.Load<Texture2D>("Sprites/new-link-sprite-main");
            LifeHUD = Static.Content.Load<Texture2D>("UI/life-hud");
            RupeeHUD = Static.Content.Load<Texture2D>("UI/rupee-hud");
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