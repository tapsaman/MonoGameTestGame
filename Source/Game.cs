using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ZA6.Controls;
using ZA6.Managers;
using ZA6.Manangers;
using ZA6.Models;

namespace ZA6
{
    public class ZeldaAdventure666 : Game
    {
        public RenderStateMachine StateMachine;
        public HUD Hud;
        public Vector2 TitlePosition;

        public ZeldaAdventure666()
        {
            Static.Graphics = new GraphicsDeviceManager(this);
        }

        protected override void Initialize()
        {
            Content.RootDirectory = "Content";
            //Window.AllowUserResizing = true;
            Window.AllowAltF4 = true;
            Window.Title = "Zelda Adventure 666";
            Window.IsBorderless = false;
            IsMouseVisible = true;

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
            StateMachine.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            StateMachine.Draw(Static.SpriteBatch, gameTime);

            base.Draw(gameTime);
        }

        private void LoadGlobals()
        {
            Static.Game = this;
            Static.Content = Content;
            Static.TiledProjectDirectory = "Content\\TiledProjects\\testmap";
            Static.Font = Content.Load<SpriteFont>("Fonts/TestFont");
            Static.Renderer = new Renderer();
            Static.Renderer.Init(GraphicsDevice);
            Static.SceneManager = new SceneManager();
            Static.EventSystem = new EventSystem();
            Static.DialogManager = new DialogManager();
            //Static.DialogManager.DialogEnd += QuitDialogState;
            BitmapFontRenderer.Font = new BitmapFont.LinkToThePast();
            SFX.Load();
            Shaders.Load();
            Img.Load();
            Static.SpriteBatch = new SpriteBatch(GraphicsDevice);
            Hud = new HUD();
            Hud.Load();
        }

        private void InitGlobals()
        {
            Button.ClickSound = SFX.MessageFinish;
            Select<Resolution>.ChangeSound = SFX.ChestOpen;
            Slider.ChangeSound = SFX.ChestOpen;

            TitlePosition = new Vector2(Static.NativeWidth, 1);

            Static.SceneManager.Init();
            
            Dictionary<string, State> states = new Dictionary<string, State>()
            {
                { "MainMenu", new GameStateMainMenu(this) },
                { "Default", new GameStateDefault(this) },
                { "Dialog", new GameStateDialog(this) },
                { "GameOver", new GameStateGameOver(this) },
                { "StartOver", new GameStateStartOver(this) },
                { "Cutscene", new GameStateCutscene(this) }
            };

            StateMachine = new RenderStateMachine(states, "MainMenu");
        }

        private void QuitDialogState()
        {
            StateMachine.TransitionTo("Default");
        }
    }
}
