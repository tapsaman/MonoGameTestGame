using Microsoft.Xna.Framework.Audio;

namespace MonoGameTestGame
{
    public static class SFX
    {
        public static SoundEffect ChestOpen;
        public static SoundEffect Cursor;
        public static SoundEffect EnemyDies;
        public static SoundEffect EnemyHit;
        public static SoundEffect Error;
        public static SoundEffect MenuClose;
        public static SoundEffect MenuOpen;
        public static SoundEffect Message;
        public static SoundEffect MessageFinish;
        public static void Load()
        {
            ChestOpen = StaticData.Content.Load<SoundEffect>("linktothepast-sfx/chest open");
            Cursor = StaticData.Content.Load<SoundEffect>("linktothepast-sfx/cursor");
            EnemyDies = StaticData.Content.Load<SoundEffect>("linktothepast-sfx/enemy dies");
            EnemyHit = StaticData.Content.Load<SoundEffect>("linktothepast-sfx/enemy hit");
            Error = StaticData.Content.Load<SoundEffect>("linktothepast-sfx/error");
            MenuClose = StaticData.Content.Load<SoundEffect>("linktothepast-sfx/menu close");
            MenuOpen = StaticData.Content.Load<SoundEffect>("linktothepast-sfx/menu open");
            Message = StaticData.Content.Load<SoundEffect>("linktothepast-sfx/message");
            MessageFinish = StaticData.Content.Load<SoundEffect>("linktothepast-sfx/message finish");
        }
    }
}