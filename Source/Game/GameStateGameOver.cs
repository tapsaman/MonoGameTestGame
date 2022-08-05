using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using MonoGameTestGame.Managers;

namespace MonoGameTestGame.Models
{
    public class GameStateGameOver : RenderState
    {
        private ZeldaAdventure666 _game;
        private Song _song;
        private Animations.GameOver _animation;
        private DialogManager _dialogManager;
        private float _elapsedWaitTime = 0;
        private const float _WAIT_TIME = 1f;

        public GameStateGameOver(ZeldaAdventure666 game)
        {
            CanReEnter = false;
            _game = game;
            _song = Static.Content.Load<Song>("oot_game_over");
        }

        public override void Enter()
        {
            _animation = new Animations.GameOver();
            _animation.Enter();
            _elapsedWaitTime = 0;
            Music.PlayOnce(_song);
        }

        public override void Update(GameTime gameTime)
        {
            Music.Update(gameTime);
            _animation.Update(gameTime);

            if (_animation.IsDone)
            {
                if (_dialogManager == null)
                {
                    _dialogManager = new DialogManager() { Borderless = true };
                    _dialogManager.Load(new Dialog(
                        new DialogAsk("Continue?", "Yes", "Ok?")
                    ));
                }
                else if (!_dialogManager.IsDone)
                {
                    _dialogManager.Update(gameTime);
                }
                else
                {
                    _elapsedWaitTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    
                    if (_elapsedWaitTime > _WAIT_TIME)
                    {
                        _game.StateMachine.TransitionTo("StartOver");
                    }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Static.Renderer.Start();

            Static.SceneManager.Draw(spriteBatch);
            _game.Hud.Draw(spriteBatch, Static.SceneManager.Player);
            _animation.Draw(spriteBatch);

            if (_dialogManager != null)
                _dialogManager.Draw(spriteBatch);

            Static.Renderer.End(gameTime);
        }

        public override void Exit()
        {
            _dialogManager = null;
        }
    } 
}