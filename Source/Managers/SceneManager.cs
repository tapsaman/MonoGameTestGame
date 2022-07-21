using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameTestGame.Manangers
{
    public class SceneManager
    {
        public Player Player;
        public Scene CurrentScene;
        public Scene ChangingToScene;
        public bool Changing;
        private TileMap.Exit _mapexit;
        private SceneTransition _sceneTransition;

        public void Init(Scene firstScene)
        {
            StaticData.Scene = CurrentScene = firstScene;
            Player = new Player() { Position = new Vector2(100, 100) };
            CurrentScene.Init(Player);
        }

        public void GoTo(TileMap.Exit exit)
        {
            Changing = true;
            _mapexit = exit;
            _sceneTransition = TransitionTypeToSceneTransition(_mapexit.TransitionType);
            _sceneTransition.SceneManager = this;
            _sceneTransition.Start(CurrentScene, Player, _mapexit.Direction);
            CurrentScene.Paused = true;
        }

        // Exposed for ScreenTransitions
        public Scene LoadNextScene()
        {
            ChangingToScene = MapCodeToScene(_mapexit.MapCode);
            ChangingToScene.Paused = true;
            StaticData.Scene = ChangingToScene;
            ChangingToScene.Init(Player);
            ChangingToScene.RegisterHitbox(Player.Hitbox);
            ChangingToScene.RegisterHitbox(Player.SwordHitbox);
            StaticData.Scene = CurrentScene;
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
                CurrentScene = StaticData.Scene = ChangingToScene;
                ChangingToScene = null;
                Changing = false;
                CurrentScene.Start();
                _sceneTransition = null;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!Changing)
            {
                CurrentScene.DrawGround(spriteBatch);
                CurrentScene.DrawTop(spriteBatch);
            }
            else
            {
                _sceneTransition.Draw(spriteBatch);
            }

            spriteBatch.DrawString(StaticData.Font, Player.Position.ToString(), new Vector2(200, 200), Color.Black);
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
