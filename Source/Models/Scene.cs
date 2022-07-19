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
        public StateMachine StateMachine;
        public EventManager EventManager;
        public TileMap TileMap;
        public Player Player;
        public DialogManager DialogManager;
        public List<MapEntity> InteractableEntities { get; private set; }
        public List<Character> Characters { get; private set; }
        public List<Character> HittableEntities { get; private set; }
        public List<Character> CollidingEntities { get; private set; }
        public List<Character> UncollidingEntities { get; private set; }
        public bool Paused = false;
        public Vector2 DrawOffset = Vector2.Zero;
        public int Width { get; private set; }
        public int Height { get; private set; }

        protected abstract void Load();

        public Scene(Player player)
        {
            //MapEntities = new List<MapEntity>();
            InteractableEntities = new List<MapEntity>();
            Characters = new List<Character>();
            HittableEntities = new List<Character>();
            CollidingEntities = new List<Character>();
            UncollidingEntities = new List<Character>();

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

            foreach(var item in TileMap.Tiles[TileMap.GroundLayer])
            {
                if (item.Value.TypeName == "Bush")
                {
                    
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
            CollidingEntities.Sort();

            if (Player.Velocity != Vector2.Zero)
                UpdateCamera(Player.Position);
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

        public void Add(Character character)
        {
            Add((MapEntity)character);

            Characters.Add(character);

            if (character.Hittable)
            {
                HittableEntities.Add(character);
            }
            if (character.Colliding)
            {
                CollidingEntities.Add(character);
            }
            else
            {
                UncollidingEntities.Add(character);
            }
        }

        public void Remove(MapEntity mapEntity)
        {
            //MapEntities.Remove(mapEntity);

            if (mapEntity.Interactable)
            {
                InteractableEntities.Remove(mapEntity);
            }
        }

        public void Remove(Character character)
        {
            Remove((MapEntity)character);

            Characters.Remove(character);

            if (character.Hittable)
            {
                HittableEntities.Remove(character);
            }
            if (character.Colliding)
            {
                CollidingEntities.Remove(character);
            }
            else
            {
                UncollidingEntities.Remove(character);
            }
        }

        private void QuitDialog()
        {
            StateMachine.TransitionTo("Default");
        }
    }
}