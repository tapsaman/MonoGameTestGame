using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ZA6.Models;
using TapsasEngine;

namespace ZA6.Managers
{
    public class StateMachine
    {
        public string CurrentStateKey { get; private set; } = null;
        public State CurrentState { get; private set; }
        public virtual Dictionary<string, State> States
        {
            get { return _states; }
            private set
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
            CurrentState.Enter(new StateArgs());
            CurrentStateKey = initialStateName;
        }

        public StateMachine(Dictionary<string, State> states)
        {
            States = states;
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
                return;
            }
            else if (CurrentState == null)
            {
                Sys.Debug("StateMachine entering first state " + newStateName);
            }
            else if (CurrentState == States[newStateName] && !CurrentState.CanReEnter)
            {
                Sys.Debug("StateMachine can't re-enter state " + newStateName);
                return;
            }
            else
            {
                CurrentState.Exit();
                Sys.Debug("StateMachine entering state " + newStateName);
            }

            CurrentState = States[newStateName];
            CurrentState.Enter(args);
            CurrentStateKey = newStateName;
        }
    }
}