using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZA6.Models;

namespace ZA6.Manangers
{
    public class SceneManager
    {
        public TiledWorld World;
        public Player Player { get; private set; }
        public Scene CurrentScene;
        public Scene ChangingToScene;
        public bool Changing;
        public MapExit MapExit { get; private set; }
        private SceneTransition _sceneTransition;

        public void Init(string startMapName = "A1")
        {
            // Static.Scene must be defined before player so that hitboxes can register
            Static.Scene = CurrentScene = LoadScene(startMapName);
            Static.Player = Static.Game.Hud.Player = Player = new Player()
            {
                Position = CurrentScene.TileMap.PlayerStartPosition
            };
            // Player must be defined for scene before scene init for events and map entities 
            CurrentScene.Init(Player);
            CurrentScene.UpdateCamera(Player.Position);
        }

        public void Start()
        {
            CurrentScene.Start();
        }

        public void GoTo(MapExit exit)
        {
            if (Changing)
                return;
            
            Changing = true;
            MapExit = exit;
            Static.Game.StateMachine.TransitionTo("Cutscene");
            _sceneTransition = TransitionTypeToSceneTransition(MapExit.TransitionType);
            _sceneTransition.SceneManager = this;
            _sceneTransition.Start(CurrentScene, Player, MapExit.Direction);
            
            //CurrentScene.Paused = true;
        }

        public void GoTo(string mapName)
        {
            GoTo(new MapExit(Player.Direction, mapName, TransitionType.Doorway));
        }

        // Exposed for ScreenTransitions
        public Scene LoadNextScene()
        {
            ChangingToScene = LoadScene(MapExit.MapName);
            ChangingToScene.Paused = true;
            
            Static.Scene = ChangingToScene;
            ChangingToScene.Init(Player);
            ChangingToScene.RegisterHitbox(Player.Hitbox);
            ChangingToScene.RegisterHitbox(Player.SwordHitbox);
            Static.Scene = ChangingToScene;
            
            return ChangingToScene;
        }

        private Scene LoadScene(string mapName)
        {
            TileMap map = World.LoadTileMap(mapName);
            Scene scene = MapNameToScene(mapName);
            scene.TileMap = map;

            return scene;
        }

        public void Update(GameTime gameTime)
        {
            if (!Changing)
            {
                CurrentScene.Update(gameTime);
                return;
            }

            _sceneTransition.Update(gameTime);

            if (_sceneTransition.Done)
            {
                CurrentScene = Static.Scene = ChangingToScene;
                ChangingToScene = null;
                Changing = false;
                _sceneTransition = null;

                // Make player walk two steps
                Static.EventSystem.Load(
                    new AnimateEvent(
                        new Animations.Walk.Timed(
                            Player,
                            MapExit.Direction.ToVector() * CurrentScene.TileMap.TileSize * 2,
                            0.2f
                )));

                CurrentScene.Start();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!Changing)
            {
                CurrentScene.DrawGround(spriteBatch);
                CurrentScene.DrawTop(spriteBatch);
                CurrentScene.DrawOverlay(spriteBatch);
            }
            else
            {
                _sceneTransition.Draw(spriteBatch);
            }
        }

        private Scene MapCodeToScene(MapCode mapCode)
        {
            switch (mapCode)
            {
                case MapCode.A1:
                    return new TestScene();
                case MapCode.A2:
                    return new SceneA2();
                case MapCode.B1:
                    return new SceneB1();
                case MapCode.B2:
                    return new SceneB2();
                case MapCode.C1:
                    return new SceneC1();
            }
            return null;
        }

        private Scene MapNameToScene(string mapName)
        {
            switch (mapName)
            {
                case "A1":
                    return new TestScene();
                case "A2":
                    return new SceneA2();
                case "B1":
                    return new SceneB1();
                case "B2":
                    return new SceneB2();
                case "C1":
                    return new SceneC1();
                case "Cave":
                    return new SceneCave();
                default:
                    return new Scene();
            }
        }

        private SceneTransition TransitionTypeToSceneTransition(TransitionType transitionType)
        {
            switch (transitionType)
            {
                case TransitionType.Pan:
                    return new SceneTransition.Pan();
                case TransitionType.FadeToBlack:
                    return new SceneTransition.FadeToBlack();
                case TransitionType.Doorway:
                    return new SceneTransition.Doorway();
            }
            return null;
        }
    }
}
