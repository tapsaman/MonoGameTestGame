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
        //public List<MapEntity> MapEntities { get; private set; }
        public List<MapEntity> InteractableEntities { get; private set; }
        public List<Character> Characters { get; private set; }
        public List<Character> HittableEntities { get; private set; }
        public List<Character> CollidingEntities { get; private set; }
        public List<Character> UncollidingEntities { get; private set; }

        public abstract void Load();

        public Scene()
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
        }

        public virtual void Update(GameTime gameTime)
        {
            EventManager.Update();
            StateMachine.Update(gameTime);
            CollidingEntities.Sort();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            TileMap.Draw(spriteBatch, 0);

            foreach (var mapEntity in CollidingEntities)
            {
                mapEntity.Draw(spriteBatch);
            }

            TileMap.Draw(spriteBatch, 1);

            foreach (var mapEntity in UncollidingEntities)
            {
                mapEntity.Draw(spriteBatch);
            }

            Player.SwordHitbox.Draw(spriteBatch);
            DialogManager.Draw(spriteBatch);
            spriteBatch.DrawString(StaticData.Font, Player.Position.ToString(), new Vector2(200, 200), Color.Black);
        }

        public void Add(MapEntity mapEntity)
        {
            //MapEntities.Add(mapEntity);

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

        private void QuitDialog()
        {
            StateMachine.TransitionTo("Default");
        }
    }
}