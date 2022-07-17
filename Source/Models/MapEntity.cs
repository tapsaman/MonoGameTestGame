using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTestGame.Managers;
using MonoGameTestGame.Sprites;

namespace MonoGameTestGame
{
    public abstract class MapEntity
    {
        public Hitbox Hitbox;
        public bool Interactable { get; protected set; } = false;
        public event Action Trigger;
        public bool HasTrigger()
        {
            return Trigger != null;
        }
        public void InvokeTrigger()
        {
            Trigger.Invoke();
        }
        public virtual Vector2 Position
        {
            get { return _position; }
            set
            {
                _position = value;
                Hitbox.Position = _position;
            }
        }
        private Vector2 _position;
        private static int _count;
        public int Index { get; private set; } 

        public MapEntity()
        {
            Hitbox = new Hitbox();
            Index = ++_count;
        }
    }
}
