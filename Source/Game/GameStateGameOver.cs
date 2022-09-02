using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using TapsasEngine;
using ZA6.Managers;

namespace ZA6.Models
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
        private bool _hasDropped;
        private Args _args;

        public class Args : StateArgs
        {
            public string Question = null;
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
            Static.EventSystem.Clear();
            Static.Player.Moving = false;
            Static.Player.StateMachine.TransitionTo("Stopped");
            
            if (Static.GameData.GetInt("progress") == 1)
                Static.GameData.Save("progress", 2);
            
            if (Static.GameData.GetString("scenario") != "crispy")
                _animation = new Animations.GameOver();
            else
                _animation = new Animations.GameOver("PLEASE", "IMINPAIN", "IM IN PAIN");
            _elapsedTime = 0;
            _hasDropped = false;

            _args = args != null && args is Args a ? a : new Args();

            if (!_args.Falling)
            {
                Music.Stop();
                SFX.LinkDies.Play();
                Static.Player.AnimatedSprite.SetAnimation("Dying");
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
            _game.Hud.Update(gameTime);

            if (!_args.Falling && !_hasDropped)
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

            if (_animation.IsDone)
            {
                if (_dialogManager == null)
                {
                    _dialogManager = new DialogManager() { Borderless = true };
                    _dialogManager.Load(new Dialog(
                        new DialogAsk(_args.Question ?? "Continue?", "Save And Continue", "Save And Quit"/*, "Do Not Save And Continue"*/)
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
                        if (_dialogManager.AnswerIndex == 0)
                        {
                            SaveData.CreateAndSave();
                            _game.StateMachine.TransitionTo("StartOver");
                        }
                        else if (_dialogManager.AnswerIndex == 1)
                        {
                            SaveData.CreateAndSave();
                            _game.StateMachine.TransitionTo("StartMenu");
                        }  
                        else
                        {
                            _game.StateMachine.TransitionTo("StartOver");
                        }
                    }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Static.Renderer.Start();

            Static.SceneManager.Draw(spriteBatch);
            _animation.Draw(spriteBatch);
            
            if (_dialogManager == null)
            {
                _game.Hud.Draw(spriteBatch);
            }
            else
            {
                _dialogManager.Draw(spriteBatch);
            }

            Static.Renderer.End();
        }

        public override void Exit()
        {
            _dialogManager = null;
        }
    } 
}