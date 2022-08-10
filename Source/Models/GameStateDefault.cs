using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTestGame.Managers;

namespace MonoGameTestGame.Models
{
    public class GameStateDefault : RenderState
    {
        private ZeldaAdventure666 _game;

        public GameStateDefault(ZeldaAdventure666 game)
        {
            _game = game;
        }

        public override void Enter(StateArgs _)
        {
            if (!Static.GameStarted)
            {
                Static.SceneManager.Start();
                Static.GameStarted = true;
            }
        }

        public override void Update(GameTime gameTime)
        {
            Static.EventSystem.Update(gameTime);
            Static.SceneManager.Update(gameTime);
            Music.Update(gameTime);

            _game.TitlePosition.X -= (float)gameTime.ElapsedGameTime.TotalSeconds * 15;

            if (Input.P1.IsPressed(Input.P1.Start) || Input.P1.IsPressed(Input.P1.Select))
                stateMachine.TransitionTo("MainMenu");
            // To access main menu easily
            else if (Input.P1.JustReleasedMouseLeft())
                stateMachine.TransitionTo("MainMenu");
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Static.Renderer.Start();

            Static.SceneManager.Draw(spriteBatch);
            _game.Hud.Draw(spriteBatch, Static.Player);
            
            BitmapFontRenderer.DrawString(spriteBatch, "zeldan seikkailut mikä mikä maassa vittu", _game.TitlePosition);
            
            Static.Renderer.End(gameTime);
        }

        public override void Exit() {}
    } 
}