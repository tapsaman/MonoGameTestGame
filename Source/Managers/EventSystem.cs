using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGameTestGame.Models;

namespace MonoGameTestGame.Managers
{
    
    public class EventSystem : IUpdatable
    {
        [Flags]
        public enum Settings
        {
            None            = 0b_0000_0000,
            Parallel        = 0b_0000_0001,
            RemoveWithStage = 0b_0000_0010,
            Looping         = 0b_0000_0100
        }

        private List<EventManagerAndSettings> _queue = new List<EventManagerAndSettings>();
        private List<EventManagerAndSettings> _parallel = new List<EventManagerAndSettings>();

        public void Load(Event singleEvent, Settings settings = Settings.None)
        {
            Load(new Event[] { singleEvent }, settings);
        }
        
        public void Load(Event[] eventList, Settings settings = Settings.None)
        {

            if ((settings & Settings.Parallel) == Settings.Parallel)
            {
                _parallel.Add(new EventManagerAndSettings()
                {
                    EventManager = new EventManager(eventList),
                    Settings = settings
                });
            }
            else
            {
                Static.Game.StateMachine.TransitionTo("Cutscene");

                _queue.Add(new EventManagerAndSettings()
                {
                    EventManager = new EventManager(eventList),
                    Settings = settings
                });
            }
        }

        public void Update(GameTime gameTime)
        {
            if (_queue.Count != 0)
            {
                _queue[0].EventManager.Update(gameTime);

                if (_queue[0].EventManager.IsDone)
                {
                    _queue.RemoveAt(0);

                    if (_queue.Count == 0)
                    {
                        Static.Game.StateMachine.TransitionTo("Default");
                    }

                }
            }
            else
            {
                foreach (var item in _parallel)
                {
                    item.EventManager.Update(gameTime);
                }
            }
        }

        private class EventManagerAndSettings
        {
            public EventManager EventManager;
            public Settings Settings;
        }

    }
}