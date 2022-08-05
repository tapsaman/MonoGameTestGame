using Microsoft.Xna.Framework;

namespace MonoGameTestGame.Models
{
    public class PlayerStateFalling : PlayerState
    {
        public const float _FALL_TIME = 0.6f;
        private float _elapsedFallTime;
        private bool _fallen;

        public PlayerStateFalling(Player player) : base(player)
        {
            CanReEnter = false;
        }

        public override void Enter()
        {
            SFX.LinkFall.Play();
            Player.Sprite.SetAnimation("Falling");
            Player.Moving = false;
            _elapsedFallTime = 0f;
            _fallen = false;
        }

        public override void Update(GameTime gameTime)
        {
            _elapsedFallTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (!_fallen && _elapsedFallTime > _FALL_TIME)
            {
                _fallen = true;
                Static.Game.StateMachine.TransitionTo("GameOver");
            }
        }

        public override void Exit() {}
    } 
}