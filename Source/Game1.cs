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
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private DialogManager _dialogManager;
        private RenderTarget2D _nativeRenderTarget; 
        private Rectangle _actualScreenRectangle;
        private SceneManager _sceneManager;

        public Game1()
        {
            StaticData.Graphics = _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            StaticData.TiledProjectDirectory = "Content\\TiledProjects\\testmap";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _nativeRenderTarget = new RenderTarget2D(GraphicsDevice, StaticData.NativeWidth, StaticData.NativeHeight);
            _actualScreenRectangle = new Rectangle(0, 0, StaticData.BackBufferWidth, StaticData.BackBufferHeight);

            _graphics.PreferredBackBufferWidth = StaticData.BackBufferWidth;
            _graphics.PreferredBackBufferHeight = StaticData.BackBufferHeight;
            //_graphics.IsFullScreen = true;
            _graphics.ApplyChanges();

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
            StaticData.Font = Content.Load<SpriteFont>("Fonts/TestFont");
            StaticData.TileTexture = Content.Load<Texture2D>("TiledProjects/testmap/linktothepast-tiles");
            StaticData.ObjectTexture = Content.Load<Texture2D>("TiledProjects/testmap/linktothepast-objects");
            SFX.Load();
            BitmapFontRenderer.Font = new BitmapFont.LinkToThePast();
            StaticData.SceneManager = _sceneManager = new SceneManager();
            Player player = new Player() { Position = new Vector2(100, 100) };
            _sceneManager.Player = player;
            _sceneManager.CurrentScene = StaticData.Scene = new TestScene(player);
            StaticData.SpriteBatch = _spriteBatch = new SpriteBatch(GraphicsDevice);
            _dialogManager = new DialogManager();

            _startButton = new Button(Content.Load<Texture2D>("Button"), Content.Load<SpriteFont>("Fonts/TestFont"))
            {
                Text = "START"
            };
            _startButton.Click += StartButton_Click;
            
            //StaticData.Content = null;
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
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        private Vector2 _titlePosition = new Vector2(StaticData.NativeWidth, 0);
        private bool started = false;

        protected override void Update(GameTime gameTime)
        {
            Input.Update();

            if (!started)
            {
                _startButton.Update(gameTime);
                
                if (Input.JustPressed(Input.Start)) {
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
            GraphicsDevice.SetRenderTarget(_nativeRenderTarget);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //_spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Matrix.CreateScale(1f));
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            _sceneManager.Draw(_spriteBatch);

            if (!started)
            {
                _startButton.Draw(gameTime, _spriteBatch);
            }
            
            _dialogManager.Draw(_spriteBatch);
            BitmapFontRenderer.DrawString(_spriteBatch, "zeldan seikkailut mikä mikä maassa vittu", _titlePosition);
            _spriteBatch.End();

            // after drawing the game at native resolution we can render _nativeRenderTarget to the backbuffer!
            // First set the GraphicsDevice target back to the backbuffer
            GraphicsDevice.SetRenderTarget(null);
            // RenderTarget2D inherits from Texture2D so we can render it just like a texture
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            _spriteBatch.Draw(_nativeRenderTarget, _actualScreenRectangle, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
