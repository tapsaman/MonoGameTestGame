using System;
using Microsoft.Xna.Framework;

namespace MonoGameTestGame
{
    public abstract class MapEntity
    {
        public Hitbox Hitbox;
        public bool Interactable { get; protected set; }
        public bool TriggeredOnTouch { get; protected set; }
        public event Action<Character> Trigger;
        public virtual Vector2 Position
        {
            get { return _position; }
            set
            {
                _position = value;
                Hitbox.Position = _position;
            }
        }
        public int Index { get; private set; } 
        private Vector2 _position;
        private static int _count;

        public MapEntity()
        {
            Hitbox = new Hitbox();
            Index = ++_count;
        }
        ~MapEntity()
        {
            Unload();
        }
        public virtual void Unload()
        {
            StaticData.Scene.UnregisterHitbox(Hitbox);
            Hitbox.Dispose();
        }
        public bool HasTrigger()
        {
            return Trigger != null;
        }
        public void InvokeTrigger(Character triggerer)
        {
            Trigger.Invoke(triggerer);
        }
    }
}
