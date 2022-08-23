using Microsoft.Xna.Framework.Audio;

namespace ZA6
{
    public static class SFX
    {
        public static float Volume
        {
            get
            {
                return SoundEffect.MasterVolume;
            }
            set
            {
                SoundEffect.MasterVolume = value;
            }
        }
        
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
        public static SoundEffect Soldier;
        public static SoundEffect SmallEnemyHit;
        public static SoundEffect LinkHurt;
        public static SoundEffect LinkFall;
        public static SoundEffect LinkDies;
        public static SoundEffect Fall;
        public static SoundEffect LargeBeam;
        public static SoundEffect Keese;
        public static SoundEffect Heart;
        public static SoundEffect LifeRefill;
        public static SoundEffect Rupee;
        public static SoundEffect LowHP;
        public static SoundEffect Jump;

        public static void Load()
        {
            Volume = 0.5f;
            ChestOpen = Static.Content.Load<SoundEffect>("linktothepast/sfx/chest open");
            Cursor = Static.Content.Load<SoundEffect>("linktothepast/sfx/cursor");
            EnemyDies = Static.Content.Load<SoundEffect>("linktothepast/sfx/enemy dies");
            EnemyHit = Static.Content.Load<SoundEffect>("linktothepast/sfx/enemy hit");
            Error = Static.Content.Load<SoundEffect>("linktothepast/sfx/error");
            MenuClose = Static.Content.Load<SoundEffect>("linktothepast/sfx/menu close");
            MenuOpen = Static.Content.Load<SoundEffect>("linktothepast/sfx/menu open");
            Message = Static.Content.Load<SoundEffect>("linktothepast/sfx/message");
            MessageFinish = Static.Content.Load<SoundEffect>("linktothepast/sfx/message finish");
            Sword1 = Static.Content.Load<SoundEffect>("linktothepast/sfx/fighter sword 1");
            Sword2 = Static.Content.Load<SoundEffect>("linktothepast/sfx/fighter sword 2");
            WalkGrass = Static.Content.Load<SoundEffect>("linktothepast/sfx/walk grass");
            Soldier = Static.Content.Load<SoundEffect>("linktothepast/sfx/soldier");
            SmallEnemyHit = Static.Content.Load<SoundEffect>("linktothepast/sfx/small enemy hit");
            LinkHurt = Static.Content.Load<SoundEffect>("linktothepast/sfx/link hurt");
            LinkFall = Static.Content.Load<SoundEffect>("linktothepast/sfx/link falls");
            LinkDies = Static.Content.Load<SoundEffect>("linktothepast/sfx/link dies");
            Fall = Static.Content.Load<SoundEffect>("linktothepast/sfx/fall");
            LargeBeam = Static.Content.Load<SoundEffect>("linktothepast/sfx/large beam");
            Keese = Static.Content.Load<SoundEffect>("linktothepast/sfx/keese");
            Heart = Static.Content.Load<SoundEffect>("linktothepast/sfx/heart");
            LifeRefill = Static.Content.Load<SoundEffect>("linktothepast/sfx/life refill");
            Rupee = Static.Content.Load<SoundEffect>("linktothepast/sfx/rupee");
            LowHP = Static.Content.Load<SoundEffect>("linktothepast/sfx/low hp");
            Jump = Static.Content.Load<SoundEffect>("linktothepast/sfx/error");
        }
    }
}