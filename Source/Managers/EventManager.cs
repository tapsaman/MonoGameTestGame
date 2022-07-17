using MonoGameTestGame.Models;

namespace MonoGameTestGame.Managers
{
    public class EventManager
    {
        private int _eventIndex; 
        private Event[] _eventList;
        private bool _lastEventDone = false;

        public void Load(Event[] eventList)
        {
            _eventList = eventList;
            _eventIndex = 0;
            InitEvent(0);
        }

        public void Update()
        {
            if (_eventList == null)
                return;

            if (!_lastEventDone)
            {
                _eventList[_eventIndex].Update();                
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
                Unload();
            }
        }

        private void Unload()
        {
            _eventList = null;
        }
    }
}