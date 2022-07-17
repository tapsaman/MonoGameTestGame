using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTestGame.Managers;

namespace MonoGameTestGame
{
    public abstract class Scene
    {
        public StateMachine StateMachine;
        public EventManager EventManager;
        public TileMap TileMap;
        public Player Player;
        public DialogManager DialogManager;
        public List<MapEntity> MapEntities { get; private set; }
        public List<MapEntity> InteractableEntities { get; private set; }
        public List<MapEntity> HittableEntities { get; private set; }
        public List<MapEntity> CollidingEntities { get; private set; }
        public List<MapEntity> UncollidingEntities { get; private set; }

        public abstract void Load();
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);

        public Scene()
        {
            MapEntities = new List<MapEntity>();
            InteractableEntities = new List<MapEntity>();
            HittableEntities = new List<MapEntity>();
            CollidingEntities = new List<MapEntity>();
            UncollidingEntities = new List<MapEntity>();
        }
        public void Add(MapEntity mapEntity)
        {
            MapEntities.Add(mapEntity);

            if (mapEntity.Interactable)
            {
                InteractableEntities.Add(mapEntity);
            }
            if (mapEntity.Hittable)
            {
                HittableEntities.Add(mapEntity);
            }
            if (mapEntity.Colliding)
            {
                CollidingEntities.Add(mapEntity);
            }
            else
            {
                UncollidingEntities.Add(mapEntity);
            }
        }
    }
}