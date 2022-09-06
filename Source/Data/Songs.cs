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
        public static Song MorningSunlight;
        public static Song GameOver;

        public static void Load()
        {
            DarkWorld = Static.Content.Load<Song>("Songs/darkworld");
            Forest = Static.Content.Load<Song>("Songs/forest");
            CrispyWorld = Static.Content.Load<Song>("Songs/darkworld_bassboost");
            Mario = Static.Content.Load<Song>("Songs/mario-bros-main-theme-overworld");
            Screamer = Static.Content.Load<Song>("Songs/jump-scare");
            Intro = Static.Content.Load<Song>("Songs/loz_intro");
            MorningSunlight = Static.Content.Load<Song>("Songs/ct_Morning_Sunlight");
            GameOver = Static.Content.Load<Song>("Songs/oot_game_over");
        }
    }
}