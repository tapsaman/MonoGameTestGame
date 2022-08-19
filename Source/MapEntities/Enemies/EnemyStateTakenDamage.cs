using Microsoft.Xna.Framework;

namespace ZA6.Models
{
    public class EnemyStateTakenDamage : CharacterState
    {
        private Enemy _enemy;
        private Vector2 _flyVelocity;
        private float _elapsedTime;
        private const float _FLY_TIME = 0.35f;
        private int _colorIndex = 0;
        private Color[] _damageColors = new Color[]
        {
            new Color(255, 200, 200)
        };

        public EnemyStateTakenDamage(Enemy enemy) : base(enemy)
        {
            _enemy = enemy;
        }

        public override void Enter(StateArgs _)
        {
            SFX.EnemyHit.Play();
            _elapsedTime = 0;
            _colorIndex = 0;
            var vel = (_enemy.Position - _enemy.DamagerPosition);
            vel.Normalize();
            _flyVelocity = vel * 150f;
            _enemy.Moving = true;
            _enemy.Sprite.SetAnimation("Idle" + _enemy.Facing);
        }

        public override void Update(GameTime gameTime)
        {
            /*if (_enemy.CollidingX != Direction.None || _enemy.CollidingY != Direction.None)
            {
                //_flyVelocity = Vector2.Zero;
            }*/

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
                    stateMachine.TransitionTo("Attacking");
                    _enemy.IsInvincible = false;
                }
                else
                {
                    SFX.EnemyDies.Play();
                    Static.Scene.Add(new Animations.EnemyDeath(_enemy.Position + new Vector2(7)));
                    Static.Scene.Remove(_enemy);
                }
            }
        }

        public override void Exit() {}
    } 
}