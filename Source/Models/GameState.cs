using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTestGame.Managers;

namespace MonoGameTestGame.Models
{
    public abstract class GameState : State
    {
        public Scene Scene;
        public GameState(Scene scene)
        {
            Scene = scene;
        }
    } 
}