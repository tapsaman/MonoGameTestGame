using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTestGame.Managers;

namespace MonoGameTestGame
{
    public abstract class Scene
    {
        public TileMap TileMap;
        public MapEntity[] MapEntities;
        public Player Player;
        public DialogManager DialogManager;
        public List<MapEntity> InteractableEntities { get; private set; }
        public List<MapEntity> HittableEntities { get; private set; }

        public abstract void Load();
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);

        public void Add(MapEntity mapEntity)
        {
            if (mapEntity.Interactable)
            {
                InteractableEntities.Add(mapEntity);
            }
            if (mapEntity.Hittable)
            {
                HittableEntities.Add(mapEntity);
            }
        }
    }
}