using Microsoft.Xna.Framework;

namespace MonoGameTestGame.Models
{
    public class GuardStateTakenDamage : CharacterState
    {
        private Guard _guard;
        private Vector2 _flyVelocity;
        private float _elapsedTime;
        private const float _FLY_TIME = 0.5f;

        public GuardStateTakenDamage(Guard guard) : base(guard)
        {
            _guard = guard;
        }

        public override void Enter()
        {
            SFX.SmallEnemyHit.Play();
            _elapsedTime = 0;
            var vel = (_guard.Position - StaticData.Scene.Player.Position);
            vel.Normalize();
            _flyVelocity = vel * 150f;
            _guard.Health -= 1;
            _guard.Moving = true;
            _guard.Sprite.SetAnimation("Idle" + _guard.Direction);
        }

        public override void Update(GameTime gameTime)
        {
            _guard.Velocity = _flyVelocity;
            _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_elapsedTime > _FLY_TIME)
            {
                if (_guard.Health > 0)
                {
                    stateMachine.TransitionTo("Attacking");
                    _guard.IsHit = false;
                }
                else
                {
                    SFX.EnemyDies.Play();
                    StaticData.Scene.SetToRemove(_guard);
                }
            }
        }

        public override void Exit() {}
    } 
}