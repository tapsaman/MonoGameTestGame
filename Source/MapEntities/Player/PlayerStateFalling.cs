using Microsoft.Xna.Framework;

namespace ZA6.Models
{
    public class PlayerStateFalling : PlayerState
    {
        public const float _FALL_TIME = 2f;
        private float _elapsedFallTime;

        public PlayerStateFalling(Player player) : base(player)
        {
            CanReEnter = false;
        }

        public override void Enter(StateArgs _)
        {
            SFX.LinkFall.Play();
            Player.AnimatedSprite.SetAnimation("Falling");
            Player.Moving = false;
            Player.DrawingShadow = false;
            _elapsedFallTime = 0f;
        }

        public override void Update(GameTime gameTime)
        {
            _elapsedFallTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_elapsedFallTime > _FALL_TIME)
            {
                Static.Game.StateMachine.TransitionTo(
                    "GameOver",
                    new GameStateGameOver.Args() { Falling = true }
                );
            }
        }

        public override void Exit() {}
    } 
}