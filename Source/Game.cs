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
using TapsasEngine;

namespace ZA6
{
    public class ZeldaAdventure666 : Game
    {
        public RenderStateMachine StateMachine;
        public bool WindowIsActive { get; private set; } = true;
        public HUD Hud;
        public Animations.TitleText TitleText;
        private GraphicsDeviceManager _graphicsDeviceManager;

        public ZeldaAdventure666()
        {
            _graphicsDeviceManager = new GraphicsDeviceManager(this);

            //_graphicsDeviceManager.PreparingDeviceSettings += new EventHandler<PreparingDeviceSettingsEventArgs>(SetToPreserve);

            if (Static.PauseOnWindowDeactive)
            {
                Deactivated += DectivateWindow;
                Activated += ActivateWindow;
            }
        }

        /*private void SetToPreserve(object sender, PreparingDeviceSettingsEventArgs eventargs)
        {
            eventargs.GraphicsDeviceInformation.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;
        }*/
    
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

            Input.DetectGamePadController();

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
            
            Static.GameTime = gameTime;
            Tengine.GameTime = gameTime;
            Tengine.Delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Input.Update();
            Static.DevUtils.Update(gameTime);
            StateMachine.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Tengine.DrawCount++;
            StateMachine.Draw(Static.SpriteBatch);

            base.Draw(gameTime);
        }

        private void LoadGlobals()
        {
            Tengine.Content = Content;

            Static.Game = this;
            Static.SpriteBatch = Static.Renderer.SpriteBatch;
            Static.Renderer.Init(Static.NativeWidth, Static.NativeHeight, Static.DefaultResolution);
            Static.Content = Content;
            Static.Font = Content.Load<SpriteFont>("Fonts/TestFont");
            Static.FontSmall = Content.Load<SpriteFont>("Fonts/TestFontSmall");
            Static.FontLarge = Content.Load<SpriteFont>("Fonts/TestFontLarge");
            Static.World = new TiledWorld("Content\\TiledProject", "ZA6.world");
            Static.SceneManager = new SceneManager()
            {
                World = Static.World
            };
            Static.EventSystem = new EventSystem();
            Static.DialogManager = new DialogManager();
            SFX.Load();
            Shaders.Load();
            Img.Load();
            Songs.Load();
            BitmapFontRenderer.Font = new BitmapFont.LinkToThePast();
            BitmapFontRenderer.FontEffects = new Effect[]
            {
                null,
                Shaders.Highlight,
                Shaders.Rainbow
            };
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

            var buttonTexture = Static.Content.Load<Texture2D>("UI/Button");
            var disabledButtonTexture = Static.Content.Load<Texture2D>("UI/DisabledButton");
            UIComponent.DefaultBackground = new SectionedSprite(buttonTexture, 2);
            UIComponent.DefaultDisabledBackground = new SectionedSprite(disabledButtonTexture, 2);

            TitleText = new Animations.TitleText();
            
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
                { "Cartoon", new GameStateCartoon(this) },
                { "Stopped", new GameStateStopped(this) },
            };

            StateMachine = new RenderStateMachine(states, "StartMenu");

            if (false && Static.Debug)
            {
                SaveData.LoadAndApply();
            }
        }

        private void QuitDialogState()
        {
            StateMachine.TransitionTo("Default");
        }

        private void DectivateWindow(object sendet, EventArgs args)
        {
            WindowIsActive = false;
            Music.Pause(this);
        }

        private void ActivateWindow(object sendet, EventArgs args)
        {
            WindowIsActive = true;
            Music.Resume(this);
        }
    }
}
