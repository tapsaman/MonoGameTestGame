using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZA6.Managers;

namespace ZA6.Models
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