using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTestGame.Managers;

namespace MonoGameTestGame.Models
{
    public class GameStateDialog : GameState
    {
        public GameStateDialog(Scene scene) : base(scene) {}
        public override void Enter()
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            Scene.DialogManager.Update(gameTime);
        }

        public override void Exit() {}
    } 
}