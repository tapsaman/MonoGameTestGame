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
        private string _mapName;
        private SaveData _saveData;
        private bool _sceneInited;
        private Animations.Spotlight _spotlight;
        private float _elapsedTime;
        private const float _TIME_1 = 0.0f;
        private const float _TIME_2 = 1.0f;

        public class Args : StateArgs
        {
            public string MapName = null;
            public SaveData SaveData = null;
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

            _mapName = null;
            _saveData = null;

            if (args is Args a)
            {
                _mapName = a.MapName;
                _saveData = a.SaveData;
            }
        }

        public override void Update(GameTime gameTime)
        {
            _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            if (!_sceneInited)
            {
                if (_elapsedTime < _TIME_1)
                    return;
                
                Static.GameStarted = false;
                Static.SessionData = new DataStore();
                _game.TitleText = new Animations.TitleText();

                if (_mapName != null)
                    Static.SceneManager.Init(_mapName);
                else if (_saveData != null)
                    Static.SceneManager.Init(_saveData);
                else
                    Static.SceneManager.Init();
                
                _sceneInited = true;
                return;
            }

            if (_elapsedTime < _TIME_2)
            {
                return;
            }
            else if (_spotlight == null)
            {
                _spotlight = new Animations.Spotlight(Static.Player, false);
                _spotlight.Enter();
                SFX.Fall.Play();
                return;
            }
            else if (!_spotlight.IsDone)
            {
                _spotlight.Update(gameTime);
            }
            else
            {
                _game.StateMachine.TransitionTo("Default");
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