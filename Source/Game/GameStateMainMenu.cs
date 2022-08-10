using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using MonoGameTestGame.Managers;

namespace MonoGameTestGame.Models
{
    public class GameStateMainMenu : RenderState
    {
        private ZeldaAdventure666 _game;
        private GameMenu _menu;

        public GameStateMainMenu(ZeldaAdventure666 game)
        {
            CanReEnter = false;
            _game = game;
        }

        public override void Enter(StateArgs _)
        {
            if (Static.GameStarted)
            {
                SFX.MenuOpen.Play();
                MediaPlayer.Pause();
            }

            _menu = new GameMenu(ResumeGame);
            UI.Add(_menu);
        }

        public override void Update(GameTime gameTime)
        {
            UI.Update(gameTime);
            Music.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Static.Renderer.Start();

            Static.SceneManager.Draw(spriteBatch);
            _game.Hud.Draw(spriteBatch, Static.Player);
            //Static.DialogManager.Draw(spriteBatch);
            
            BitmapFontRenderer.DrawString(spriteBatch, "zeldan seikkailut mikä mikä maassa vittu", _game.TitlePosition);

            UI.Draw(spriteBatch);
            
            Static.Renderer.End(gameTime);
        }

        public override void Exit()
        {
            if (Static.GameStarted)
            {
                SFX.MenuClose.Play();
                MediaPlayer.Resume();
            }
        }

        private void ResumeGame(object sender, EventArgs e)
        {
            stateMachine.TransitionTo("Default");
            UI.SetToRemove(_menu);
            _menu = null;
        }
    } 
}