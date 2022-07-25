using System;
using Microsoft.Xna.Framework;
using MonoGameTestGame.Managers;

namespace MonoGameTestGame.Models
{
    public enum EventType
    {
        Text,
        Move,
        Face,
        Animate,
        Condition,
        SaveValue
    }

    public abstract class Event
    {
        public EventType Type { get; private set; }
        public string ID { get; private set; }
        public string WaitForID { get; private set; }
        public Action WhenDone;
        private static int _noIDCount = 0;

        public abstract void Enter();
        public abstract void Update();
        public abstract void Exit();

        public Event(EventType type, string id = "", string waitForID = "")
        {
            Type = type;
            WaitForID = waitForID;
            ID = id == "" ? "Unnamed_" + (_noIDCount++).ToString() : id;
        }
    }

    public class TextEvent : Event
    {
        public Dialog Dialog;
        public MapEntity Speaker;
        public TextEvent(Dialog dialog, MapEntity speaker)
            : base(EventType.Text)
        {
            Dialog = dialog;
            Speaker = speaker;
        }

        public override void Enter()
        {
            bool topDialogBox = Speaker.Position.Y > StaticData.NativeHeight / 2;
            StaticData.Scene.DialogManager.Load(Dialog, topDialogBox);
            StaticData.Scene.StateMachine.TransitionTo("Dialog");
            StaticData.Scene.DialogManager.DialogEnd += WhenDone;
        }

        public override void Update() {}

        public override void Exit()
        {
            StaticData.Scene.DialogManager.DialogEnd -= WhenDone;
        }
    }

    public class FaceEvent : Event
    {
        public Character Target;
        public Direction? FaceTo;
        public MapEntity FaceTowards;

        public FaceEvent(Character target, Direction faceto)
            : base(EventType.Text)
        {
            Target = target;
            FaceTo = faceto;
        }
        public FaceEvent(Character target, MapEntity faceTowards)
            : base(EventType.Text)
        {
            Target = target;
            FaceTowards = faceTowards;
        }

        public override void Enter()
        {
            if (FaceTo != null)
            {
                Target.Direction = (Direction)FaceTo;
            }
            else 
            {
                Target.FaceTowards(FaceTowards.Position);
            }
        }

        public override void Update()
        {
            // End on first update so we get one rendering before next event 
            WhenDone?.Invoke();
        }

        public override void Exit() {}
    }

    public class SaveValueEvent : Event
    {
        private EventStore _eventStore;
        private string _id;
        private bool _value;

        public SaveValueEvent(EventStore eventStore, string id, bool value)
            : base(EventType.SaveValue)
        {
            _eventStore = eventStore;
            _id = id;
            _value = value;
        }

        public override void Enter()
        {
            DataStore dataStore = StaticData.Scene.SceneData;

            dataStore.Save(_id, _value);

            WhenDone?.Invoke();
        }

        public override void Update() {}

        public override void Exit() {}
    }

    public class ConditionEvent : Event
    {
        public EventStore EventStore;
        public string BasedOnID;
        public bool Value;
        public Event[] IfTrue; 
        public Event[] IfFalse;
        private EventManager _eventManager;

        public ConditionEvent(EventStore eventStore, string id)
            : base(EventType.Condition)
        {
            EventStore = eventStore;
            BasedOnID = id;
        }

        public override void Enter()
        {
            DataStore dataStore = StaticData.Scene.SceneData;
            bool condition = dataStore.Get(BasedOnID);

            if (condition && IfTrue != null)
            {
                _eventManager = new EventManager();
                _eventManager.Load(IfTrue);
            }
            else if (!condition && IfFalse != null)
            {
                _eventManager = new EventManager();
                _eventManager.Load(IfFalse);
            }
            else
            {
                WhenDone?.Invoke();
            }
        }

        public override void Update()
        {
            if (_eventManager == null || _eventManager.Done)
                WhenDone?.Invoke();
            else
                _eventManager.Update();
        }

        public override void Exit() {}
    }
}