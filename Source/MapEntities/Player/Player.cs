using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTestGame.Managers;
using MonoGameTestGame.Models;
using MonoGameTestGame.Sprites;

namespace MonoGameTestGame
{
    public class Player : Character
    {
        public int MaxHealth { get; private set; } = 6;
        public bool Hitting = false;
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
            Colliding = true;
            Moving = true;
            WalkSpeed = 70f;
            SpriteOffset = new Vector2(-13, -24);

            var texture = Static.Content.Load<Texture2D>("linktothepast/link-sprites");
            SAnimation.DefaultFrameWidth = 40;
            SAnimation.DefaultFrameHeight = 50;

            Dictionary<string, SAnimation> animations = new Dictionary<string, SAnimation>()
            {
                { "IdleDown",       new SAnimation(texture, 1, 0.04f, false, 0, 1) },
                { "IdleUp",         new SAnimation(texture, 1, 0.04f, false, 1, 3) },
                { "IdleLeft",       new SAnimation(texture, 1, 0.04f, false, 2, 3) },
                { "IdleRight",      new SAnimation(texture, 1, 0.04f, false, 3, 3) },
                { "WalkDown",       new SAnimation(texture, 8, 0.04f, true, 0) },
                { "WalkUp",         new SAnimation(texture, 8, 0.04f, true, 1) },
                { "WalkLeft",       new SAnimation(texture, 6, 0.04f, true, 2) },
                { "WalkRight",      new SAnimation(texture, 6, 0.04f, true, 3) },
                { "SwordHitDown",   new SAnimation(texture, 6, 0.04f, false, 4) },
                { "SwordHitUp",     new SAnimation(texture, 5, 0.04f, false, 5) },
                { "SwordHitLeft",   new SAnimation(texture, 5, 0.04f, false, 6) },
                { "SwordHitRight",  new SAnimation(texture, 5, 0.04f, false, 7) },
                { "Falling",        new SAnimation(texture, 6, 0.04f, false, 4) },
            };

            Hitbox.Load(14, 14);
            Sprite.SetAnimations(animations);

            // First parameter is considered width when Link faces up or down
            SwordHitbox = new SwordHitbox(14, 10) { Color = Color.HotPink };

            Dictionary<string, State> states = new Dictionary<string, State>()
            {
                { "Idle", new PlayerStateIdle(this) },
                { "Walking", new PlayerStateWalking(this) },
                { "SwordHit", new PlayerStateSwordHit(this) },
                { "TakenDamage", new PlayerStateTakenDamage(this) },
                { "Falling", new PlayerStateFalling(this) },
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
                
                foreach (var entity in Static.Scene.HittableEntities)
                {
                    if (SwordHitbox.Rectangle.Intersects(entity.Hitbox.Rectangle))
                    {
                        entity.InvokeTrigger(this);
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

        public void TakeDamage(Vector2 hitPosition)
        {
            if (!IsInvincible)
            {
                Health = Math.Max(0, Health - 1);
                IsInvincible = true;
                HitPosition = hitPosition;
                StateMachine.TransitionTo("TakenDamage");
            }
        }

        public void DetermineInputVelocity()
        {
            Velocity = Input.P1.GetDirectionVector() * WalkSpeed;
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
            if (Direction == Direction.Up)
                return new FloatRectangle(Hitbox.Rectangle.Left, Hitbox.Rectangle.Top - _touchAreaLength, Hitbox.Rectangle.Width, _touchAreaLength);
            if (Direction == Direction.Right)
                return new FloatRectangle(Hitbox.Rectangle.Right, Hitbox.Rectangle.Top, _touchAreaLength, Hitbox.Rectangle.Height);
            if (Direction == Direction.Down)
                return new FloatRectangle(Hitbox.Rectangle.Left, Hitbox.Rectangle.Bottom, Hitbox.Rectangle.Width, _touchAreaLength);
            
            return new FloatRectangle(Hitbox.Rectangle.Left - _touchAreaLength, Hitbox.Rectangle.Top, _touchAreaLength, Hitbox.Rectangle.Height);
        }
    }
}
