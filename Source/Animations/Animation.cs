using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TapsasEngine;

namespace ZA6.Animations
{
    public abstract class Animation : IDraw
    {
        public float Delta { get; private set; }
        public float ElapsedTime { get; private set; }
        public float ElapsedStageTime { get; private set; }
        public int CurrentIndex { get; private set; }
        public virtual bool Looping { get => false; }
        public bool IsDone { get; private set; }
        public Vector2 DrawOffset { private get; set; }
        public AnimationStage[] Stages {
            get => _stages;
            set
            {
                _stages = value;

                foreach (var stage in _stages)
                {
                    stage.Animation = this;
                }

                _drawingStages = new List<AnimationStage>();
            }
        }
        private AnimationStage[] _stages;
        private List<AnimationStage> _drawingStages;

        public void Enter()
        {
            EnterStage(0);
        }

        public void GoToNext()
        {
            if (!Stages[CurrentIndex].DrawAfterDone)
            {
                _drawingStages.Remove(Stages[CurrentIndex]);
            }

            if (CurrentIndex < Stages.Length - 1)
            {
                EnterStage(CurrentIndex + 1);
            }
            else
            {
                if (Looping)
                {
                    EnterStage(0);
                }
                else
                {
                    IsDone = true;
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            Delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            ElapsedTime += Delta;
            ElapsedStageTime += Delta;

            UpdateStage(CurrentIndex);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var stage in _drawingStages)
            {
                stage.Draw(spriteBatch);
            }
        }

        private void EnterStage(int index)
        {
            CurrentIndex = index;
            ElapsedStageTime = 0f;
            _drawingStages.Add(Stages[CurrentIndex]);
            Stages[CurrentIndex].Enter();
            AfterStageUpdate(CurrentIndex);
         }

        private void UpdateStage(int index)
        {
            Stages[index].Update(ElapsedStageTime);
            AfterStageUpdate(index);
        }

        private void AfterStageUpdate(int index)
        {
            var stage = Stages[index];

            if (stage.StageTime != null && ElapsedStageTime > stage.StageTime)
            {
                stage.IsDone = true;
                GoToNext();
            }
            else if (stage.IsDone)
            {
                GoToNext();
            }
        }
    }
}