using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using MonoGameTestGame.Managers;

namespace MonoGameTestGame.Models
{
    public class GameStateStartOver : RenderState
    {
        private ZeldaAdventure666 _game;
        private bool _sceneInited;
        private bool _shaderInited;
        private float _elapsedTime;
        private const float _TIME_1 = 0.0f;
        private const float _TIME_2 = 1.0f;
        private const float _TIME_3 = 2.5f;

        public GameStateStartOver(ZeldaAdventure666 game)
        {
            CanReEnter = false;
            _game = game;
        }

        public override void Enter(StateArgs _)
        {
            _elapsedTime = 0;
            _sceneInited = false;
            _shaderInited = false;
        }

        public override void Update(GameTime gameTime)
        {
            _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            if (!_sceneInited)
            {
                if (_elapsedTime < _TIME_1)
                    return;
                
                Static.GameStarted = false;
                Static.SceneManager.Init();
                _sceneInited = true;
                return;
            }

            if (!_shaderInited && _elapsedTime > _TIME_2)
            {
                SFX.Fall.Play();
                _shaderInited = true;
                Static.Renderer.ApplyPostEffect(Shaders.Spotlight);
                return;
            }

            if (_elapsedTime > _TIME_3)
            {
                Static.Renderer.ApplyPostEffect(null);
                _game.StateMachine.TransitionTo("Default");
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Static.Renderer.Start();

            if (_shaderInited)
            {
                Static.SceneManager.Draw(spriteBatch);
                _game.Hud.Draw(spriteBatch, Static.SceneManager.Player);
            }
            else
            {
                Utility.DrawOverlay(spriteBatch, Color.Black);
            }

            Static.Renderer.End(gameTime);
        }

        public override void Exit() {}
    } 
}