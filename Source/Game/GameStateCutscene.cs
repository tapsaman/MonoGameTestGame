using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZA6.Managers;

namespace ZA6.Models
{
    public class GameStateCutscene : RenderState
    {
        private ZeldaAdventure666 _game;

        public GameStateCutscene(ZeldaAdventure666 game)
        {
            CanReEnter = false;
            _game = game;
        }

        public override void Enter(StateArgs _)
        {
            Static.Player.StateMachine.TransitionTo("Stopped");
        }

        public override void Update(GameTime gameTime)
        {
            Static.PlayTimeTimer.Update(gameTime);
            Static.SceneManager.Update(gameTime);
            Static.EventSystem.Update(gameTime);
            Music.Update(gameTime);
            _game.TitleText.Update(gameTime);
            _game.Hud.Update(gameTime);

            if (Input.P1.IsPressed(Input.P1.Start) || Input.P1.IsPressed(Input.P1.Select))
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

        public override void Exit()
        {
            Static.Player.StateMachine.TransitionTo("Idle");
        }
    } 
}