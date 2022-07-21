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
        public static SoundEffect Sword1;
        public static SoundEffect Sword2;
        public static SoundEffect WalkGrass;
        public static void Load()
        {
            ChestOpen = StaticData.Content.Load<SoundEffect>("linktothepast/sfx/chest open");
            Cursor = StaticData.Content.Load<SoundEffect>("linktothepast/sfx/cursor");
            EnemyDies = StaticData.Content.Load<SoundEffect>("linktothepast/sfx/enemy dies");
            EnemyHit = StaticData.Content.Load<SoundEffect>("linktothepast/sfx/enemy hit");
            Error = StaticData.Content.Load<SoundEffect>("linktothepast/sfx/error");
            MenuClose = StaticData.Content.Load<SoundEffect>("linktothepast/sfx/menu close");
            MenuOpen = StaticData.Content.Load<SoundEffect>("linktothepast/sfx/menu open");
            Message = StaticData.Content.Load<SoundEffect>("linktothepast/sfx/message");
            MessageFinish = StaticData.Content.Load<SoundEffect>("linktothepast/sfx/message finish");
            Sword1 = StaticData.Content.Load<SoundEffect>("linktothepast/sfx/fighter sword 1");
            Sword2 = StaticData.Content.Load<SoundEffect>("linktothepast/sfx/fighter sword 2");
            WalkGrass = StaticData.Content.Load<SoundEffect>("linktothepast/sfx/walk grass");
        }
    }
}