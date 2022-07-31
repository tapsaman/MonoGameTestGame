using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGameTestGame.Models;

namespace MonoGameTestGame.Managers
{
    public class StateMachine
    {
        public State CurrentState { get; private set; }
        public virtual Dictionary<string, State> States
        {
            get { return _states; }
            set
            {
                _states = value;
                
                foreach (State state in _states.Values)
                {
                    state.stateMachine = this;
                }
            }
        }
        public Dictionary<string, State> _states;

        public StateMachine(Dictionary<string, State> states, string initialStateName)
        {
            States = states;
            CurrentState = States[initialStateName];
            CurrentState.Enter();
        }

        public void Update(GameTime gameTime)
        {
            CurrentState.Update(gameTime);
        }

        public void TransitionTo(string newStateName)
        {
            if (!States.ContainsKey(newStateName))
            {
                Sys.LogError("StateMachine attempted to transition to undefined state '" + newStateName + "'");
            }
            else
            {
                CurrentState.Exit();
                CurrentState = States[newStateName];
                CurrentState.Enter();
            }

        }
    }
}