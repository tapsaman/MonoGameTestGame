using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTestGame.Managers;

namespace MonoGameTestGame.Models
{
    public class GameStateCutscene : RenderState
    {
        private ZeldaAdventure666 _game;

        public GameStateCutscene(ZeldaAdventure666 game)
        {
            CanReEnter = false;
            _game = game;
        }

        public override void Enter()
        {
            Static.Player.StateMachine.TransitionTo("Stopped");
        }

        public override void Update(GameTime gameTime)
        {
            Static.EventSystem.Update(gameTime);
            Static.Scene.TileMap.Update(gameTime);
            foreach (var mapEntity in Static.Scene.MapObjects)
            {
                mapEntity.Update(gameTime);
            }
            _game.TitlePosition.X -= (float)gameTime.ElapsedGameTime.TotalSeconds * 15;
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Static.Renderer.Start();
            
            Static.SceneManager.Draw(spriteBatch);
            _game.Hud.Draw(spriteBatch, Static.Player);
            //Static.DialogManager.Draw(spriteBatch);
            
            BitmapFontRenderer.DrawString(spriteBatch, "zeldan seikkailut mikä mikä maassa vittu", _game.TitlePosition);
            
            Static.Renderer.End(gameTime);
        }

        public override void Exit()
        {
            Static.Player.StateMachine.TransitionTo("Idle");
        }
    } 
}