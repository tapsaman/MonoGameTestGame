﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZA6.Managers;
using ZA6.Models;
using ZA6.Sprites;

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
            Trigger += TakeDamage;
        }

        public override void Update(GameTime gameTime)
        {
            if (Static.Game.StateMachine.CurrentState is GameStateDefault == false)
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
                Sprite.Draw(spriteBatch, offset);
            }
            else
            {
                Static.Renderer.ChangeToDamageEffect();
                Sprite.Draw(spriteBatch, offset);
                Static.Renderer.ChangeToDefault();
            }
        }

        private void TakeDamage(Character damager)
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
