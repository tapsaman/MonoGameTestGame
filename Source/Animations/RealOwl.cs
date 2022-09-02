using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TapsasEngine.Sprites;
using ZA6.Models;

namespace ZA6.Animations
{
    public class RealOwl : Animation
    {
        public Vector2 MapPosition;
        public AnimatedSprite OwlSprite;

        public RealOwl(Player player, Vector2 mapPosition)
        {
            MapPosition = mapPosition;

            var owlTexture = Static.Content.Load<Texture2D>("owl");
            Dictionary<string, SAnimation> animations = new Dictionary<string, SAnimation>()
            {
                { "Idle", new SAnimation(owlTexture, 2, 0, 255, 248) },
                { "Fly",  new SAnimation(owlTexture, 2, 255, 248, 0.2f, isLooping: true) }
            };
            OwlSprite = new AnimatedSprite(animations, "Idle");
            
            Stages = new AnimationStage[]
            {
                new WaitForEnterStage(),
                new Resting() { Player = player, MapPosition = mapPosition },
                new Flying() { MapStartPosition = mapPosition }
            };

            Static.Renderer.OnPostDraw += DrawOwl;
        }

        public void Exit()
        {
            Static.Renderer.OnPostDraw -= DrawOwl;
        }

        private void DrawOwl()
        {
            Vector2 position = (MapPosition + DrawOffset) // Native position
                * Static.Renderer.NativeSizeMultiplier
                + Static.Renderer.ScreenRectangle.Location.ToVector2() // Rendering position
                + new Vector2(-150, -248); // Sprite offset
            
            OwlSprite.Draw(Static.SpriteBatch, position);
        }

        private class Resting : AnimationStage
        {
            public Player Player;
            public Vector2 MapPosition;
            private const float _LEAVE_DISTANCE = 70f;
            
            public override void Update(float elapsedTime)
            {
                var diff = Player.Position - MapPosition;

                if (Math.Pow(diff.X, 2) + Math.Pow(diff.Y, 2) < Math.Pow(_LEAVE_DISTANCE, 2))
                {
                    SFX.Keese.Play();
                    ((RealOwl)Animation).OwlSprite.SetAnimation("Fly");
                    IsDone = true;
                }
            }
        }

        private class Flying : AnimationStage
        {
            public Vector2 MapStartPosition;
            public Vector2 Velocity = new Vector2(120, -30);

            public override void Enter()
            {
                Static.GameData.Save("bird scare count", 0);
                Static.SessionData.Save("bird scared", true);
            }

            public override void Update(float elapsedTime)
            {
                var animation = (RealOwl)Animation;
                animation.MapPosition = MapStartPosition + Velocity * elapsedTime;
            }
        }
    }
}