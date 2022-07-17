using System;
using Microsoft.Xna.Framework;

namespace MonoGameTestGame.Models
{
    public enum EventType
    {
        Text,
        Move,
        Face,
        Animate, // should be Misc?
        Condition
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
        public MapEntity Target;
        public Direction? FaceTo;
        public MapEntity FaceTowards;
        private int _updated = 0;

        public FaceEvent(MapEntity target, Direction faceto)
            : base(EventType.Text)
        {
            Target = target;
            FaceTo = faceto;
        }
        public FaceEvent(MapEntity target, MapEntity faceTowards)
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
                Vector2 diff = Target.Position - FaceTowards.Position;

                if (Math.Abs(diff.X) > Math.Abs(diff.Y))
                {
                    Target.Direction = diff.X > 0 ? Direction.Left : Direction.Right;
                }
                else
                {
                    Target.Direction = diff.Y > 0 ? Direction.Up : Direction.Down;
                }
            }
        }

        public override void Update()
        {
            // End on first update so we get one rendering before next event 
            WhenDone?.Invoke();
        }

        public override void Exit() {}
    }
}