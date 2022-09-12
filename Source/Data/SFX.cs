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
        public static SoundEffect ItemGet;
        public static SoundEffect RitualEndSound;
        public static SoundEffect Quake1;
        public static SoundEffect Quake2;
        public static SoundEffect MoveObject;
        public static SoundEffect Ram;
        public static SoundEffect FYou;

        public static void Load()
        {
            Volume = 0.5f;
            ChestOpen = Static.Content.Load<SoundEffect>("SFX/LinkToThePast/chest open");
            Cursor = Static.Content.Load<SoundEffect>("SFX/LinkToThePast/cursor");
            EnemyDies = Static.Content.Load<SoundEffect>("SFX/LinkToThePast/enemy dies");
            EnemyHit = Static.Content.Load<SoundEffect>("SFX/LinkToThePast/enemy hit");
            Error = Static.Content.Load<SoundEffect>("SFX/LinkToThePast/error");
            MenuClose = Static.Content.Load<SoundEffect>("SFX/LinkToThePast/menu close");
            MenuOpen = Static.Content.Load<SoundEffect>("SFX/LinkToThePast/menu open");
            Message = Static.Content.Load<SoundEffect>("SFX/LinkToThePast/message");
            MessageFinish = Static.Content.Load<SoundEffect>("SFX/LinkToThePast/message finish");
            Sword1 = Static.Content.Load<SoundEffect>("SFX/LinkToThePast/fighter sword 1");
            Sword2 = Static.Content.Load<SoundEffect>("SFX/LinkToThePast/fighter sword 2");
            WalkGrass = Static.Content.Load<SoundEffect>("SFX/LinkToThePast/walk grass");
            Soldier = Static.Content.Load<SoundEffect>("SFX/LinkToThePast/soldier");
            SmallEnemyHit = Static.Content.Load<SoundEffect>("SFX/LinkToThePast/small enemy hit");
            LinkHurt = Static.Content.Load<SoundEffect>("SFX/LinkToThePast/link hurt");
            LinkFall = Static.Content.Load<SoundEffect>("SFX/LinkToThePast/link falls");
            LinkDies = Static.Content.Load<SoundEffect>("SFX/LinkToThePast/link dies");
            Fall = Static.Content.Load<SoundEffect>("SFX/LinkToThePast/fall");
            LargeBeam = Static.Content.Load<SoundEffect>("SFX/LinkToThePast/large beam");
            Keese = Static.Content.Load<SoundEffect>("SFX/LinkToThePast/keese");
            Heart = Static.Content.Load<SoundEffect>("SFX/LinkToThePast/heart");
            LifeRefill = Static.Content.Load<SoundEffect>("SFX/LinkToThePast/life refill");
            Rupee = Static.Content.Load<SoundEffect>("SFX/LinkToThePast/rupee");
            LowHP = Static.Content.Load<SoundEffect>("SFX/LinkToThePast/low hp");
            Jump = Static.Content.Load<SoundEffect>("SFX/LinkToThePast/error");
            ItemGet = Static.Content.Load<SoundEffect>("SFX/LinkToThePast/item get 1");
            Quake1 = Static.Content.Load<SoundEffect>("SFX/LinkToThePast/quake 1");
            Quake2 = Static.Content.Load<SoundEffect>("SFX/LinkToThePast/quake 2");
            MoveObject = Static.Content.Load<SoundEffect>("SFX/LinkToThePast/move object");
            Ram = Static.Content.Load<SoundEffect>("SFX/LinkToThePast/ram");
            RitualEndSound = Static.Content.Load<SoundEffect>("SFX/halloween-impact-wav");
            FYou = Static.Content.Load<SoundEffect>("SFX/fyou");
        }
    }
}