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
            _game = game;
        }

        public override void Enter() {}

        public override void Update(GameTime gameTime)
        {
            _game.DialogManager.Update(gameTime);
            _game.TitlePosition.X -= (float)gameTime.ElapsedGameTime.TotalSeconds * 15;
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Rendering.Start();
            
            _game.SceneManager.Draw(spriteBatch);
            _game.Hud.Draw(spriteBatch, _game.SceneManager.Player);
            _game.DialogManager.Draw(spriteBatch);
            
            BitmapFontRenderer.DrawString(spriteBatch, "zeldan seikkailut mikä mikä maassa vittu", _game.TitlePosition);
            
            Rendering.End(gameTime);
        }

        public override void Exit() {}
    } 
}