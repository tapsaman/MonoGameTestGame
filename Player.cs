using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTestGame.Managers;
using MonoGameTestGame.Models;
using MonoGameTestGame.Sprites;

namespace MonoGameTestGame
{
    public class Player
    {
        public float WalkSpeed = 60f;
        public bool Hit = false;

        private StateMachine _stateMachine;

        public MapEntity MapEntity;
        public SwordHitbox SwordHitbox;
            
        public Player()
        {
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

            Hitbox hitbox = new Hitbox(14, 14);
            Sprite sprite = new Sprite(animations);

            MapEntity = new MapEntity(sprite, hitbox)
            {
                Position = new Vector2(100, 100),
                Moving = true,
                SpriteOffset = new Vector2(-13, -24)
            };

            SwordHitbox = new SwordHitbox(14, 14) { Color = Color.Black };

            Dictionary<string, State> states = new Dictionary<string, State>()
            {
                { "Idle", new PlayerStateIdle(this) },
                { "Walking", new PlayerStateWalking(this) },
                { "SwordHit", new PlayerStateSwordHit(this) }
            };

            _stateMachine = new StateMachine(states, "Idle");
        }


        public void Update(GameTime gameTime)
        {
            _stateMachine.Update(gameTime);
            SwordHitbox.Update(gameTime);
        }

        public void DetermineInputVelocity(GameTime gameTime)
        {
            if (Input.IsPressed(Input.Up))
                MapEntity.Velocity.Y = -1;
            if (Input.IsPressed(Input.Down))
                MapEntity.Velocity.Y = 1;
            if (Input.IsPressed(Input.Left))
                MapEntity.Velocity.X = -1;
            if (Input.IsPressed(Input.Right))
                MapEntity.Velocity.X = 1;
            
            if (MapEntity.Velocity != Vector2.Zero)
            {
                //MapEntity.Velocity.Normalize();
                MapEntity.Velocity *= WalkSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        public void DetermineHitInput()
        {
            if (!Input.IsPressed(Input.A))
                return;

            if (Input.JustPressed(Input.A))
            {
                var touchArea = MapEntity.GetTouchArea();

                foreach (var mapEntity in StaticData.Scene.MapEntities)
                {
                    if (mapEntity.Hitbox != null && touchArea.Intersects(mapEntity.Hitbox.Rectangle) && mapEntity.HasTrigger())
                    {
                        mapEntity.InvokeTrigger();
                        return;
                    }
                }
                Hit = true;
            }
            else
            {
                //Hit = true;
            } 
        }
    }
}
