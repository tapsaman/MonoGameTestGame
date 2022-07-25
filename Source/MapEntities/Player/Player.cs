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
        public bool Hitting = false;
        public bool IsInvincible = false;
        private const int _touchAreaLength = 10;
        public Vector2 HitPosition;
        private float _elapsedInvincibleTime = 0;
        private const float _INVINCIBLE_TIME = 1.5f;

        public SwordHitbox SwordHitbox;
            
        public Player()
        {
            Interactable = false;
            Hittable = false;
            Colliding = true;
            Moving = true;
            WalkSpeed = 70f;
            SpriteOffset = new Vector2(-13, -24);

            var texture = StaticData.Content.Load<Texture2D>("linktothepast/link-sprites");

            Dictionary<string, Animation> animations = new Dictionary<string, Animation>()
            {
                { "IdleDown",       new Animation(texture, 1, 0.04f, false, 0, 1) },
                { "IdleUp",         new Animation(texture, 1, 0.04f, false, 1, 3) },
                { "IdleLeft",       new Animation(texture, 1, 0.04f, false, 2, 3) },
                { "IdleRight",      new Animation(texture, 1, 0.04f, false, 3, 3) },
                { "WalkDown",       new Animation(texture, 8, 0.04f, true, 0) },
                { "WalkUp",         new Animation(texture, 8, 0.04f, true, 1) },
                { "WalkLeft",       new Animation(texture, 6, 0.04f, true, 2) },
                { "WalkRight",      new Animation(texture, 6, 0.04f, true, 3) },
                { "SwordHitDown",   new Animation(texture, 6, 0.04f, false, 4) },
                { "SwordHitUp",     new Animation(texture, 5, 0.04f, false, 5) },
                { "SwordHitLeft",   new Animation(texture, 5, 0.04f, false, 6) },
                { "SwordHitRight",  new Animation(texture, 5, 0.04f, false, 7) }
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
                { "TakenDamage", new PlayerStateTakenDamage(this) }
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
                }
            }
            
            if (SwordHitbox.Enabled)
            {
                SwordHitbox.Update(gameTime);
                
                foreach (var entity in StaticData.Scene.HittableEntities)
                {
                    if (SwordHitbox.Rectangle.Intersects(entity.Hitbox.Rectangle))
                    {
                        entity.InvokeTrigger();
                    }
                }
            }
            else if (StaticData.Scene.TileMap.Exits.ContainsKey(MapBorder))
            {
                StaticData.SceneManager.GoTo(StaticData.Scene.TileMap.Exits[MapBorder]);
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            if (!IsInvincible || _elapsedInvincibleTime < 0.5f || Math.Round((decimal)_elapsedInvincibleTime, 1) % 0.2M != 0)
            {
                base.Draw(spriteBatch, offset);
            }
        }

        public void TakeDamage(Vector2 hitPosition)
        {
            if (!IsInvincible)
            {
                IsInvincible = true;
                HitPosition = hitPosition;
                StateMachine.TransitionTo("TakenDamage");
            }
        }

        public void DetermineInputVelocity()
        {
            Velocity = Vector2.Zero;

            if (Input.IsPressed(Input.Up))
                Velocity.Y = -1;
            if (Input.IsPressed(Input.Down))
                Velocity.Y = 1;
            if (Input.IsPressed(Input.Left))
                Velocity.X = -1;
            if (Input.IsPressed(Input.Right))
                Velocity.X = 1;
            
            if (Velocity != Vector2.Zero)
            {
                //MapEntity.Velocity.Normalize();
                Velocity *= WalkSpeed;
            }
        }

        public void DetermineHitInput()
        {
            if (!Input.IsPressed(Input.A))
                return;

            if (Input.JustPressed(Input.A))
            {
                var touchArea = GetTouchArea();

                foreach (var mapEntity in StaticData.Scene.InteractableEntities)
                {
                    if (touchArea.Intersects(mapEntity.Hitbox.Rectangle))
                    {
                        mapEntity.InvokeTrigger();
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
