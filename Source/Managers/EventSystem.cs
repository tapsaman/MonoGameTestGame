using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TapsasEngine;
using ZA6.Models;

namespace ZA6.Managers
{
    
    public class EventSystem : IUpdate
    {
        [Flags]
        public enum Settings
        {
            None                = 0b_0000_0000,
            Parallel            = 0b_0000_0001,
            SustainSceneChange  = 0b_0000_0010,
            Looping             = 0b_0000_0100
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

        public void OnSceneChange()
        {
            _queue.RemoveAll(DoesNotSustainSceneChange);
            _parallel.RemoveAll(DoesNotSustainSceneChange);
        }

        private bool DoesNotSustainSceneChange(EventManagerAndSettings item)
        {
            return (item.Settings & Settings.SustainSceneChange) != Settings.SustainSceneChange;
        }

        public void Update(GameTime gameTime)
        {
            if (_queue.Count != 0)
            {
                var firstEventManager = _queue[0].EventManager;

                if (!firstEventManager.IsEntered)
                {
                    firstEventManager.Enter();
                }
                else if (!firstEventManager.IsDone)
                {
                    firstEventManager.Update(gameTime);
                }
                else
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