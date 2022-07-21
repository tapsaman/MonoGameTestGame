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
        private const float _LOAD_TIME = 0.2f;
        private const float _CHANGE_TIME = 1f;
        private float _elapsedChangeTime;
        private Direction _newDirection;
        private MapCode _newMapCode;
        private Vector2 _currentSceneOffsetBefore;
        private Vector2 _currentSceneOffsetAfter;
        private Vector2 _newSceneOffsetBefore;
        private Vector2 _newSceneOffsetAfter;
        private Vector2 _playerPositionBefore;
        private Vector2 _playerPositionAfter;

        public void Init(Scene firstScene)
        {
            StaticData.Scene = CurrentScene = firstScene;
            Player = new Player() { Position = new Vector2(100, 100) };
            CurrentScene.Init(Player);
        }

        public void Update(GameTime gameTime)
        {
            if (!Changing)
            {
                CurrentScene.Update(gameTime);
                return;
            }
            
            
            if (_elapsedChangeTime == 0)
            {
                StartChanging();
                _elapsedChangeTime = 0.0001f;
                return;
            }
            else
            {
                _elapsedChangeTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (_elapsedChangeTime < _LOAD_TIME)
            {
                return;
            }
            else if (_elapsedChangeTime < _LOAD_TIME + _CHANGE_TIME)
            {
                //Sys.Log(_elapsedChangeTime.ToString());
                //Sys.Log("running slow = " + gameTime.IsRunningSlowly);
                float changePercentage = (_elapsedChangeTime - _LOAD_TIME) / _CHANGE_TIME;
                CurrentScene.DrawOffset = Vector2.Lerp(_currentSceneOffsetBefore, _currentSceneOffsetAfter, changePercentage);
                ChangingToScene.DrawOffset = Vector2.Lerp(_newSceneOffsetBefore, _newSceneOffsetAfter, changePercentage);
                Player.Position = Vector2.Lerp(_playerPositionBefore, _playerPositionAfter, changePercentage);
            }
            else
            {
                CurrentScene = StaticData.Scene = ChangingToScene;
                ChangingToScene = null;
                Changing = false;
                CurrentScene.DrawOffset = Vector2.Zero;
                Player.Position = _playerPositionAfter;
                CurrentScene.Start();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            if (!Changing || _elapsedChangeTime == 0)
            {
                CurrentScene.DrawGround(spriteBatch);
                CurrentScene.DrawTop(spriteBatch);
            }
            else
            {
                CurrentScene.DrawGround(spriteBatch);

                StaticData.Scene = ChangingToScene;
                ChangingToScene.DrawGround(spriteBatch);
                StaticData.Scene = CurrentScene;

                CurrentScene.DrawTop(spriteBatch);

                StaticData.Scene = ChangingToScene;
                ChangingToScene.DrawTop(spriteBatch);
                StaticData.Scene = CurrentScene;
            }

            spriteBatch.DrawString(StaticData.Font, Player.Position.ToString(), new Vector2(200, 200), Color.Black);
        }

        public void GoTo(Direction direction, MapCode mapCode)
        {
            _newDirection = direction;
            _newMapCode = mapCode;
            _elapsedChangeTime = 0;
            CurrentScene.Paused = true;
            Changing = true;
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

        private void StartChanging()
        {
            ChangingToScene = MapCodeToScene(_newMapCode);
            ChangingToScene.Paused = true;
            StaticData.Scene = ChangingToScene;
            ChangingToScene.Init(Player);
            ChangingToScene.RegisterHitbox(Player.Hitbox);
            ChangingToScene.RegisterHitbox(Player.SwordHitbox);
            StaticData.Scene = CurrentScene;

            CurrentScene.Remove(Player);
            CurrentScene.Player = null;

            const int playerLength = 14;

            switch (_newDirection)
            {
                case Direction.Up:
                    Player.Position = new Vector2(Player.Position.X, StaticData.NativeHeight);
                    _playerPositionBefore = Player.Position;
                    _playerPositionAfter = new Vector2(_playerPositionBefore.X, StaticData.NativeHeight - playerLength);
                    _currentSceneOffsetBefore = CurrentScene.DrawOffset;
                    _currentSceneOffsetAfter = new Vector2(0, StaticData.NativeHeight);
                    ChangingToScene.DrawOffset = new Vector2(0, -StaticData.NativeHeight);
                    _newSceneOffsetBefore = ChangingToScene.DrawOffset;
                    _newSceneOffsetAfter = new Vector2(0, 0);
                    break;
                case Direction.Down:
                    Player.Position = new Vector2(Player.Position.X, -playerLength);
                    _playerPositionBefore = Player.Position;
                    _playerPositionAfter = new Vector2(_playerPositionBefore.X, 0);
                    _currentSceneOffsetBefore = CurrentScene.DrawOffset;
                    _currentSceneOffsetAfter = new Vector2(0, -StaticData.NativeHeight);
                    ChangingToScene.DrawOffset = new Vector2(0, StaticData.NativeHeight);
                    _newSceneOffsetBefore = ChangingToScene.DrawOffset;
                    _newSceneOffsetAfter = new Vector2(0, 0);
                    break;
                case Direction.Right:
                    Player.Position = new Vector2(-playerLength, Player.Position.Y);
                    _playerPositionBefore = Player.Position;
                    _playerPositionAfter = new Vector2(0, _playerPositionBefore.Y);
                    _currentSceneOffsetBefore = CurrentScene.DrawOffset;
                    _currentSceneOffsetAfter = new Vector2(-StaticData.NativeWidth, 0);
                    ChangingToScene.DrawOffset = new Vector2(StaticData.NativeWidth, 0);
                    _newSceneOffsetBefore = ChangingToScene.DrawOffset;
                    _newSceneOffsetAfter = new Vector2(0, 0);
                    break;
                case Direction.Left:
                    Player.Position = new Vector2(StaticData.NativeWidth, Player.Position.Y);
                    _playerPositionBefore = Player.Position;
                    _playerPositionAfter = new Vector2(StaticData.NativeWidth - playerLength, _playerPositionBefore.Y);
                    _currentSceneOffsetBefore = Vector2.Zero; // jumping to 0,0 cos i don't care to code moving to here :P would look bad anyway
                    _currentSceneOffsetAfter = new Vector2(StaticData.NativeWidth, 0);
                    ChangingToScene.DrawOffset = new Vector2(-StaticData.NativeWidth, 0);
                    _newSceneOffsetBefore = ChangingToScene.DrawOffset;
                    _newSceneOffsetAfter = new Vector2(0, 0);
                    break;
            }
        }
    }
}
