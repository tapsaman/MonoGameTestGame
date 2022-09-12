using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZA6.Models
{
    public class GameStateStopped : RenderState
    {
        private ZeldaAdventure666 _game;

        public GameStateStopped(ZeldaAdventure666 game)
        {
            CanReEnter = false;
            _game = game;
        }

        public override void Enter(StateArgs _) {}

        public override void Update(GameTime gameTime)
        {
            Static.EventSystem.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Static.Renderer.Start();
            
            Static.SceneManager.Draw(spriteBatch);
            _game.Hud.Draw(spriteBatch);
            
            Static.Renderer.End();
        }

        public override void Exit() {}
    } 
}