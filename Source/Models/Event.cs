using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TapsasEngine;
using TapsasEngine.Enums;
using TapsasEngine.Utilities;
using ZA6.Animations;
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
        Wait,
        Teleport,
        Remove,
        Run
    }

    public abstract class Event //: IStage
    {
        public EventType Type { get; private set; }
        public string ID;
        public string WaitForID;
        public bool IsDone { get; set; }
        public Action Traverse;

        // Events must be reset in Enter for reusage!
        public abstract void Enter();
        public abstract void Update(GameTime gameTime);
        public abstract void Exit();

        public Event(EventType type)
        {
            Type = type;
        }
    }

    public class TextEvent : Event
    {
        public Dialog Dialog;
        public MapEntity Speaker;
        private string _previousGameState;
        public TextEvent(Dialog dialog)
            : base(EventType.Text)
        {
            Dialog = dialog;
        }

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
            bool topDialogBox = Speaker == null
                ? false
                : Speaker.Position.Y + Static.Scene.Camera.Offset.Y > Static.NativeHeight / 2;
            
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

        public override void Exit()
        {
            if (_eventManager != null)
                _eventManager.Exit();
        }
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

        public override void Exit()
        {
            _animation.Exit();
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
                Target.Facing = (Direction)FaceTo;
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
        private DataStoreType _dataStoreType;
        private string _id;
        private bool _value;

        public SaveValueEvent(DataStoreType dataStoreType, string id, bool value)
            : base(EventType.SaveValue)
        {
            _dataStoreType = dataStoreType;
            _id = id;
            _value = value;
        }

        public override void Enter()
        {
            Static.GetStoreByType(_dataStoreType).Save(_id, _value);

            IsDone = true;
            Traverse?.Invoke();
        }

        public override void Update(GameTime gameTime) {}

        public override void Exit() {}
    }

    public class ConditionEvent : Event
    {
        public Event[] IfTrue; 
        public Event[] IfFalse;
        private Func<bool> _condition;
        private EventManager _eventManager;

        public ConditionEvent(Func<bool> condition)
            : base(EventType.Condition)
        {
            _condition = condition;
        }

        public ConditionEvent(DataStoreType dataStoreType, string id)
            : base(EventType.Condition)
        {
            _condition = () => Static.GetStoreByType(dataStoreType).Get(id);
        }

        public override void Enter()
        {
            IsDone = false;
            bool conditionResult = _condition();

            if (conditionResult && IfTrue != null)
            {
                _eventManager = new EventManager(IfTrue);
                _eventManager.Enter();
            }
            else if (!conditionResult && IfFalse != null)
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

        public override void Exit()
        {
            if (_eventManager != null)
                _eventManager.Exit();
        }
    }

    public class WaitEvent : Event
    {
        public float WaitTime;
        private float _elapsedWaitTime;

        public WaitEvent(float waitTime)
            : base(EventType.Wait)
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

    public class TeleportEvent : Event
    {
        public string MapName;

        public TeleportEvent(string mapName)
            : base(EventType.Teleport)
        {
            MapName = mapName;
        }

        public override void Enter()
        {
            IsDone = false;
            Static.SceneManager.GoTo(MapName);
        }

        public override void Update(GameTime gameTime)
        {
            if (!Static.SceneManager.Changing)
            {
                IsDone = true;
                Traverse?.Invoke();
            }
        }

        public override void Exit() {}
    }

    public class RemoveEvent : Event
    {
        private MapEntity _target;

        public RemoveEvent(MapEntity target)
            : base(EventType.Remove)
        {
            _target = target;
        }

        public override void Enter()
        {
            if (_target is Character c)
            {
                Static.Scene.Remove(c);
            }
            else if (_target is MapObject mo)
            {
                Static.Scene.Remove(mo);
            }
            else
            {
                Static.Scene.Remove(_target);
            }

            IsDone = true;
            Traverse?.Invoke();
        }

        public override void Update(GameTime gameTime) {}

        public override void Exit() {}
    }

    public class RunEvent : Event
    {
        private Action _action;

        public RunEvent(Action action)
            : base(EventType.Run)
        {
            _action = action;
        }

        public override void Enter()
        {
            _action.Invoke();

            IsDone = true;
            Traverse?.Invoke();
        }

        public override void Update(GameTime gameTime) {}

        public override void Exit() {}
    }

    public class DoEvent : Event
    {
        private Func<Event[]> _callback;
        private EventManager _eventManager;

        public DoEvent(Func<Event[]> callback)
            : base(EventType.Run)
        {
            _callback = callback;
        }

        public override void Enter()
        {
            IsDone = false;
            _eventManager = new EventManager(_callback());
            _eventManager.Enter();
        }

        public override void Update(GameTime gameTime)
        {
            if (_eventManager.IsDone)
            {
                IsDone = true;
                Traverse?.Invoke();
            }
            else
            {
                _eventManager.Update(gameTime);
            }
        }

        public override void Exit()
        {
            _eventManager.Exit();
        }
    }
}