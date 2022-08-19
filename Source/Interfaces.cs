using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TapsasEngine.Maps;

namespace TapsasEngine
{
    public interface IMapManager
    {
        public Map LoadTileMap(string mapName);
    }

    public interface IUpdate
    {
        void Update(GameTime gameTime);
    }

    public interface IDraw
    {
        void Draw(SpriteBatch spriteBatch);
    }

    public interface IDialogContent {}

    public interface IManager : IUpdate
    {
        public float ElapsedStageTime { get; set; }
        public bool IsDone { get; set; }
    }

    public interface IListManager<T, L> : IManager
        where T : IStage
        where L : IList<T>
    {
        public L Stages { get; set; }
        public void GoToNext();
        public int CurrentIndex { get; set; }
        public bool Looping { get; }
    }

    public interface IIndexedManager : IManager
    {
        public void GoToNext();
        public int CurrentIndex { get; set; }
        public bool Looping { get; }
    }

    public interface IArrayManager<T> : IManager where T : IStage
    {
        public T[] Stages { get; set; }
        public void GoToNext();
        public int CurrentIndex { get; set; }
        public bool Looping { get; }
    }


    public interface IDictionaryManager<K, T> : IManager 
        where T : IStage
    {
        public Dictionary<K, T> Stages { get; protected set; }
        public void GoTo(K stageKey);
        public K CurrentKey { get; set; }
    }

    public interface IStage : IUpdate
    {
        public void Enter();
        public bool IsDone { get; set; }
    }
}