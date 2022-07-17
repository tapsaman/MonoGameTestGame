using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTestGame.Managers;
using MonoGameTestGame.Models;
using MonoGameTestGame.Sprites;

namespace MonoGameTestGame
{
    public class Player : MapEntity
    {
        public bool Hitting = false;
        private float _walkSpeed = 60f;
        private const int _touchAreaLength = 10;

        public SwordHitbox SwordHitbox;
            
        public Player(Vector2 position)
            : base(position)
        {
            Interactable = false;
            Hittable = false;
            Colliding = true;
            Moving = true;
            SpriteOffset = new Vector2(-13, -24);

            var texture = StaticData.Content.Load<Texture2D>("linktothepast-spritesheet");

            Dictionary<string, Animation> animations = new Dictionary<string, Animation>()
            {
                { "IdleDown",       new Animation(texture, 1, 0, 1) },
                { "IdleUp",         new Animation(texture, 1, 1, 3) },
                { "IdleLeft",       new Animation(texture, 1, 2, 3) },
                { "IdleRight",      new Animation(texture, 1, 3, 3) },
                { "WalkDown",       new Animation(texture, 8, 0) },
                { "WalkUp",         new Animation(texture, 8, 1) },
                { "WalkLeft",       new Animation(texture, 6, 2) },
                { "WalkRight",      new Animation(texture, 6, 3) },
                { "SwordHitDown",   new Animation(texture, 6, 4, 0, false, 0.04f) },
                { "SwordHitUp",     new Animation(texture, 5, 5, 0, false, 0.04f) },
                { "SwordHitLeft",   new Animation(texture, 5, 6, 0, false, 0.04f) },
                { "SwordHitRight",  new Animation(texture, 5, 7, 0, false, 0.04f) },
            };

            Hitbox.Load(14, 14);
            Sprite.SetAnimations(animations);

            SwordHitbox = new SwordHitbox(14, 14) { Color = Color.Black };

            Dictionary<string, State> states = new Dictionary<string, State>()
            {
                { "Idle", new PlayerStateIdle(this) },
                { "Walking", new PlayerStateWalking(this) },
                { "SwordHit", new PlayerStateSwordHit(this) }
            };

            StateMachine = new StateMachine(states, "Idle");
        }


        public override void Update(GameTime gameTime)
        {
            StateMachine.Update(gameTime);
            SwordHitbox.Update(gameTime);
            base.Update(gameTime);
        }

        public void DetermineInputVelocity(GameTime gameTime)
        {
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
                Velocity *= _walkSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
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
            }
            else
            {
                //Hit = true;
            } 
        }

        private Rectangle GetTouchArea()
        {
            if (Direction == Direction.Up)
                return new Rectangle(Hitbox.Rectangle.Left, Hitbox.Rectangle.Top - _touchAreaLength, Hitbox.Rectangle.Width, _touchAreaLength);
            if (Direction == Direction.Right)
                return new Rectangle(Hitbox.Rectangle.Right, Hitbox.Rectangle.Top, _touchAreaLength, Hitbox.Rectangle.Height);
            if (Direction == Direction.Down)
                return new Rectangle(Hitbox.Rectangle.Left, Hitbox.Rectangle.Bottom, Hitbox.Rectangle.Width, _touchAreaLength);
            
            return new Rectangle(Hitbox.Rectangle.Left - _touchAreaLength, Hitbox.Rectangle.Top, _touchAreaLength, Hitbox.Rectangle.Height);
        }
    }
}
