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

    public abstract class Event : IStage
    {
        public EventType Type { get; private set; }
        public string ID { get; private set; }
        public string WaitForID { get; private set; }
        public bool IsDone { get; set; }
        public Action WhenDone;
        private static int _noIDCount = 0;

        // Events must be reset in Enter for reusage!
        public abstract void Enter();
        public abstract void Update(GameTime gameTime);
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
        private string _previousGameState;
        public TextEvent(Dialog dialog, MapEntity speaker)
            : base(EventType.Text)
        {
            Dialog = dialog;
            Speaker = speaker;
        }

        public override void Enter()
        {
            IsDone = false;
            _previousGameState = Static.Game.StateMachine.CurrentStateKey;
            bool topDialogBox = Speaker.Position.Y > Static.NativeHeight / 2;
            Static.DialogManager.Load(Dialog, topDialogBox);
            Static.Game.StateMachine.TransitionTo("Dialog");
            Static.DialogManager.DialogEnd += OnDone;
        }

        private void OnDone()
        {
            Static.Game.StateMachine.TransitionTo(_previousGameState);
            IsDone = true;
            WhenDone?.Invoke();
        }

        public override void Update(GameTime gameTime)
        {
            // Since GameStateDialog stops event updates we are done one first update
            //IsDone = true;
        }

        public override void Exit()
        {
            Static.DialogManager.DialogEnd -= OnDone;
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
            IsDone = false;

            if (FaceTo != null)
            {
                Target.Direction = (Direction)FaceTo;
            }
            else 
            {
                Target.FaceTowards(FaceTowards.Position);
            }
        }

        public override void Update(GameTime gameTime)
        {
            // End on first update so we get one rendering before next event 
            IsDone = true;
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
            IsDone = false;
            DataStore dataStore = Static.Scene.SceneData;

            dataStore.Save(_id, _value);

            IsDone = true;
            WhenDone?.Invoke();
        }

        public override void Update(GameTime gameTime) {}

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
            IsDone = false;
            DataStore dataStore = Static.Scene.SceneData;
            bool condition = dataStore.Get(BasedOnID);

            if (condition && IfTrue != null)
            {
                _eventManager = new EventManager(IfTrue);
                //_eventManager.Enter();
            }
            else if (!condition && IfFalse != null)
            {
                _eventManager = new EventManager(IfFalse);
                //_eventManager.Enter();
            }
            else
            {
                WhenDone?.Invoke();
                IsDone = true;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (_eventManager == null || _eventManager.IsDone)
            {
                WhenDone?.Invoke();
                IsDone = true;
            }
            else
                _eventManager.Update(gameTime);
        }

        public override void Exit() {}
    }

    public class WaitEvent : Event
    {
        public float WaitTime;
        private float _elapsedWaitTime;

        public WaitEvent(float waitTime)
            : base(EventType.Condition)
        {
            WaitTime = waitTime;
        }

        public override void Enter()
        {
            IsDone = false;
            _elapsedWaitTime = 0;
        }

        public override void Update(GameTime gameTime)
        {
            _elapsedWaitTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            if (_elapsedWaitTime > WaitTime)
            {
                IsDone = true;
                WhenDone?.Invoke();
            }
        }

        public override void Exit() {}
    }
}