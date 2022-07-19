using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTestGame.Managers;
using MonoGameTestGame.Models;

namespace MonoGameTestGame
{
    public abstract class Scene
    {
        public DataStore SceneData;
        public StateMachine StateMachine;
        public EventManager EventManager;
        public TileMap TileMap;
        public Player Player;
        public DialogManager DialogManager;
        public List<MapEntity> InteractableEntities { get; private set; }
        public List<MapObject> MapObjects { get; private set; }
        public List<MapObject> HittableEntities { get; private set; }
        public List<MapObject> CollidingEntities { get; private set; }
        public List<MapObject> UncollidingEntities { get; private set; }
        public bool Paused = false;
        public Vector2 DrawOffset = Vector2.Zero;
        public int Width { get; private set; }
        public int Height { get; private set; }
        private List<MapObject> _removeAfterUpdate;

        protected abstract void Load();

        public Scene(Player player)
        {
            SceneData = new DataStore();
            MapObjects = new List<MapObject>();
            InteractableEntities = new List<MapEntity>();
            HittableEntities = new List<MapObject>();
            CollidingEntities = new List<MapObject>();
            UncollidingEntities = new List<MapObject>();
            _removeAfterUpdate = new List<MapObject>();

            Dictionary<string, State> states = new Dictionary<string, State>()
            {
                { "Default", new GameStateDefault(this) },
                { "Dialog", new GameStateDialog(this) }
            };

            StateMachine = new StateMachine(states, "Default");
            EventManager = new EventManager();
            DialogManager = new DialogManager();
            DialogManager.DialogEnd += QuitDialog;
            Player = player;
            Add(Player);
            Load();

            Width = TileMap.DrawWidth;
            Height = TileMap.DrawHeight;

            foreach(var obj in TileMap.Objects)
            {
                if (obj.TypeName == "Bush")
                {
                    Add(new Bush() { Position = obj.Position });
                }
            }
        }

        public virtual void Start()
        {
            Paused = false;
        }

        public void Update(GameTime gameTime)
        {
            if (Paused)
                return;
            
            EventManager.Update();
            StateMachine.Update(gameTime);
            TileMap.Update(gameTime);  
            //CollidingEntities.Sort();
            CollidingEntities.Sort((a, b) => a.Position.Y.CompareTo(b.Position.Y));

            if (Player.Velocity != Vector2.Zero)
                UpdateCamera(Player.Position);

            foreach (var mapEntity in _removeAfterUpdate)
            {
                Remove(mapEntity);
            }

            _removeAfterUpdate.Clear();
        }

        public void UpdateCamera(Vector2 targetPosition)
        {
            int halfWidth = StaticData.NativeWidth / 2;
            int x = Math.Min(TileMap.DrawWidth - StaticData.NativeWidth, Math.Max(0, (int)targetPosition.X - halfWidth));

            int halfHeight = StaticData.NativeHeight / 2;
            int y = Math.Min(TileMap.DrawHeight - StaticData.NativeHeight, Math.Max(0, (int)targetPosition.Y - halfHeight));

            DrawOffset = new Vector2(-x, -y);
        }

        public void DrawGround(SpriteBatch spriteBatch)
        {
            TileMap.Draw(spriteBatch, TileMap.GroundLayer, DrawOffset);

            foreach (var mapEntity in CollidingEntities)
            {
                mapEntity.Draw(spriteBatch);
            }
        }

        public void DrawTop(SpriteBatch spriteBatch)
        {
            TileMap.Draw(spriteBatch, TileMap.TopLayer, DrawOffset);

            foreach (var mapEntity in UncollidingEntities)
            {
                mapEntity.Draw(spriteBatch);
            }

            DialogManager.Draw(spriteBatch);
        }

        public void Add(MapEntity mapEntity)
        {
            if (mapEntity.Interactable)
            {
                InteractableEntities.Add(mapEntity);
            }
        }

        public void Add(MapObject mapobject)
        {
            Add((MapEntity)mapobject);

            MapObjects.Add(mapobject);

            if (mapobject.Hittable)
            {
                HittableEntities.Add(mapobject);
            }
            if (mapobject.Colliding)
            {
                CollidingEntities.Add(mapobject);
            }
            else
            {
                UncollidingEntities.Add(mapobject);
            }
        }

        public void Remove(MapEntity mapEntity)
        {
            if (mapEntity.Interactable)
            {
                InteractableEntities.Remove(mapEntity);
            }
        }

        public void Remove(MapObject mapobject)
        {
            Remove((MapEntity)mapobject);

            MapObjects.Remove(mapobject);

            if (mapobject.Hittable)
            {
                HittableEntities.Remove(mapobject);
            }
            if (mapobject.Colliding)
            {
                CollidingEntities.Remove(mapobject);
            }
            else
            {
                UncollidingEntities.Remove(mapobject);
            }
        }

        /* Set map entities to be removed after updates */
        public void SetToRemove(MapObject mapEntity)
        {
            _removeAfterUpdate.Add(mapEntity);
        }

        private void QuitDialog()
        {
            StateMachine.TransitionTo("Default");
        }
    }
}