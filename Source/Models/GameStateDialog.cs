using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZA6.Managers;

namespace ZA6.Models
{
    public class GameStateDialog : RenderState
    {
        private ZeldaAdventure666 _game;

        public GameStateDialog(ZeldaAdventure666 game)
        {
            CanReEnter = false;
            _game = game;
        }

        public override void Enter(StateArgs _) {}

        public override void Update(GameTime gameTime)
        {
            Static.EventSystem.Update(gameTime);
            Static.DialogManager.Update(gameTime);
            _game.Hud.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Static.Renderer.Start();
            
            Static.SceneManager.Draw(spriteBatch);
            _game.Hud.Draw(spriteBatch);

            if (!Static.DialogManager.IsDone)
                Static.DialogManager.Draw(spriteBatch);
            
            Static.Renderer.End();
        }

        public override void Exit() {}
    } 
}