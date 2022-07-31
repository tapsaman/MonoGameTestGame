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
            _game = game;
        }

        public override void Enter()
        {
            MediaPlayer.Pause();
            _menu = new GameMenu(ResumeGame);
        }

        public override void Update(GameTime gameTime)
        {
            _menu.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Rendering.Start();

            _game.SceneManager.Draw(spriteBatch);
            _game.Hud.Draw(spriteBatch, _game.SceneManager.Player);
            _game.DialogManager.Draw(spriteBatch);
            
            BitmapFontRenderer.DrawString(spriteBatch, "zeldan seikkailut mikä mikä maassa vittu", _game.TitlePosition);

            _menu.Draw(spriteBatch);
            
            Rendering.End(gameTime);
        }

        public override void Exit()
        {
            MediaPlayer.Resume();
            SFX.MessageFinish.Play();
        }

        private void ResumeGame(object sender, EventArgs e)
        {
            stateMachine.TransitionTo("Default");
            _menu = null;
        }
    } 
}