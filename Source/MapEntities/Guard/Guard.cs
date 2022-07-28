using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTestGame.Managers;
using MonoGameTestGame.Models;
using MonoGameTestGame.Sprites;

namespace MonoGameTestGame
{
    public class Guard : Enemy
    {
        public Hitbox DetectionHitbox;
        public Hitbox DamageHitbox1;
        public Hitbox DamageHitbox2;

        public Guard()
        {
            Health = 3;
            Colliding = true;
            Moving = true;
            Hitbox.Load(14, 14);
            SpriteOffset = new Vector2(-3, -14);
            WalkSpeed = 30f;
            Direction = Utility.RandomDirection();

            DetectionHitbox = new Hitbox();
            DetectionHitbox.Load(60, 100);
            DamageHitbox1 = new Hitbox(16, 16) { Color = Color.Orange };
            DamageHitbox2 = new Hitbox(4, 8) { Color = Color.Pink };
            
            var texture = StaticData.Content.Load<Texture2D>("linktothepast/guard-sprites");
            Animation.DefaultFrameWidth = 20;
            Animation.DefaultFrameHeight = 28;

            Dictionary<string, Animation> animations = new Dictionary<string, Animation>()
            {
                { "IdleDown",           new Animation(texture, 1, 0.04f, false, 1, 0) },
                { "IdleUp",             new Animation(texture, 1, 0.04f, false, 7, 0) },
                { "IdleLeft",           new Animation(texture, 1, 0.04f, false, 5, 0) },
                { "IdleRight",          new Animation(texture, 1, 0.04f, false, 3, 0) },
                { "WalkDown",           new Animation(texture, 2, 0.2f,  true,  0, 0) },
                { "WalkUp",             new Animation(texture, 2, 0.2f,  true,  6, 0) },
                { "WalkLeft",           new Animation(texture, 2, 0.2f,  true,  4, 0) },
                { "WalkRight",          new Animation(texture, 2, 0.2f,  true,  2, 0) },
                { "RunDown",            new Animation(texture, 2, 0.1f, true,  0, 0) },
                { "RunUp",              new Animation(texture, 2, 0.1f, true,  6, 0) },
                { "RunLeft",            new Animation(texture, 2, 0.1f, true,  4, 0) },
                { "RunRight",           new Animation(texture, 2, 0.1f, true,  2, 0) },
                /*{ "LookAroundDown",     new Animation(texture, 4, 0.8f,  true,  1, 0) },
                { "LookAroundUp",       new Animation(texture, 4, 0.8f,  true,  7, 0) },
                { "LookAroundLeft",     new Animation(texture, 4, 0.8f,  true,  5, 0) },
                { "LookAroundRight",    new Animation(texture, 4, 0.8f,  true,  3, 0) },*/
                { "IdleDownLookLeft",   new Animation(texture, 1, 0.04f, false, 1, 3) },
                { "IdleDownLookRight",  new Animation(texture, 1, 0.04f, false, 1, 1) },
                { "IdleUpLookLeft",     new Animation(texture, 1, 0.04f, false, 7, 1) },
                { "IdleUpLookRight",    new Animation(texture, 1, 0.04f, false, 7, 3) },
                { "IdleLeftLookUp",     new Animation(texture, 1, 0.04f, false, 5, 3) },
                { "IdleLeftLookDown",   new Animation(texture, 1, 0.04f, false, 5, 1) },
                { "IdleRightLookUp",    new Animation(texture, 1, 0.04f, false, 3, 1) },
                { "IdleRightLookDown",  new Animation(texture, 1, 0.04f, false, 3, 3) },
            };

            Sprite.SetAnimations(animations);
            
            Dictionary<string, State> states = new Dictionary<string, State>()
            {
                { "Default", new GuardStateDefault(this) }, // walks some length -> look around
                { "LookAround", new GuardStateLookAround(this) }, // scans around, change to random direction -> default
                { "NoticedPlayer", new GuardStateNoticedPlayer(this) }, // looks to player's direction a short while -> attacking
                { "Attacking", new GuardStateAttacking(this) }, // charges toward player -> look around
                { "TakenDamage", new EnemyStateTakenDamage(this) }, // flies to hit direction -> attacking
            };

            StateMachine = new StateMachine(states, "Default");
        }

        ~Guard()
        {
            Unload();
        }

        public override void Unload()
        {
            base.Unload();
            StaticData.Scene.UnregisterHitbox(DetectionHitbox);
            DetectionHitbox.Dispose();
            StaticData.Scene.UnregisterHitbox(DamageHitbox1);
            DamageHitbox1.Dispose();
            StaticData.Scene.UnregisterHitbox(DamageHitbox2);
            DamageHitbox2.Dispose();
        }

        public override void DeterminePlayerDamage()
        {
            var playerRectangle = StaticData.Scene.Player.Hitbox.Rectangle;
            DamageHitbox1.Position = Position + new Vector2(-1, -1);

            switch(Direction)
            {
                case Direction.Up:
                    DamageHitbox2.Load(4, 8);
                    DamageHitbox2.Position = Position + new Vector2(1, -8);
                    break;
                case Direction.Down:
                    DamageHitbox2.Load(4, 8);
                    DamageHitbox2.Position = Position + new Vector2(10, 6);
                    break;
                case Direction.Left:
                    DamageHitbox2.Load(8, 4);
                    DamageHitbox2.Position = Position + new Vector2(-10, 10);
                    break;
                case Direction.Right:
                    DamageHitbox2.Load(8, 4);
                    DamageHitbox2.Position = Position + new Vector2(16, 10);
                    break;
            }

            if (DamageHitbox1.Rectangle.Intersects(playerRectangle) || DamageHitbox2.Rectangle.Intersects(playerRectangle))
            {
                StaticData.Scene.Player.TakeDamage(Hitbox.Rectangle.Center);
            }
        }

        public bool DetectingPlayer(Direction direction = Direction.None)
        {
            direction = direction == Direction.None ? Direction : direction;

            switch(direction)
            {
                case Direction.Up:
                    DetectionHitbox.Load(60, 100);
                    DetectionHitbox.Position = Position - new Vector2(23, 80);
                    break;
                case Direction.Down:
                    DetectionHitbox.Load(60, 100);
                    DetectionHitbox.Position = Position - new Vector2(23, 6);
                    break;
                case Direction.Left:
                    DetectionHitbox.Load(100, 60);
                    DetectionHitbox.Position = Position - new Vector2(80, 23);
                    break;
                case Direction.Right:
                    DetectionHitbox.Load(100, 60);
                    DetectionHitbox.Position = Position - new Vector2(6, 23);
                    break;
            }
            
            return DetectionHitbox.Rectangle.Intersects(StaticData.Scene.Player.Hitbox.Rectangle);
        }
    }
}
