using Microsoft.Xna.Framework;

namespace MonoGameTestGame.Models
{
    public class GameStateDefault : GameState
    {
        public GameStateDefault(Scene scene) : base(scene) {}
        public override void Enter() {}

        public override void Update(GameTime gameTime)
        {
            foreach (var mapEntity in Scene.Characters)
            {
                mapEntity.Update(gameTime);
            }
        }

        public override void Exit() {}
    } 
}