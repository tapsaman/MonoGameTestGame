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
    public class Player : Component
    {
        public Vector2 Velocity;
        public float Speed = 60f;
        public bool Hit = false;

        public Sprite Sprite;
        public StateMachine StateMachine;

        public string Direction = "Down";

        public Input Input;

            
        public Player(Texture2D playerTexture)
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

            Sprite = new Sprite(animations)
            {
                Position = new Vector2(100, 100),
                Input = new Input()
                {
                    Up = Keys.W,
                    Right = Keys.D,
                    Down = Keys.S,
                    Left = Keys.A
                }
            };

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
            Sprite.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch);
        }

        public void DetermineInputVelocity()
        {
            Velocity = Vector2.Zero;

            if (Keyboard.GetState().IsKeyDown(Input.Up))
                Velocity.Y = -Speed;
            if (Keyboard.GetState().IsKeyDown(Input.Down))
                Velocity.Y = Speed;
            if (Keyboard.GetState().IsKeyDown(Input.Left))
                Velocity.X = -Speed;
            if (Keyboard.GetState().IsKeyDown(Input.Right))
                Velocity.X = Speed;
        }

        public void DetermineHitInput()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
                Hit = true;
        }

        public void Move(GameTime gameTime)
        {
            Sprite.Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Velocity.Y < 0)
                Direction = "Up";
            if (Velocity.X > 0)
                Direction = "Right";
            if (Velocity.Y > 0)
                Direction = "Down";
            if (Velocity.X < 0)
                Direction = "Left";
        }
    }
}
