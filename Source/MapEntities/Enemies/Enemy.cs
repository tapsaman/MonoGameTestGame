using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZA6.Managers;
using ZA6.Models;
using TapsasEngine.Sprites;

namespace ZA6
{
    public abstract class Enemy : Character
    {
        public Vector2 DamagerPosition;

        public abstract void DeterminePlayerDamage();

        public Enemy()
        {
            Interactable = false;
            Hittable = true;
            Hitbox.Color = Color.Red;
        }

        public override void Update(GameTime gameTime)
        {
            var gameStateKey = Static.Game.StateMachine.CurrentStateKey;
            
            if (gameStateKey != "Default" && gameStateKey != "StartMenu")
                return;
            
            base.Update(gameTime);

            //if (!IsInvincible)
            //{
            DeterminePlayerDamage();
            //}
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            if (!IsInvincible)
            {
                base.Draw(spriteBatch, offset);
            }
            else
            {
                Static.Renderer.ChangeToEffect(Shaders.EnemyDamage);
                base.Draw(spriteBatch, offset);
                Static.Renderer.ChangeToDefault();
            }
        }

        public override void TakeHit(Character damager)
        {
            if (!IsInvincible)
            {
                Health -= 1;
                DamagerPosition = damager.Hitbox.Rectangle.Center;
                IsInvincible = true;
                StateMachine.TransitionTo("TakenDamage");
            }
        }

        public virtual void Die()
        {
            SFX.EnemyDies.Play();
            Static.Scene.Add(new Animations.EnemyDeath(Hitbox.Rectangle.Center));
            Static.Scene.Remove(this);
        }
    }
}
