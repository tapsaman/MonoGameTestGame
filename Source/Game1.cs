using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGameTestGame.Controls;
using MonoGameTestGame.Managers;
using MonoGameTestGame.Manangers;
using MonoGameTestGame.Models;
using MonoGameTestGame.Sprites;

namespace MonoGameTestGame
{
    public class Game1 : Game
    {
        private static SpriteBatch _spriteBatch;
        private DialogManager _dialogManager;
        private SceneManager _sceneManager;
        private HUD _hud;

        public Game1()
        {
            Content.RootDirectory = "Content";
            StaticData.TiledProjectDirectory = "Content\\TiledProjects\\testmap";
            StaticData.Graphics = new GraphicsDeviceManager(this);
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Rendering.Init(GraphicsDevice);

            Window.AllowUserResizing = true;
            Window.AllowAltF4 = true;
            Window.Title = "hello";

            IsMouseVisible = true;

            base.Initialize();
        }

        private Button _startButton;

        protected override void LoadContent()
        {
            StaticData.Content = Content;
            StaticData.SpriteBatch = _spriteBatch = new SpriteBatch(GraphicsDevice);
            StaticData.Font = Content.Load<SpriteFont>("Fonts/TestFont");
            StaticData.TileTexture = Content.Load<Texture2D>("TiledProjects/testmap/linktothepast-tiles");
            StaticData.ObjectTexture = Content.Load<Texture2D>("TiledProjects/testmap/linktothepast-objects");
            BitmapFontRenderer.Font = new BitmapFont.LinkToThePast();
            SFX.Load();
            Shaders.Load();
            StaticData.SceneManager = _sceneManager = new SceneManager();
            _sceneManager.Init(new TestScene());
            _dialogManager = new DialogManager();
            _hud = new HUD();
            _hud.Load();

            _startButton = new Button(Content.Load<Texture2D>("Button"), Content.Load<SpriteFont>("Fonts/TestFont"))
            {
                Text = "START"
            };
            _startButton.Click += StartButton_Click;
            
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            Start();
        }

        private void Start()
        {
            started = true;
            _dialogManager.Load(new Dialog("terve", "mitä äijä?"));
            SFX.MessageFinish.Play();
            _sceneManager.CurrentScene.Start();
            _startButton = null;
            //Rendering.ApplyPostEffect(Shaders.Noise);
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        private Vector2 _titlePosition = new Vector2(StaticData.NativeWidth, 0);
        private bool started = false;

        protected override void Update(GameTime gameTime)
        {
            Input.P1.Update();

            if (!started)
            {
                _startButton.Update(gameTime);
                
                if (Input.P1.JustPressed(Input.P1.Start)) {
                    Start();
                }

                return;
            }

            _sceneManager.Update(gameTime);
            _dialogManager.Update(gameTime);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _titlePosition.X -= (float)gameTime.ElapsedGameTime.TotalSeconds * 15;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {            
            Rendering.Start(GraphicsDevice);

            _sceneManager.Draw(_spriteBatch);

            if (!started)
            {
                _startButton.Draw(gameTime, _spriteBatch);
            }
            
            _hud.Draw(_spriteBatch, _sceneManager.Player);
            _dialogManager.Draw(_spriteBatch);
            BitmapFontRenderer.DrawString(_spriteBatch, "zeldan seikkailut mikä mikä maassa vittu", _titlePosition);
            
            Rendering.End(GraphicsDevice, gameTime);

            base.Draw(gameTime);
        }
    }
}
