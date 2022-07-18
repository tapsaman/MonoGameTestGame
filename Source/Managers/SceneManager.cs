using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameTestGame.Manangers
{
    public class SceneManager
    {
        public Player Player;
        public bool Changing;
        public Scene CurrentScene;
        public Scene ChangingToScene;
        private float _elapsedChangeTime;
        private const float _CHANGE_TIME = 2f;
        private Vector2 _currentSceneOffsetBefore;
        private Vector2 _currentSceneOffsetAfter;
        private Vector2 _newSceneOffsetBefore;
        private Vector2 _newSceneOffsetAfter;
        private Vector2 _playerPositionBefore;
        private Vector2 _playerPositionAfter;
        public void GoTo(Direction direction, MapCode mapCode)
        {
            _elapsedChangeTime = 0;
            ChangingToScene = MapCodeToScene(mapCode);

            CurrentScene.Paused = true;
            ChangingToScene.Paused = true;
            Changing = true;
            CurrentScene.Player = null;

            switch (direction)
            {
                case Direction.Right:
                    Player.Position = new Vector2(-14 ,Player.Position.Y);
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
                    _playerPositionAfter = new Vector2(StaticData.NativeWidth - 14, _playerPositionBefore.Y);
                    _currentSceneOffsetBefore = CurrentScene.DrawOffset;
                    _currentSceneOffsetAfter = new Vector2(StaticData.NativeWidth, 0);
                    ChangingToScene.DrawOffset = new Vector2(-StaticData.NativeWidth, 0);
                    _newSceneOffsetBefore = ChangingToScene.DrawOffset;
                    _newSceneOffsetAfter = new Vector2(0, 0);
                    break;
            }
        }

        public Scene MapCodeToScene(MapCode mapCode)
        {
            switch (mapCode)
            {
                case MapCode.B1:
                    return new SceneB1(Player);
                case MapCode.A1:
                default:
                    return new TestScene(Player);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (!Changing)
            {
                CurrentScene.Update(gameTime);
                return;
            }
            
            _elapsedChangeTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_elapsedChangeTime < _CHANGE_TIME)
            {
                float changePercentage = (_elapsedChangeTime / _CHANGE_TIME);
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
            CurrentScene.Draw(spriteBatch);

            if (Changing)
            {
                StaticData.Scene = ChangingToScene;
                ChangingToScene.Draw(spriteBatch);
                StaticData.Scene = CurrentScene;
            }

            spriteBatch.DrawString(StaticData.Font, Player.Position.ToString(), new Vector2(200, 200), Color.Black);
        }
    }
}
