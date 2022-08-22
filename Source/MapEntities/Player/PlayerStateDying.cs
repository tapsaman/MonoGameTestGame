
using Microsoft.Xna.Framework;

namespace ZA6.Models
{
    public class PlayerStateDying : PlayerState
    {
        public const float _TIME_1 = 3.0f;
        private float _elapsedTime;

        public PlayerStateDying(Player player) : base(player)
        {
            CanReEnter = false;
        }

        public override void Enter(StateArgs _)
        {
            SFX.LinkFall.Play();
            _elapsedTime = 0f;
            Player.AnimatedSprite.SetAnimation("Dying");
            Static.Game.StateMachine.TransitionTo("Cutscene");
        }

        public override void Update(GameTime gameTime)
        {
            _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_elapsedTime > _TIME_1)
            {
                Static.Game.StateMachine.TransitionTo("GameOver");
            }
        }

        public override void Exit() {}
    } 
}