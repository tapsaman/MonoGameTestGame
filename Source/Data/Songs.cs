using Microsoft.Xna.Framework.Media;

namespace ZA6
{
    public static class Songs
    {
        public static Song DarkWorld;
        public static Song Forest;
        public static Song CrispyWorld;
        public static Song Mario;
        public static Song Screamer;
        public static Song Intro;

        public static void Load()
        {
            DarkWorld = Static.Content.Load<Song>("linktothepast/darkworld");
            Forest = Static.Content.Load<Song>("linktothepast/forest");
            CrispyWorld = Static.Content.Load<Song>("linktothepast/darkworld_bassboost");
            Mario = Static.Content.Load<Song>("mario-bros-main-theme-overworld");
            Screamer = Static.Content.Load<Song>("jump-scare");
            Intro = Static.Content.Load<Song>("loz_intro");
        }
    }
}