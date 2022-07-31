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
    public class ZeldaAdventure666 : Game
    {
        public RenderStateMachine StateMachine;
        public SceneManager SceneManager;
        public DialogManager DialogManager;
        public HUD Hud;
        public Vector2 TitlePosition;
        private static SpriteBatch _spriteBatch;

        public ZeldaAdventure666()
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

        protected override void LoadContent()
        {
            StaticData.Game = this;
            StaticData.Content = Content;
            StaticData.SpriteBatch = _spriteBatch = new SpriteBatch(GraphicsDevice);
            StaticData.Font = Content.Load<SpriteFont>("Fonts/TestFont");
            StaticData.TileTexture = Content.Load<Texture2D>("TiledProjects/testmap/linktothepast-tiles");
            StaticData.ObjectTexture = Content.Load<Texture2D>("TiledProjects/testmap/linktothepast-objects");
            BitmapFontRenderer.Font = new BitmapFont.LinkToThePast();
            SFX.Load();
            Shaders.Load();
            StaticData.SceneManager = SceneManager = new SceneManager();
            TitlePosition = new Vector2(StaticData.NativeWidth, 1);

            SceneManager.Init(new TestScene());
            DialogManager = new DialogManager();
            DialogManager.DialogEnd += QuitDialog;
            Hud = new HUD();
            Hud.Load();

            Dictionary<string, State> states = new Dictionary<string, State>()
            {
                { "MainMenu", new GameStateMainMenu(this) },
                { "Default", new GameStateDefault(this) },
                { "Dialog", new GameStateDialog(this) }
            };

            StateMachine = new RenderStateMachine(states, "MainMenu");
        }

        private void QuitDialog()
        {
            StateMachine.TransitionTo("Default");
        }

        protected override void Update(GameTime gameTime)
        {
            Input.P1.Update();
            StateMachine.Update(gameTime);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            StateMachine.Draw(_spriteBatch, gameTime);
            base.Draw(gameTime);
        }
    }
}
