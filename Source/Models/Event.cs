using System;
using Microsoft.Xna.Framework;
using ZA6.Managers;

namespace ZA6.Models
{
    public enum EventType
    {
        Text,
        Ask,
        Face,
        Animate,
        Condition,
        SaveValue,
        Wait
    }

    public abstract class Event //: IStage
    {
        public EventType Type { get; private set; }
        public string ID;
        public string WaitForID;
        public bool IsDone { get; set; }
        public Action Traverse;
        private static int _noIDCount = 0;

        // Events must be reset in Enter for reusage!
        public abstract void Enter();
        public abstract void Update(GameTime gameTime);
        public abstract void Exit();

        public Event(EventType type)
        {
            Type = type;
            //WaitForID = waitForID;
            //ID = id == "" ? "Unnamed_" + (_noIDCount++).ToString() : id;
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
            Static.DialogManager.DialogEnd -= OnDone;
            Static.Game.StateMachine.TransitionTo(_previousGameState);
            IsDone = true;
            Traverse?.Invoke();
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

    public class AskEvent : Event
    {
        public Dialog Dialog;
        public MapEntity Speaker;
        private string _previousGameState;
        public Event[] IfOption1; 
        public Event[] IfOption2;
        private EventManager _eventManager;
        public AskEvent(Dialog dialog, MapEntity speaker)
            : base(EventType.Ask)
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
            Static.DialogManager.DialogEnd += OnDialogDone;
        }

        private void OnDialogDone()
        {
            Static.DialogManager.DialogEnd -= OnDialogDone;
            Static.Game.StateMachine.TransitionTo(_previousGameState);
            
            if (Static.DialogManager.AnswerIndex == 0 && IfOption1 != null)
            {
                _eventManager = new EventManager(IfOption1);
                _eventManager.Enter();
            }
            else if (Static.DialogManager.AnswerIndex == 1 && IfOption2 != null)
            {
                _eventManager = new EventManager(IfOption2);
                _eventManager.Enter();
            }
            else
            {
                IsDone = true;
                Traverse?.Invoke();
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (_eventManager != null)
            {
                if (!_eventManager.IsDone)
                {
                    _eventManager.Update(gameTime);
                }
                else
                {
                    IsDone = true;
                    Traverse?.Invoke();
                }
            }
        }

        public override void Exit() {}
    }

    public class AnimateEvent : Event
    {
        public bool Wait = true;
        private Animation _animation;

        public AnimateEvent(Animation animation)
            : base(EventType.Animate)
        {
            _animation = animation;
        }

        public override void Enter()
        {
            IsDone = false;
            _animation.Enter();
            
            if (!Wait)
            {
                Traverse?.Invoke();
            }
        }

        public override void Update(GameTime gameTime)
        {
            _animation.Update(gameTime);

            if (_animation.IsDone)
            {
                IsDone = true;
                Traverse?.Invoke();
            }
        }

        public override void Exit() {}
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
            Traverse?.Invoke();
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
            DataStore dataStore = Static.Scene.SceneData;

            dataStore.Save(_id, _value);

            IsDone = true;
            Traverse?.Invoke();
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
                _eventManager.Enter();
            }
            else if (!condition && IfFalse != null)
            {
                _eventManager = new EventManager(IfFalse);
                _eventManager.Enter();
            }
            else
            {
                IsDone = true;
                Traverse?.Invoke();
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (_eventManager == null || _eventManager.IsDone)
            {
                IsDone = true;
                Traverse?.Invoke();
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
                Traverse?.Invoke();
            }
        }

        public override void Exit() {}
    }
}