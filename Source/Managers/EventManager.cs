using Microsoft.Xna.Framework;
using MonoGameTestGame.Models;

namespace MonoGameTestGame.Managers
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

        public bool IsDone { get; set; }
        private int _eventIndex;
        private Event[] _eventList;
        private bool _lastEventDone = false;

        public EventManager(Event[] eventList)
        {
            Load(eventList);
        }
        
        public void Load(Event[] eventList)
        {
            IsDone = false;
            _eventList = eventList;
            _eventIndex = 0;
            InitEvent(0);
        }

        public void Update(GameTime gameTime)
        {
            if (_eventList == null)
                return;

            if (!_lastEventDone)
            {
                _eventList[_eventIndex].Update(gameTime);        
            }
            else
            {
                InitEvent(_eventIndex);
            }
        }
    
        private void InitEvent(int index)
        {
            _lastEventDone = false;
            var e = _eventList[index];
            e.WhenDone += EventDone;
            Sys.Debug("Entering " + e.ToString());
            e.Enter();
        }

        private void EventDone()
        {
            var e = _eventList[_eventIndex];
            e.Exit();
            e.WhenDone -= EventDone;
            _eventIndex++;

            if (_eventIndex < _eventList.Length)
            {
                _lastEventDone = true;
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