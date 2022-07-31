using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTestGame.Models;

namespace MonoGameTestGame.Managers
{
    public class RenderStateMachine : StateMachine
    {
        public RenderStateMachine(Dictionary<string, State> states, string initialStateName)
            : base(states, initialStateName) {}

        public void Draw(SpriteBatch spriteBatch)
        {
            if (CurrentState is RenderState state)
                state.Draw(spriteBatch);
        }
    }
}