using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
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
        private DialogManager _dialogManager;
        private Scene _scene;

        public Game1()
        {
            StaticData.Graphics = _graphics = new GraphicsDeviceManager(this);
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
            StaticData.Font = Content.Load<SpriteFont>("Fonts/TestFont");
            SFX.Load();
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            StaticData.Scene = _scene = new TestScene();
            _scene.Load();
            _dialogManager = new DialogManager();
            _dialogManager.Load(new Dialog("terve", "mitä äijä?"));

            // TODO: use this.Content to load your game content here
            ballTexture = Content.Load<Texture2D>("ball");

            var quitButton = new Button(Content.Load<Texture2D>("Button"), Content.Load<SpriteFont>("Fonts/TestFont"))
            {
                Position = new Vector2(350, 285),
                Text = "Quit"
            };

            quitButton.Click += QuitButton_Click;

            _gameComponents = new List<Component>()
            {
                quitButton
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
            Input.Update();
            _scene.Update(gameTime);
            _dialogManager.Update(gameTime);

            foreach (var component in _gameComponents)
            {
                component.Update(gameTime);
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //_spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Matrix.CreateScale(1f));
            _spriteBatch.Begin();
            _scene.Draw(_spriteBatch);
            //_spriteBatch.Draw(ballTexture, ballPosition, Color.White);
            /*_spriteBatch.Draw(
                ballTexture,
                ballPosition,
                null,
                Color.White,
                0f,
                new Vector2(ballTexture.Width / 2, ballTexture.Height / 2),
                Vector2.One,
                SpriteEffects.None,
                0f
            );*/
            foreach (var component in _gameComponents)
            {
                component.Draw(gameTime, _spriteBatch);
            }
            
            _dialogManager.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
