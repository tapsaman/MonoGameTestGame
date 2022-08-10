using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ZA6.Models;

namespace ZA6.Managers
{
    public class EventManager //: ArrayManager<Event>
    {
        /*public EventManager(Event[] events)
        {
            Stages = events;
        }

        public override void OnEnter(Event stage)
        {
            stage.Enter();
        }

        public override void OnExit(Event stage)
        {
            stage.Exit();
        }*/

        public bool IsDone { get; private set; }
        public bool IsEntered { get; private set; }
        private int _eventIndex;
        private Event[] _eventList;
        private Dictionary<string, Event> _waitingForID = new Dictionary<string, Event>();
        private UpdatableList<Event> _updating = new UpdatableList<Event>();
        private bool _shouldTraverse;

        public EventManager(Event[] eventList)
        {
            Load(eventList);
        }
        
        public void Load(Event[] eventList)
        {
            IsDone = false;
            IsEntered = false;
            _shouldTraverse = false;
            _eventList = eventList;
            _eventIndex = 0;
        }

        public void Enter()
        {
            IsEntered = true;
            InitEvent(_eventList[_eventIndex]);
        }

        public void Update(GameTime gameTime)
        {
            _updating.Update();

            foreach (var ev in _updating)
            {
                if (ev.IsDone)
                {
                    ev.Exit();

                    if (!string.IsNullOrEmpty(ev.ID) && _waitingForID.ContainsKey(ev.ID))
                    {
                        InitWaitingEvent(_waitingForID[ev.ID]);
                    }

                    _updating.SetToRemove(ev);
                }
                else
                {
                    ev.Update(gameTime);
                }
            }

            if (_shouldTraverse)
            {
                _shouldTraverse = false;
                InitEvent(_eventList[_eventIndex]);
            }
        }
    
        private void InitEvent(Event ev)
        {
            if (!string.IsNullOrEmpty(ev.WaitForID))
            {
                _waitingForID[ev.WaitForID] = ev;
                GoToNext();
                return;
            }

            _updating.Add(ev);
            ev.Traverse += GoToNext;
            Sys.Debug("Entering " + ev.ToString());
            ev.Enter();
        }

        private void InitWaitingEvent(Event ev)
        {
            if (string.IsNullOrEmpty(ev.WaitForID))
                throw new System.Exception("Ran InitWaitingEvent for event without WaitForID");
            
            _waitingForID.Remove(ev.WaitForID);
            _updating.SetToAdd(ev);
            Sys.Debug("Entering " + ev.ToString());
            ev.Enter();
        }

        private void GoToNext()
        {
            var e = _eventList[_eventIndex];
            e.Traverse -= GoToNext;
            _eventIndex++;

            if (_eventIndex < _eventList.Length)
            {
                _shouldTraverse = true;
            }
            else
            {
                IsDone = true;
                Unload();
            }
        }

        private void Unload()
        {
            _eventList = null;
        }
    }
}