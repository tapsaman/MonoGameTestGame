using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TapsasEngine.Sprites;
using TapsasEngine.Utilities;
using ZA6.UI;
using ZA6.Managers;
using ZA6.Manangers;
using ZA6.Models;
using ZA6.Utilities;
using System;
using Microsoft.Xna.Framework.Media;

namespace ZA6
{
    public class ZeldaAdventure666 : Game
    {
        public RenderStateMachine StateMachine;
        public bool WindowIsActive { get; private set; } = true;
        public HUD Hud;
        public Animations.TitleText TitleText;
        private GraphicsDeviceManager _graphicsDeviceManager;
        private bool _deactivationPausedMusic;

        public ZeldaAdventure666()
        {
            _graphicsDeviceManager = new GraphicsDeviceManager(this);
            Deactivated += DectivateWindow;
            Activated += ActivateWindow;
        }

        protected override void Initialize()
        {
            Content.RootDirectory = "Content";
            //Window.AllowUserResizing = true;
            Window.AllowAltF4 = true;
            Window.Title = "Zelda Adventure 666";
            Window.IsBorderless = false;
            IsMouseVisible = true;

            Static.Renderer = new GameRenderer(
                GraphicsDevice,
                _graphicsDeviceManager
            );

            base.Initialize();
        }

        protected override void LoadContent()
        {
            LoadGlobals();
            InitGlobals();
        }

        protected override void Update(GameTime gameTime)
        {
            if (!WindowIsActive)
                return;
            
            Input.P1.Update();
            Static.DevUtils.Update(gameTime);
            StateMachine.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            StateMachine.Draw(Static.SpriteBatch);

            base.Draw(gameTime);
        }

        private void LoadGlobals()
        {
            Static.Game = this;
            Static.SpriteBatch = Static.Renderer.SpriteBatch;
            Static.Renderer.Init(Static.NativeWidth, Static.NativeHeight, Static.DefaultResolution);
            Static.Content = Content;
            Static.Font = Content.Load<SpriteFont>("Fonts/TestFont");
            Static.FontSmall = Content.Load<SpriteFont>("Fonts/TestFontSmall");
            Static.World = new TiledWorld("Content\\TiledProject", "ZA6.world");
            Static.SceneManager = new SceneManager()
            {
                World = Static.World
            };
            Static.EventSystem = new EventSystem();
            Static.DialogManager = new DialogManager();
            BitmapFontRenderer.Font = new BitmapFont.LinkToThePast();
            SFX.Load();
            Shaders.Load();
            Img.Load();
            Static.DevUtils = new DevUtils();
            Static.GameData = new DataStore();
            Static.SessionData = new DataStore();
            Hud = new HUD();
            SavedConfig.LoadAndApply();
        }

        private void InitGlobals()
        {
            InputController.Renderer = Static.Renderer;

            Button.ClickSound = SFX.MessageFinish;
            Select<RenderResolution>.ChangeSound = SFX.ChestOpen;
            Slider.ChangeSound = SFX.ChestOpen;

            var buttonTexture = Static.Content.Load<Texture2D>("Button");
            var disabledButtonTexture = Static.Content.Load<Texture2D>("DisabledButton");
            UIComponent.DefaultBackground = new SectionedSprite(buttonTexture, 2);
            UIComponent.DefaultDisabledBackground = new SectionedSprite(disabledButtonTexture, 2);

            TitleText = new Animations.TitleText();

            //Static.SceneManager.Init();
            
            Dictionary<string, State> states = new Dictionary<string, State>()
            {
                { "StartMenu", new GameStateStartMenu(this) },
                { "Intro", new GameStateIntro(this) },
                { "StartOver", new GameStateStartOver(this) },
                { "Default", new GameStateDefault(this) },
                { "MainMenu", new GameStateMainMenu(this) },
                { "Dialog", new GameStateDialog(this) },
                { "Cutscene", new GameStateCutscene(this) },
                { "GameOver", new GameStateGameOver(this) },
            };

            StateMachine = new RenderStateMachine(states, "StartMenu");
        }

        private void QuitDialogState()
        {
            StateMachine.TransitionTo("Default");
        }

        private void DectivateWindow(object sendet, EventArgs args)
        {
            WindowIsActive = false;

            if (MediaPlayer.State == MediaState.Playing)
            {
                _deactivationPausedMusic = true;
                MediaPlayer.Pause();
            }
        }

        private void ActivateWindow(object sendet, EventArgs args)
        {
            WindowIsActive = true;

            if (_deactivationPausedMusic)
            {
                MediaPlayer.Resume();
            }
        }
    }
}
