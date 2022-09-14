using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using TapsasEngine.Utilities;
using TapsasEngine.Enums;
using ZA6.Models;
using TapsasEngine.Sprites;
using TapsasEngine;
using ZA6.Managers;

namespace ZA6
{
    public class Scene : IUpdate
    {
        public bool Locked;
        public bool LockedCamera;
        public Song Theme { get; protected set; }
        public TileMap TileMap;
        public Camera Camera { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public Player Player;
        public DataStore SceneData;
        public Dictionary<Direction, TransitionType> ExitTransitions;
        public string[] UseAlternativeLayers = new string[0];
        public bool Paused = true;
        public Vector2 OverlayOffset = Vector2.Zero;
        public Vector2? CameraTarget = null;
        public List<MapEntity> InteractableEntities { get; private set; }
        public List<Character> Characters { get; private set; }
        public List<MapObject> MapObjects { get; private set; }
        public List<MapObject> HittableEntities { get; private set; }
        /*public List<MapObject> CollidingEntities { get; private set; }
        public List<MapObject> UncollidingEntities { get; private set; }*/
        public List<MapEntity> TouchTriggers { get; private set; }
        public List<MapObject> GroundLevel { get; private set; }
        public List<MapObject> CharacterLevel { get; private set; }
        public List<MapObject> AirLevel { get; private set; }
        
        private List<IAnimationEffect> _animationEffects;
        // Register hitboxes for rendering
        private List<Hitbox> _hitboxes;

        public Scene()
        {
            // Load basics in constructor
            Camera = new Camera();
            SceneData = new DataStore();
            GroundLevel = new List<MapObject>();
            CharacterLevel = new List<MapObject>();
            AirLevel = new List<MapObject>();
            MapObjects = new List<MapObject>();
            Characters = new List<Character>();
            InteractableEntities = new List<MapEntity>();
            HittableEntities = new List<MapObject>();
            TouchTriggers = new List<MapEntity>();
            ExitTransitions = new Dictionary<Direction, TransitionType>();
            _animationEffects = new List<IAnimationEffect>();
            _hitboxes = new List<Hitbox>();
        }

        protected virtual void Load()
        {
            // Init events and map objects in Load
        }

        public void Init(Player player)
        {
            Width = TileMap.DrawWidth;
            Height = TileMap.DrawHeight;
            
            Camera.SceneWidth = Width;
            Camera.SceneHeight = Height;
            Camera.ScrollX = Camera.ScrollY = TileMap.Infinite;

            Player = player;
            Add(Player);

            LoadMapObjects();
            Load();
            Static.Scenarios.CurrentScenario.Apply(this);

            foreach (var exitKeyValue in TileMap.Exits)
            {
                if (ExitTransitions.ContainsKey(exitKeyValue.Key))
                {
                    exitKeyValue.Value.TransitionType = ExitTransitions[exitKeyValue.Key];
                }
            }
        }

        private void LoadMapObjects()
        {
            foreach(var obj in TileMap.Objects)
            {
                switch (obj.TypeName)
                {
                    // Map objects
                    case "Bush":
                        Add(new Bush() {
                            Position = obj.Position,
                            OverHole = obj.BoolProperty && Static.GameData.GetString("scenario") != null
                        });
                        break;
                    case "Sign":
                        Add(new Sign() { Position = obj.Position, Text = obj.TextProperty });
                        break;
                    case "TreasureChest":
                        Add(new TreasureChest(TileMap.Name + obj.Position.X + "," + obj.Position.Y) 
                        {
                            Position = obj.Position,
                            ItemID = obj.TextProperty
                        });
                        break;

                    // Event triggers
                    case "Text":
                        Add(new Text() { Position = obj.Position, Message = obj.TextProperty });
                        break;
                    case "Doorway":
                        Add(new Doorway(obj.Position, obj.TextProperty));
                        break;
                    
                    // Enemies
                    case "Guard":
                        Add(new Guard() { Position = obj.Position });
                        break;
                    case "Bat":
                        Add(new Bat() { Position = obj.Position });
                        break;
                    case "Bubble":
                        Add(new Bubble() { Position = obj.Position });
                        break;
                    case "Bari":
                        Add(new Bari() { Position = obj.Position });
                        break;
                    case "RedBari":
                        Add(new RedBari() { Position = obj.Position });
                        break;
                }
            }
        }

        public virtual void Start()
        {
            Paused = false;

            if (Theme != null)
                Music.Play(Theme);
            
            if (Static.GameData.GetString("scenario") == "tape")
            {
                Static.Renderer.ApplyPostEffect(Shaders.VHS);
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            if (Paused)
                return;
            
            TileMap.Update(gameTime);

            // Reverse for loops so we can safely add/remove items during iteration
            for (int i = MapObjects.Count - 1; i >= 0 ; i--)
            {
                MapObjects[i].Update(gameTime);
            }

            CharacterLevel.Sort((a, b) => a.Position.Y.CompareTo(b.Position.Y));

            Camera.Target = CameraTarget ?? Player.Hitbox.Rectangle.Center;

            for (int i = _animationEffects.Count - 1; i >= 0 ; i--)
            {
                _animationEffects[i].Update(gameTime);
            }
        }

        public void UpdateCamera(Vector2 targetPosition)
        {
            Camera.Target = targetPosition;
        }

        public void DrawGround(SpriteBatch spriteBatch)
        {
            TileMap.DrawGround(spriteBatch, Camera.Screen);

            foreach (var sprite in GroundLevel)
            {
                sprite.Draw(spriteBatch, Camera.Offset);
            }
            if (Static.RenderHitboxes)
            {
                foreach (var hitbox in _hitboxes)
                {
                    hitbox.Draw(spriteBatch, Camera.Offset);
                }
            }
            foreach (var mapEntity in CharacterLevel)
            {
                mapEntity.Draw(spriteBatch, Camera.Offset);
            }
        }

        public void DrawTop(SpriteBatch spriteBatch)
        {
            TileMap.DrawTop(spriteBatch, Camera.Offset);

            foreach (var mapEntity in AirLevel)
            {
                mapEntity.Draw(spriteBatch, Camera.Offset);
            }
            foreach (var animationEffect in _animationEffects)
            {
                animationEffect.Draw(spriteBatch, Camera.Offset);
            }
        }

        public virtual void DrawOverlay(SpriteBatch spriteBatch)
        {
            // Do nothing by default
        }

        public virtual void Exit()
        {
            // Do nothing by default
        }

        // For animations
        public void DrawPlayerOnly(SpriteBatch spriteBatch)
        {
            Player.Draw(spriteBatch, Camera.Offset);
        }

        public List<MapObject> LevelToList(MapLevel level)
        {
            switch (level)
            {
                case MapLevel.Character:
                    return CharacterLevel;
                case MapLevel.Ground:
                    return GroundLevel;
                case MapLevel.Air:
                    return AirLevel;
                default:
                    throw new Exception("Unexpected MapLevel value '" + level + "'");
            }
        }


        public void Add(MapEntity mapEntity)
        {
            if (mapEntity.Interactable)
            {
                InteractableEntities.Add(mapEntity);
            }
            if (mapEntity.TriggeredOnTouch)
            {
                TouchTriggers.Add(mapEntity);
            }

            if (mapEntity is MapObject mapObject)
            {
                MapObjects.Add(mapObject);
                LevelToList(mapObject.Level).Add(mapObject);

                if (mapObject.Hittable)
                {
                    HittableEntities.Add(mapObject);
                }

                if (mapObject is Character character)
                {
                    Characters.Add(character);
                }
            }
        }

        public void Remove(MapEntity mapEntity)
        {
            UnregisterHitbox(mapEntity.Hitbox);
            mapEntity.Unload();

            if (mapEntity.Interactable)
            {
                InteractableEntities.Remove(mapEntity);
            }
            if (mapEntity.TriggeredOnTouch)
            {
                TouchTriggers.Remove(mapEntity);
            }

            if (mapEntity is MapObject mapObject)
            {
                MapObjects.Remove(mapObject);
                LevelToList(mapObject.Level).Remove(mapObject);

                if (mapObject.Hittable)
                {
                    HittableEntities.Remove(mapObject);
                }

                if (mapObject is Character character)
                {
                    Characters.Remove(character);
                }
            }
        }

        public void Add(IAnimationEffect animationEffect)
        {
            _animationEffects.Add(animationEffect);
        }

        public void Remove(IAnimationEffect animationEffect)
        {
            _animationEffects.Remove(animationEffect);
        }

        public void RegisterHitbox(Hitbox hitbox)
        {
            _hitboxes.Add(hitbox);
        }

        public void UnregisterHitbox(Hitbox hitbox)
        {
            _hitboxes.Remove(hitbox);
        }
    }
}