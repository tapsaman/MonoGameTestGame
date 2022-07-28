using Microsoft.Xna.Framework;

namespace MonoGameTestGame.Models
{
    public class BatStateTakenDamage : CharacterState
    {
        private Bat _bat;
        private Vector2 _flyVelocity;
        private float _elapsedTime;
        private const float _FLY_TIME = 0.5f;
        private int _colorIndex = 0;
        private Color[] _damageColors = new Color[]
        {
            new Color(255, 200, 200)
        };

        public BatStateTakenDamage(Bat bat) : base(bat)
        {
            _bat = bat;
        }

        public override void Enter()
        {
            SFX.EnemyHit.Play();
            _elapsedTime = 0;
            _colorIndex = 0;
            var vel = (_bat.Position - _bat.DamagerPosition);
            vel.Normalize();
            _flyVelocity = vel * 150f;
            _bat.Moving = true;
            _bat.Sprite.SetAnimation("TakenDamage");
        }

        public override void Update(GameTime gameTime)
        {
            if (_bat.CollidingX != Direction.None || _bat.CollidingY != Direction.None)
            {
                _flyVelocity = Vector2.Zero;
            }

            _bat.Velocity = _flyVelocity;
            _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_elapsedTime < _FLY_TIME)
            {
                if (++_colorIndex == _damageColors.Length)
                    _colorIndex = 0;
            }
            else
            {
                if (_bat.Health > 0)
                {
                    stateMachine.TransitionTo("Default");
                    _bat.IsInvincible = false;
                }
                else
                {
                    SFX.EnemyDies.Play();
                    StaticData.Scene.Add(new Animations.EnemyDeath(_bat.Hitbox.Rectangle.Center));
                    StaticData.Scene.SetToRemove(_bat);
                }
            }
        }

        public override void Exit() {}
    } 
}