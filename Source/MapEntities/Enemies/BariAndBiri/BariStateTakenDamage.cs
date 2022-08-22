using Microsoft.Xna.Framework;

namespace ZA6.Models
{
    public class BariStateTakenDamage : CharacterState
    {
        private Enemy _enemy;
        private Vector2 _flyVelocity;
        private float _elapsedTime;
        private const float _FLY_TIME = 0.5f;
        private int _colorIndex = 0;
        private Color[] _damageColors = new Color[]
        {
            new Color(255, 200, 200)
        };

        public BariStateTakenDamage(Enemy enemy) : base(enemy)
        {
            _enemy = enemy;
        }

        public override void Enter(StateArgs _)
        {
            _enemy.AnimatedSprite.SetAnimation("TakenDamage");
            SFX.EnemyHit.Play();
            _elapsedTime = 0;
            _colorIndex = 0;
            var vel = (_enemy.Position - _enemy.DamagerPosition);
            vel.Normalize();
            _flyVelocity = vel * 150f;
            _enemy.Moving = true;
        }

        public override void Update(GameTime gameTime)
        {
            _enemy.Velocity = _flyVelocity;
            _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_elapsedTime < _FLY_TIME)
            {
                if (++_colorIndex == _damageColors.Length)
                    _colorIndex = 0;
            }
            else
            {
                if (_enemy.Health > 0)
                {
                    stateMachine.TransitionTo("Default");
                    _enemy.IsInvincible = false;
                }
                else
                {
                    _enemy.Die();
                }
            }
        }

        public override void Exit() {}
    } 
}