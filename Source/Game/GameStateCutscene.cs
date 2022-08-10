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
            Static.SceneManager.Update(gameTime);
            Static.EventSystem.Update(gameTime);
            
            Music.Update(gameTime);

            _game.TitlePosition.X -= (float)gameTime.ElapsedGameTime.TotalSeconds * 15;
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Static.Renderer.Start();
            
            Static.SceneManager.Draw(spriteBatch);
            _game.Hud.Draw(spriteBatch, Static.Player);
            
            BitmapFontRenderer.DrawString(spriteBatch, "zeldan seikkailut mikä mikä maassa vittu", _game.TitlePosition);
            
            Static.Renderer.End(gameTime);
        }

        public override void Exit()
        {
            Static.Player.StateMachine.TransitionTo("Idle");
        }
    } 
}