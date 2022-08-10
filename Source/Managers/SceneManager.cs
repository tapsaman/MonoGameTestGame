using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZA6.Models;

namespace ZA6.Manangers
{
    public class SceneManager
    {
        public Player Player { get; private set; }
        public Scene CurrentScene;
        public Scene ChangingToScene;
        public bool Changing;
        public TileMap.Exit MapExit { get; private set; }
        private SceneTransition _sceneTransition;

        public void Init()
        {
            // Static.Scene must be defined before player so that hitboxes can register
            Static.Scene = CurrentScene = new SceneB1();
            Static.Player = Player = new Player()
            {
                Position = CurrentScene.TileMap.ConvertTileXY(30, 44)
            };
            // Player must be defined for scene before scene init for events and map entities 
            CurrentScene.Init(Player);
            CurrentScene.UpdateCamera(Player.Position);
        }

        public void Start()
        {
            CurrentScene.Start();
        }

        public void GoTo(TileMap.Exit exit)
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

        // Exposed for ScreenTransitions
        public Scene LoadNextScene()
        {
            ChangingToScene = MapCodeToScene(MapExit.MapCode);
            ChangingToScene.Paused = true;
            Static.Scene = ChangingToScene;
            ChangingToScene.Init(Player);
            ChangingToScene.RegisterHitbox(Player.Hitbox);
            ChangingToScene.RegisterHitbox(Player.SwordHitbox);
            Static.Scene = CurrentScene;
            //CurrentScene.SetToRemove(Player);

            return ChangingToScene;
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

            spriteBatch.DrawString(Static.Font, Player.Position.ToString(), new Vector2(200, 200), Color.Black);
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
                case MapCode.C1:
                    return new SceneC1();
            }
            return null;
        }

        private SceneTransition TransitionTypeToSceneTransition(TransitionType transitionType)
        {
            switch (transitionType)
            {
                case TransitionType.Pan:
                    return new SceneTransition.Pan();
                case TransitionType.FadeToBlack:
                    return new SceneTransition.FadeToBlack();
            }
            return null;
        }
    }
}
