using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZA6.Managers;

namespace ZA6.Models
{
    public class BatStateWaiting : CharacterState
    {
        private Bat _bat;
        private const float _ATTACK_DISTANCE = 80f;

        public BatStateWaiting(Bat bat)
            : base(bat)
        {
            _bat = bat;
        }

        public override void Enter(StateArgs _) {}

        public override void Update(GameTime gameTime)
        {
            var diff = Static.Player.Position - _bat.Position;

            if (Math.Pow(diff.X, 2) + Math.Pow(diff.Y, 2) < Math.Pow(_ATTACK_DISTANCE, 2))
            {
                _bat.StateMachine.TransitionTo("Default");
            }
        }

        public override void Exit() {}
    } 
}