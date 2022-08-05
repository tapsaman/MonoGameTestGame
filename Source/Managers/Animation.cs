using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameTestGame
{
    public abstract class Animation : IArrayManager<AnimationStage>, IDrawable
    {
        public float ElapsedStageTime { get; set; }
        public AnimationStage[] Stages { get => _stages; set => SetStages(value); }
        public int CurrentIndex { get; set; }
        public bool Looping { get { return false; } }
        public bool IsDone { get; set; }
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
            if (IsDone)
                return;
            
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

    public abstract class AnimationStage : IStage, IDrawable
    {
        public bool IsDone { get; set; }

        public virtual void Enter() {}
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}