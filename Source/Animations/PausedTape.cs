using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TapsasEngine.Sprites;
using ZA6.Models;

namespace ZA6.Animations
{
    public class PausedTape : Animation
    {
        public PausedTape()
        {
            Stages = new AnimationStage[]
            {
                new WaitStage(7f),
                new PauseStage(),
                new WaitStage(3f),
                new UnpauseStage()
            };
        }

        private class PauseStage : AnimationStage
        {
            public Sprite PauseSprite = new Sprite(Static.Content.Load<Texture2D>("UI/tape-pause"));

            public override void Enter()
            {
                Static.Game.StateMachine.TransitionTo("Cutscene");
                StageTime = 1.5f;
                Shaders.VHS.OnPause();
                Music.Pause(Animation);
            }

            public override void Draw(SpriteBatch spriteBatch)
            {
                Vector2 position =  new Vector2(50, 50);
                //Static.Renderer.ChangeToUITarget();
                PauseSprite.Draw(Static.SpriteBatch, position);
                //Static.Renderer.ChangeToDefaultTarget();
            }
        }

        private class UnpauseStage : AnimationStage
        {
            public Sprite PlaySprite = new Sprite(Static.Content.Load<Texture2D>("UI/tape-play"));
            
            public override void Enter()
            {
                Static.Game.StateMachine.TransitionTo("Default");
                StageTime = 1.5f;
                Static.Scene.SceneData.Save("tape paused", true);
                Shaders.VHS.OnPlay();
                Music.Resume(Animation);
            }

            public override void Draw(SpriteBatch spriteBatch)
            {
                Vector2 position =  new Vector2(50, 50);
                PlaySprite.Draw(Static.SpriteBatch, position);
            }
        }
    }
}