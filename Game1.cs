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
    public class Game1 : Game
    {
        Texture2D ballTexture;
        Vector2 ballPosition;
        float ballSpeed;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private List<Component> _gameComponents;
        private MapEntity[] _mapEntities;
        private TileMap _tileMap;
        private Player _player;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            ballPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
            ballSpeed = 250f;

            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 600;
            //_graphics.IsFullScreen = true;
            _graphics.ApplyChanges();

            Window.AllowUserResizing = true;
            Window.AllowAltF4 = true;
            Window.Title = "hello";

            IsMouseVisible = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            StaticData.Content = Content;

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _tileMap = StaticData.TileMap = new ZeldaMap();

            // TODO: use this.Content to load your game content here
            ballTexture = Content.Load<Texture2D>("ball");

            var playerTexture_ = Content.Load<Texture2D>("rm2k-sheet01");
            var playerTexture = Content.Load<Texture2D>("linktothepast-spritesheet");

            _player = new Player(playerTexture, _graphics);
            Enemy enemy = new Enemy(playerTexture, _graphics);

            StaticData.Font = Content.Load<SpriteFont>("Fonts/TestFont");
            /*Dictionary<string, Animation> animations = new Dictionary<string, Animation>()
            {
                { "WalkUp", new Animation(playerTexture, 8, 4) },
                { "WalkRight", new Animation(playerTexture, 6, 4, 8) },
                { "WalkDown", new Animation(playerTexture, 8, 1) },
                { "WalkLeft", new Animation(playerTexture, 6, 1, 8) },
                { "SwordDown", new Animation(playerTexture, 6, 3, 0, false) },
            };

            _sprites = new List<Sprite>()
            {
                new Sprite(animations)
                {
                    Position = new Vector2(100, 100),
                    Input = new Input()
                    {
                        Up = Keys.W,
                        Right = Keys.D,
                        Down = Keys.S,
                        Left = Keys.A
                    }
                },
                new Sprite(animations)
                {
                    Position = new Vector2(150, 100),
                    Input = new Input()
                    {
                        Up = Keys.Up,
                        Right = Keys.Right,
                        Down = Keys.Down,
                        Left = Keys.Left
                    }
                }
            };*/

            var quitButton = new Button(Content.Load<Texture2D>("Button"), Content.Load<SpriteFont>("Fonts/TestFont"))
            {
                Position = new Vector2(350, 200),
                Text = "Quit"
            };

            quitButton.Click += QuitButton_Click;

            _gameComponents = new List<Component>()
            {
                quitButton
            };
            _mapEntities = new MapEntity[]
            {
                enemy.MapEntity,
                _player.MapEntity
            };

            StaticData.Content = null;
        }

        private void QuitButton_Click(object sender, EventArgs e)
        {
            Exit();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        private void MoveBall(GameTime gameTime)
        {
            // TODO: Add your update logic here
            var kstate = Keyboard.GetState();

            if (kstate.IsKeyDown(Keys.Up))
                ballPosition.Y -= ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(kstate.IsKeyDown(Keys.Down))
                ballPosition.Y += ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (kstate.IsKeyDown(Keys.Left))
                ballPosition.X -= ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(kstate.IsKeyDown(Keys.Right))
                ballPosition.X += ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(ballPosition.X > _graphics.PreferredBackBufferWidth - ballTexture.Width / 2)
                ballPosition.X = _graphics.PreferredBackBufferWidth - ballTexture.Width / 2;
            else if(ballPosition.X < ballTexture.Width / 2)
                ballPosition.X = ballTexture.Width / 2;

            if(ballPosition.Y > _graphics.PreferredBackBufferHeight - ballTexture.Height / 2)
                ballPosition.Y = _graphics.PreferredBackBufferHeight - ballTexture.Height / 2;
            else if(ballPosition.Y < ballTexture.Height / 2)
                ballPosition.Y = ballTexture.Height / 2;
        }

        protected override void Update(GameTime gameTime)
        {
            _player.Update(gameTime);

            foreach (var component in _gameComponents)
            {
                component.Update(gameTime);
            }
            foreach (var mapEntity in _mapEntities)
            {
                mapEntity.Update(gameTime, _mapEntities, _tileMap);
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _tileMap.Draw(_spriteBatch);
            //_spriteBatch.Draw(ballTexture, ballPosition, Color.White);
            _spriteBatch.Draw(
                ballTexture,
                ballPosition,
                null,
                Color.White,
                0f,
                new Vector2(ballTexture.Width / 2, ballTexture.Height / 2),
                Vector2.One,
                SpriteEffects.None,
                0f
            );
            foreach (var component in _gameComponents)
            {
                component.Draw(gameTime, _spriteBatch);
            }
            foreach (var mapEntity in _mapEntities)
            {
                mapEntity.Draw(_spriteBatch);
            }
            _player.SwordHitbox.Draw(_spriteBatch);
            _spriteBatch.DrawString(StaticData.Font, _player.MapEntity.Position.ToString(), new Vector2(500, 300), Color.Black);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
