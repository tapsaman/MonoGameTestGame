using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ZA6.Models;

namespace ZA6.Managers
{
    public class StateMachine
    {
        public string CurrentStateKey { get; private set; }
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
            CurrentStateKey = initialStateName;
            CurrentState.Enter(new StateArgs());
        }

        public void Update(GameTime gameTime)
        {
            CurrentState.Update(gameTime);
        }

        public void TransitionTo(string newStateName, StateArgs args = null)
        {
            if (!States.ContainsKey(newStateName))
            {
                Sys.LogError("StateMachine attempted to transition to undefined state '" + newStateName + "'");
            }
            else if (CurrentState.CanReEnter || CurrentState != States[newStateName])
            {
                CurrentState.Exit();
                Sys.Debug("StateMachine entering state " + newStateName);
                CurrentState = States[newStateName];
                CurrentStateKey = newStateName;
                CurrentState.Enter(args);
            }
        }
    }
}