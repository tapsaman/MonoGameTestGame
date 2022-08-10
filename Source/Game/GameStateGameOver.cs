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
        private float _elapsedTime = 0;
        private const float _DROP_TIME = 2f;
        private const float _WAIT_TIME = 1f;
        private bool _fallDeath;
        private bool _hasDropped;

        public class Args : StateArgs
        {
            public bool Falling;
        }

        public GameStateGameOver(ZeldaAdventure666 game)
        {
            CanReEnter = false;
            _game = game;
            _song = Static.Content.Load<Song>("oot_game_over");
        }

        public override void Enter(StateArgs args = null)
        {
            Static.Player.Moving = false;
            Static.Player.StateMachine.TransitionTo("Stopped");
            _animation = new Animations.GameOver();
            _elapsedTime = 0;
            _hasDropped = false;

            _fallDeath = (args is Args a && a.Falling);

            if (!_fallDeath)
            {
                Music.Stop();
                SFX.LinkDies.Play();
                Static.Player.Sprite.SetAnimation("Dying");
            }
            else
            {
                Music.PlayOnce(_song);
                _animation.Enter();
            }
        }

        public override void Update(GameTime gameTime)
        {
            Music.Update(gameTime);
            Static.SceneManager.Update(gameTime);

            if (!_fallDeath && !_hasDropped)
            {
                _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (_elapsedTime > _DROP_TIME)
                {
                    _hasDropped = true;
                    Music.PlayOnce(_song);
                    _animation.Enter();
                }

                return;
            }
            
            _animation.Update(gameTime);
            //Static.Player.Position -= new Vector2(50f * (float)gameTime.ElapsedGameTime.TotalSeconds, 0);

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
                    _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    
                    if (_elapsedTime > _WAIT_TIME)
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
            //_game.Hud.Draw(spriteBatch, Static.SceneManager.Player);
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