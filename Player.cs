using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameTestGame.Controls;
using MonoGameTestGame.Managers;
using MonoGameTestGame.Models;
using MonoGameTestGame.Sprites;

namespace MonoGameTestGame
{
    public class Player
    {
        public float WalkSpeed = 60f;
        public bool Hit = false;

        public StateMachine StateMachine;

        public Input Input;
        public MapEntity MapEntity;
        public SwordHitbox SwordHitbox;

            
        public Player(Texture2D playerTexture, GraphicsDeviceManager graphics)
        {
            Input = new Input()
            {
                Up = Keys.W,
                Right = Keys.D,
                Down = Keys.S,
                Left = Keys.A
            };

            Dictionary<string, Animation> animations = new Dictionary<string, Animation>()
            {
                { "IdleUp", new Animation(playerTexture, 1, 4, 3) },
                { "IdleRight", new Animation(playerTexture, 1, 4, 12) },
                { "IdleDown", new Animation(playerTexture, 1, 0, 1) },
                { "IdleLeft", new Animation(playerTexture, 1, 1, 12) },
                { "WalkUp", new Animation(playerTexture, 8, 4) },
                { "WalkRight", new Animation(playerTexture, 6, 4, 8) },
                { "WalkDown", new Animation(playerTexture, 8, 1) },
                { "WalkLeft", new Animation(playerTexture, 6, 1, 8) },
                { "SwordHitUp", new Animation(playerTexture, 5, 6, 0, false, 0.04f) },
                { "SwordHitRight", new Animation(playerTexture, 5, 6, 8, false, 0.04f) },
                { "SwordHitDown", new Animation(playerTexture, 6, 3, 0, false, 0.04f) },
                { "SwordHitLeft", new Animation(playerTexture, 5, 3, 8, false, 0.04f) },
            };

            Sprite sprite = new Sprite(animations);
            Hitbox hitbox = new Hitbox(graphics, 14, 14);

            MapEntity = new MapEntity(sprite, hitbox)
            {
                Position = new Vector2(100, 100),
                Moving = true
            };

            SwordHitbox = new SwordHitbox(graphics, 18, 8) { Color = Color.Black };

            Dictionary<string, State> states = new Dictionary<string, State>()
            {
                { "Idle", new PlayerStateIdle(this) },
                { "Walking", new PlayerStateWalking(this) },
                { "SwordHit", new PlayerStateSwordHit(this) }
            };

            StateMachine = new StateMachine(states, "Idle");
        }


        public void Update(GameTime gameTime)
        {
            StateMachine.Update(gameTime);
            SwordHitbox.Update(gameTime);
        }

        public void DetermineInputVelocity(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Input.Up))
                MapEntity.Velocity.Y = -WalkSpeed;
            if (Keyboard.GetState().IsKeyDown(Input.Down))
                MapEntity.Velocity.Y = WalkSpeed;
            if (Keyboard.GetState().IsKeyDown(Input.Left))
                MapEntity.Velocity.X = -WalkSpeed;
            if (Keyboard.GetState().IsKeyDown(Input.Right))
                MapEntity.Velocity.X = WalkSpeed;
            
            MapEntity.Velocity *= (float)gameTime.ElapsedGameTime.TotalSeconds;
            //MapEntity.Velocity.Normalize();
        }

        public void DetermineHitInput()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
                Hit = true;
        }
    }
}
