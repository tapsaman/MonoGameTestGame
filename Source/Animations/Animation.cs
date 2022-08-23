using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TapsasEngine;

namespace ZA6
{
    public abstract class Animation : IArrayManager<AnimationStage>, IDraw
    {
        public float ElapsedStageTime { get; set; }
        public AnimationStage[] Stages { get => _stages; set => SetStages(value); }
        public int CurrentIndex { get; set; }
        public bool Looping { get { return false; } }
        public bool IsDone { get; set; }
        public Vector2 DrawOffset { get; set; }
        private AnimationStage[] _stages;

        public void SetStages(AnimationStage[] stages)
        {
            _stages = stages;

            /*foreach (var item in _stages)
            {
                item.Manager = this;
            }*/
        }

        public void Enter()
        {
            Stages[CurrentIndex].Enter();

            if (Stages[CurrentIndex].IsDone)
            {
                GoToNext();
            }
        }


        public void GoToNext()
        {
            if (CurrentIndex + 1 == Stages.Length)
            {
                if (Looping)
                {
                    ElapsedStageTime = 0f;
                    CurrentIndex = 0;
                    Stages[CurrentIndex].Enter();
                }
                else
                {
                    IsDone = true;
                }
            }
            else
            {
                ElapsedStageTime = 0f;
                CurrentIndex += 1;
                Stages[CurrentIndex].Enter();
            }
        }

        public void Update(GameTime gameTime)
        {
            //if (IsDone)
            //    return;
            
            ElapsedStageTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            Stages[CurrentIndex].Update(gameTime);

            if (Stages[CurrentIndex].IsDone)
            {
                GoToNext();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
           Stages[CurrentIndex].Draw(spriteBatch);
        }
    }

    public abstract class AnimationStage : IStage, IDraw
    {
        public bool IsDone { get; set; }

        public virtual void Enter() {}
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
    }

    public class WaitStage : AnimationStage
    {
        private float _time;
        private bool _started;
        private float _elapsedTime;

        public WaitStage(float time)
        {
            _time = time;
        }

        public override void Enter()
        {
            _started = true;
            _elapsedTime = 0f;
        }

        public override void Update(GameTime gameTime)
        {
            if (!_started)
                return;
            
            _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_elapsedTime > _time)
            {
                _started = false;
                IsDone = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch) {}
    }
}