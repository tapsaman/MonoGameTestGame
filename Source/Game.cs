using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TapsasEngine.Utilities;
using ZA6.Controls;
using ZA6.Managers;
using ZA6.Manangers;
using ZA6.Models;
using ZA6.Utilities;

namespace ZA6
{
    public class ZeldaAdventure666 : Game
    {
        public RenderStateMachine StateMachine;
        public HUD Hud;
        public Animations.TitleText TitleText;
        private GraphicsDeviceManager _graphicsDeviceManager;

        public ZeldaAdventure666()
        {
            _graphicsDeviceManager = new GraphicsDeviceManager(this);
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
        }

        private void InitGlobals()
        {
            InputController.Renderer = Static.Renderer;
            Button.ClickSound = SFX.MessageFinish;
            Select<RenderResolution>.ChangeSound = SFX.ChestOpen;
            Slider.ChangeSound = SFX.ChestOpen;

            TitleText = new Animations.TitleText();

            //Static.SceneManager.Init();
            
            Dictionary<string, State> states = new Dictionary<string, State>()
            {
                { "StartMenu", new GameStateStartMenu(this) },
                { "MainMenu", new GameStateMainMenu(this) },
                { "Default", new GameStateDefault(this) },
                { "Dialog", new GameStateDialog(this) },
                { "GameOver", new GameStateGameOver(this) },
                { "StartOver", new GameStateStartOver(this) },
                { "Cutscene", new GameStateCutscene(this) }
            };

            StateMachine = new RenderStateMachine(states, "StartMenu");
        }

        private void QuitDialogState()
        {
            StateMachine.TransitionTo("Default");
        }
    }
}
