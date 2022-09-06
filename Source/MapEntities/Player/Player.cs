using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TapsasEngine;
using TapsasEngine.Enums;
using ZA6.Managers;
using ZA6.Models;
using TapsasEngine.Sprites;

namespace ZA6
{
    public class Player : Character
    {
        public int MaxHealth { get; private set; } = 12;
        public int Rupees;
        public bool Hitting = false;
        public bool CanDie = true;
        private const int _touchAreaLength = 10;
        public Vector2 HitPosition;
        private const float _INVINCIBLE_TIME = 1.5f;
        private float _elapsedInvincibleTime = 0;
        private bool _draw = true;

        public SwordHitbox SwordHitbox;
        
        public Player()
        {
            Health = MaxHealth;
            Interactable = false;
            Hittable = false;
            Moving = true;
            WalkSpeed = 90f;
            SpriteOffset = new Vector2(-13, -24);

            var txt1 = Static.Content.Load<Texture2D>("Sprites/new-link-sprite-main");
            var txt2 = Static.Content.Load<Texture2D>("Sprites/new-link-sprite-sword");
            var offset1 = new Vector2(8, 11);
            var offset2 = new Vector2(-4, -1);
            SAnimation.DefaultFrameWidth = 24;
            SAnimation.DefaultFrameHeight = 28;
            Dictionary<string, SAnimation> animations = new Dictionary<string, SAnimation>()
            {
                { "IdleDown",       new SAnimation(txt1, 0, 10, 24, 28, offset: offset1) },
                { "IdleLeft",       new SAnimation(txt1, 1, 10, 24, 28, offset: offset1) },
                { "IdleUp",         new SAnimation(txt1, 2, 10, 24, 28, offset: offset1) },
                { "IdleRight",      new SAnimation(txt1, 3, 10, 24, 28, offset: offset1) },
                { "DamagedDown",    new SAnimation(txt1, 4, 10, 24, 28, offset: offset1) },
                { "DamagedLeft",    new SAnimation(txt1, 5, 10, 24, 28, offset: offset1) },
                { "DamagedUp",      new SAnimation(txt1, 6, 10, 24, 28, offset: offset1) },
                { "DamagedRight",   new SAnimation(txt1, 7, 10, 24, 28, offset: offset1) },
                { "WalkDown",       new SAnimation(txt1, 8, 0.04f, true, 0, 0, offset: offset1) },
                { "WalkUp",         new SAnimation(txt1, 8, 0.04f, true, 1, 0, offset: offset1) },
                { "WalkRight",      new SAnimation(txt1, 8, 0.04f, true, 2, 0, offset: offset1) },
                { "WalkLeft",       new SAnimation(txt1, 8, 0.04f, true, 3, 0, offset: offset1) },
                { "GotItem",        new SAnimation(txt1, 6, 12, 24, 28, offset: offset1) },
                { "Falling",        new SAnimation(txt1, 7, 0.2f, false, 11, 0, offset: offset1) },
                { "Dying",          new SAnimation(txt1, 0.07f, offset1, new Rectangle[]
                    {
                        new Rectangle(0,   336, 24, 28),
                        new Rectangle(24,  336, 24, 28),
                        new Rectangle(48,  336, 24, 28),
                        new Rectangle(72,  336, 24, 28),
                        new Rectangle(0,   336, 24, 28),
                        new Rectangle(24,  336, 24, 28),
                        new Rectangle(48,  336, 24, 28),
                        new Rectangle(72,  336, 24, 28),
                        new Rectangle(0,   336, 24, 28),
                        new Rectangle(24,  336, 24, 28),
                        new Rectangle(48,  336, 24, 28),
                        new Rectangle(72,  336, 24, 28),
                        // Fall down
                        new Rectangle(96,  336, 24, 28),
                        new Rectangle(120, 336, 24, 28),
                    }
                )},
                { "SwordHitDown",   new SAnimation(txt2, 10, 46, 50, 0.04f, 0, offset: offset2) },
                { "SwordHitUp",     new SAnimation(txt2, 10, 46, 50, 0.04f, 1, offset: offset2) },
                { "SwordHitLeft",   new SAnimation(txt2, 10, 46, 50, 0.04f, 3, offset: offset2) },
                { "SwordHitRight",  new SAnimation(txt2, 10, 46, 50, 0.04f, 2, offset: offset2) },
            };

            Hitbox.Load(14, 14);
            AnimatedSprite = new AnimatedSprite(animations, "IdleDown");

            // First parameter is considered width when Link faces up or down
            SwordHitbox = new SwordHitbox(14, 10) { Color = Color.HotPink };

            Dictionary<string, State> states = new Dictionary<string, State>()
            {
                { "Idle", new PlayerStateIdle(this) },
                { "Walking", new PlayerStateWalking(this) },
                { "SwordHit", new PlayerStateSwordHit(this) },
                { "TakenDamage", new PlayerStateTakenDamage(this) },
                { "Falling", new PlayerStateFalling(this) },
                { "Dying", new PlayerStateDying(this) },
                { "Stopped", new PlayerStateStopped(this) }
            };

            StateMachine = new StateMachine(states, "Idle");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (IsInvincible)
            {
                _elapsedInvincibleTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (_elapsedInvincibleTime > _INVINCIBLE_TIME)
                {
                    _elapsedInvincibleTime = 0;
                    IsInvincible = false;
                    _draw = true;
                }
                else if (_elapsedInvincibleTime > 0.5f)
                {
                    _draw = !_draw;
                }
            }
            
            if (SwordHitbox.Enabled)
            {
                SwordHitbox.Update(gameTime);
                
                for (int i = Static.Scene.HittableEntities.Count - 1; i >= 0 ; i--)
                {
                    var entity = Static.Scene.HittableEntities[i];

                    if (SwordHitbox.Rectangle.Intersects(entity.Hitbox.Rectangle))
                    {
                        entity.TakeHit(this);
                    }
                }
            }
            else if (Static.Scene.TileMap.Exits.ContainsKey(MapBorder))
            {
                Static.SceneManager.GoTo(Static.Scene.TileMap.Exits[MapBorder]);
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            //if (!IsInvincible || _elapsedInvincibleTime < 0.5f || Math.Round((decimal)_elapsedInvincibleTime, 1) % 0.2M != 0)
            if (_draw)
            {
                base.Draw(spriteBatch, offset);
            }
        }

        public override void MakeFall()
        {
            StateMachine.TransitionTo("Falling");
        }

        public void TakeDamage(Vector2 hitPosition, int amount = 1)
        {
            if (!IsInvincible)
            {
                Health = Math.Max(0, Health - amount);
                
                if (Health == 0 && CanDie)
                {
                    Static.Game.StateMachine.TransitionTo("GameOver");
                }
                else
                {
                    IsInvincible = true;
                    HitPosition = hitPosition;
                    StateMachine.TransitionTo("TakenDamage");
                }
            }
        }

        public void ReplenishHealth(int amount)
        {
            Health = Math.Min(MaxHealth, Health + amount);
        }

        public void DetermineInputVelocity()
        {
            Velocity = Input.P1.GetDirectionVector();

            if (Velocity != Vector2.Zero)
                Velocity.Normalize();
            
            Velocity *= WalkSpeed;
        }

        public void DetermineHitInput()
        {
            if (!Input.P1.IsPressed(Input.P1.A))
                return;

            if (Input.P1.JustPressed(Input.P1.A))
            {
                var touchArea = GetTouchArea();

                foreach (var mapEntity in Static.Scene.InteractableEntities)
                {
                    if (touchArea.Intersects(mapEntity.Hitbox.Rectangle))
                    {
                        mapEntity.InvokeTrigger(this);
                        return;
                    }
                }
                Hitting = true;
                Velocity = Vector2.Zero;
            }
            else
            {
                //Hit = true;
            }
        }

        private FloatRectangle GetTouchArea()
        {
            if (Facing == Direction.Up)
                return new FloatRectangle(Hitbox.Rectangle.Left, Hitbox.Rectangle.Top - _touchAreaLength, Hitbox.Rectangle.Width, _touchAreaLength);
            if (Facing == Direction.Right)
                return new FloatRectangle(Hitbox.Rectangle.Right, Hitbox.Rectangle.Top, _touchAreaLength, Hitbox.Rectangle.Height);
            if (Facing == Direction.Down)
                return new FloatRectangle(Hitbox.Rectangle.Left, Hitbox.Rectangle.Bottom, Hitbox.Rectangle.Width, _touchAreaLength);
            
            return new FloatRectangle(Hitbox.Rectangle.Left - _touchAreaLength, Hitbox.Rectangle.Top, _touchAreaLength, Hitbox.Rectangle.Height);
        }
    }
}
