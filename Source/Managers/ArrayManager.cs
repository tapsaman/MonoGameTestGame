using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ZA6
{
    public abstract class ArrayManager<T> : IIndexedManager, IStage
        where T : IStage
    {
        public float ElapsedStageTime { get; set; }
        public T[] Stages { get => _stages; set => SetStages(value); }
        public int CurrentIndex { get; set; }
        public bool Looping { get { return false; } }
        public bool IsDone { get; set; }
        private T[] _stages;
        private bool _moveOnNextUpdate;

        public abstract void OnEnter(T stage);
        public abstract void OnExit(T stage);

        public void SetStages(T[] stages)
        {
            _stages = stages;
        }

        public void Enter()
        {
            IsDone = false;
            //Stages[CurrentIndex].Enter();
            OnEnter(Stages[CurrentIndex]);
        }

        public void GoToNext()
        {
            _moveOnNextUpdate = true;
        }

        public void Update(GameTime gameTime)
        {
            if (IsDone)
                return;

            if (_moveOnNextUpdate)
            {
                MoveToNext();
                return;
            }
            
            ElapsedStageTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            Stages[CurrentIndex].Update(gameTime);

            if (Stages[CurrentIndex].IsDone)
            {
                GoToNext();
            }
        }

        private void MoveToNext()
        {
            _moveOnNextUpdate = false;

            OnExit(Stages[CurrentIndex]);

            if (CurrentIndex + 1 == Stages.Length)
            {
                if (Looping)
                {
                    ElapsedStageTime = 0f;
                    CurrentIndex = 0;
                    OnEnter(Stages[CurrentIndex]);
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
                OnEnter(Stages[CurrentIndex]);
            }
        }
    }

    public abstract class ListManager<T> : IIndexedManager
        where T : IStage
    {
        public float ElapsedStageTime { get; set; }
        public List<T> Stages { get => _stages; set => SetStages(value); }
        public int CurrentIndex { get; set; }
        public bool Looping { get { return false; } }
        public bool IsDone { get; set; }
        private List<T> _stages;

        public void SetStages(List<T> stages)
        {
            _stages = stages;
        }

        public void GoToNext()
        {
            if (CurrentIndex + 1 == Stages.Count)
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
    }
}