using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTestGame.Managers;

namespace MonoGameTestGame.Models
{
    public abstract class State
    {
        public StateMachine stateMachine;
        public virtual bool CanReEnter { get; protected set; } = true;
        public abstract void Enter(StateArgs args);
        public abstract void Update(GameTime gameTime);
        public abstract void Exit();
    }

    public class StateArgs {}
}