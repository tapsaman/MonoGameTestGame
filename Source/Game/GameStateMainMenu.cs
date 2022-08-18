using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using ZA6.Managers;

namespace ZA6.Models
{
    public class GameStateMainMenu : RenderState
    {
        private ZeldaAdventure666 _game;
        private GameMenu _menu;
        private string _previousStateKey;

        public GameStateMainMenu(ZeldaAdventure666 game)
        {
            CanReEnter = false;
            _game = game;
        }

        public override void Enter(StateArgs _)
        {
            _previousStateKey = stateMachine.CurrentStateKey;
            _menu = new GameMenu(ResumeGame);
            UI.Add(_menu);
            LifeHUD.LowHPSound.Stop();

            if (Static.GameStarted)
            {
                SFX.MenuOpen.Play();
                MediaPlayer.Pause();
            }

            SaveData.CreateAndSave();
        }

        public override void Update(GameTime gameTime)
        {
            UI.Update(gameTime);
            Music.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Static.Renderer.Start();

            if (Static.GameStarted)
            {
                Static.SceneManager.Draw(spriteBatch);
                _game.TitleText.Draw(spriteBatch);
                _game.Hud.Draw(spriteBatch);
            }
            else
            {
                Utility.DrawOverlay(spriteBatch, Color.DarkGray);
            }

            UI.Draw(spriteBatch);
            
            Static.Renderer.End();
        }

        private void ResumeGame(object sender, EventArgs e)
        {
            stateMachine.TransitionTo(_previousStateKey);
        }

        public override void Exit()
        {
            UI.SetToClear();
            _menu = null;

            if (Static.GameStarted)
            {
                SFX.MenuClose.Play();
                MediaPlayer.Resume();
            }
        }
    } 
}