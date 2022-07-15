using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTestGame.Managers;

namespace MonoGameTestGame.Models
{
    public class GameStateDefault : GameState
    {
        public GameStateDefault(Scene scene) : base(scene) {}
        public override void Enter()
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            Scene.Player.Update(gameTime);

            foreach (var mapEntity in Scene.MapEntities)
            {
                mapEntity.Update(gameTime, Scene.MapEntities, Scene.TileMap);
            }
        }

        public override void Exit() {}
    } 
}