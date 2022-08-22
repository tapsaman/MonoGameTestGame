using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZA6.Models
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
            Static.PlayTimeTimer.Update(gameTime);
            Static.EventSystem.Update(gameTime);
            Static.SceneManager.Update(gameTime);
            Music.Update(gameTime);
            _game.Hud.Update(gameTime);
            _game.TitleText.Update(gameTime);

            if (Input.P1.JustReleased(Input.P1.Start) || Input.P1.JustReleased(Input.P1.Select))
                stateMachine.TransitionTo("MainMenu");
            // To access main menu easily
            else if (Input.P1.JustReleasedMouseLeft())
                stateMachine.TransitionTo("MainMenu");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Static.Renderer.Start();

            Static.SceneManager.Draw(spriteBatch);
            _game.TitleText.Draw(spriteBatch);
            _game.Hud.Draw(spriteBatch);
            
            Static.Renderer.End();
        }

        public override void Exit() {}
    } 
}