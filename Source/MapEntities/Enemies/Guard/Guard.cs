using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZA6.Managers;
using ZA6.Models;
using ZA6.Sprites;

namespace ZA6
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
            
            var texture = Static.Content.Load<Texture2D>("linktothepast/guard-sprites");
            SAnimation.DefaultFrameWidth = 20;
            SAnimation.DefaultFrameHeight = 28;

            Dictionary<string, SAnimation> animations = new Dictionary<string, SAnimation>()
            {
                { "IdleDown",           new SAnimation(texture, 1, 0.04f, false, 1, 0) },
                { "IdleUp",             new SAnimation(texture, 1, 0.04f, false, 7, 0) },
                { "IdleLeft",           new SAnimation(texture, 1, 0.04f, false, 5, 0) },
                { "IdleRight",          new SAnimation(texture, 1, 0.04f, false, 3, 0) },
                { "WalkDown",           new SAnimation(texture, 2, 0.2f,  true,  0, 0) },
                { "WalkUp",             new SAnimation(texture, 2, 0.2f,  true,  6, 0) },
                { "WalkLeft",           new SAnimation(texture, 2, 0.2f,  true,  4, 0) },
                { "WalkRight",          new SAnimation(texture, 2, 0.2f,  true,  2, 0) },
                { "RunDown",            new SAnimation(texture, 2, 0.1f, true,  0, 0) },
                { "RunUp",              new SAnimation(texture, 2, 0.1f, true,  6, 0) },
                { "RunLeft",            new SAnimation(texture, 2, 0.1f, true,  4, 0) },
                { "RunRight",           new SAnimation(texture, 2, 0.1f, true,  2, 0) },
                /*{ "LookAroundDown",     new SAnimation(texture, 4, 0.8f,  true,  1, 0) },
                { "LookAroundUp",       new SAnimation(texture, 4, 0.8f,  true,  7, 0) },
                { "LookAroundLeft",     new SAnimation(texture, 4, 0.8f,  true,  5, 0) },
                { "LookAroundRight",    new SAnimation(texture, 4, 0.8f,  true,  3, 0) },*/
                { "IdleDownLookLeft",   new SAnimation(texture, 1, 0.04f, false, 1, 3) },
                { "IdleDownLookRight",  new SAnimation(texture, 1, 0.04f, false, 1, 1) },
                { "IdleUpLookLeft",     new SAnimation(texture, 1, 0.04f, false, 7, 1) },
                { "IdleUpLookRight",    new SAnimation(texture, 1, 0.04f, false, 7, 3) },
                { "IdleLeftLookUp",     new SAnimation(texture, 1, 0.04f, false, 5, 3) },
                { "IdleLeftLookDown",   new SAnimation(texture, 1, 0.04f, false, 5, 1) },
                { "IdleRightLookUp",    new SAnimation(texture, 1, 0.04f, false, 3, 1) },
                { "IdleRightLookDown",  new SAnimation(texture, 1, 0.04f, false, 3, 3) },
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
            Static.Scene.UnregisterHitbox(DetectionHitbox);
            DetectionHitbox.Dispose();
            Static.Scene.UnregisterHitbox(DamageHitbox1);
            DamageHitbox1.Dispose();
            Static.Scene.UnregisterHitbox(DamageHitbox2);
            DamageHitbox2.Dispose();
        }

        public override void DeterminePlayerDamage()
        {
            var playerRectangle = Static.Scene.Player.Hitbox.Rectangle;
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
                Static.Scene.Player.TakeDamage(Hitbox.Rectangle.Center);
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
            
            return DetectionHitbox.Rectangle.Intersects(Static.Scene.Player.Hitbox.Rectangle);
        }
    }
}
