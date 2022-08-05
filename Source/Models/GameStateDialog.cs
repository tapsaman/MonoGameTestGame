using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTestGame.Managers;

namespace MonoGameTestGame.Models
{
    public class GameStateDialog : RenderState
    {
        private ZeldaAdventure666 _game;

        public GameStateDialog(ZeldaAdventure666 game)
        {
            CanReEnter = false;
            _game = game;
        }

        public override void Enter() {}

        public override void Update(GameTime gameTime)
        {
            Static.DialogManager.Update(gameTime);
            _game.TitlePosition.X -= (float)gameTime.ElapsedGameTime.TotalSeconds * 15;
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Static.Renderer.Start();
            
            Static.SceneManager.Draw(spriteBatch);
            _game.Hud.Draw(spriteBatch, Static.Player);

            if (!Static.DialogManager.IsDone)
                Static.DialogManager.Draw(spriteBatch);
            
            BitmapFontRenderer.DrawString(spriteBatch, "zeldan seikkailut mikä mikä maassa vittu", _game.TitlePosition);
            
            Static.Renderer.End(gameTime);
        }

        public override void Exit() {}
    } 
}