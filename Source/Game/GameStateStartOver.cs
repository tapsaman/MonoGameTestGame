using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using TapsasEngine.Utilities;
using ZA6.Managers;

namespace ZA6.Models
{
    public class GameStateStartOver : RenderState
    {
        private ZeldaAdventure666 _game;
        private Args _args;
        private bool _sceneInited;
        private Animations.Spotlight _spotlight;
        private float _elapsedTime;
        private const float _TIME_1 = 0.0f;
        private const float _TIME_2 = 1.0f;

        public class Args : StateArgs
        {
            public string MapName = null;
            public bool NoSpotlight = false;
        }

        public GameStateStartOver(ZeldaAdventure666 game)
        {
            CanReEnter = false;
            _game = game;
        }

        public override void Enter(StateArgs args)
        {
            _elapsedTime = 0;
            _sceneInited = false;
            _spotlight = null;

            _args = (args != null && args is Args a ? a : new Args());

            if (Static.Scene != null)
                Static.Scene.Exit();
        }

        public override void Update(GameTime gameTime)
        {
            _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            if (!_sceneInited)
            {
                if (_elapsedTime < _TIME_1)
                    return;
                
                Static.GameStarted = true;
                Static.SessionData = new DataStore();
                _game.TitleText = new Animations.TitleText();

                if (_args.MapName != null)
                    Static.SceneManager.Init(_args.MapName);
                else
                    Static.SceneManager.Init();
                
                _sceneInited = true;
                return;
            }

            if (_elapsedTime < _TIME_2)
            {
                return;
            }
            else if (!_args.NoSpotlight && _spotlight == null)
            {
                _spotlight = new Animations.Spotlight(Static.Player, false);
                _spotlight.Enter();
                SFX.Fall.Play();
                return;
            }
            else if (!_args.NoSpotlight && !_spotlight.IsDone)
            {
                _spotlight.Update(gameTime);
            }
            else
            {
                _game.StateMachine.TransitionTo("Default");
                Music.Play(Songs.DarkWorld);
                Static.SceneManager.Start();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Static.Renderer.Start();

            if (_spotlight == null)
            {
                Utility.DrawOverlay(spriteBatch, Color.Black);
            }
            else
            {
                Static.SceneManager.Draw(spriteBatch);
                _game.Hud.Draw(spriteBatch);
            }

            Static.Renderer.End();
        }

        public override void Exit() {}
    } 
}