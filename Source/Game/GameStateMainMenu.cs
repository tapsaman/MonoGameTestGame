using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using TapsasEngine;
using TapsasEngine.Utilities;
using ZA6.Managers;
using ZA6.UI;

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
            UIManager.Add(_menu);
            LifeHUD.LowHPSound.Stop();
            SFX.MenuOpen.Play();
            Music.Pause(this);
        }

        public override void Update(GameTime gameTime)
        {
            Tengine.Delta = 0f;
            UIManager.Update(gameTime);
            Music.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Static.Renderer.Start();

            Static.SceneManager.Draw(spriteBatch);

            if (_game.TitleText.IsEntered)
            {
                _game.TitleText.Draw(spriteBatch);
            }
            _game.Hud.Draw(spriteBatch);

            //Static.Renderer.StartUI();

            Static.Renderer.ChangeToUITarget();

            UIManager.Draw(spriteBatch);
            
            Static.Renderer.End();
        }

        private void ResumeGame(object sender, EventArgs e)
        {
            stateMachine.TransitionTo(_previousStateKey);
        }

        public override void Exit()
        {
            UIManager.SetToClear();
            _menu = null;

            SFX.MenuClose.Play();
            Music.Resume(this);
        }
    } 
}