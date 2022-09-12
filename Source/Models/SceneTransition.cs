using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZA6.Manangers;
using TapsasEngine.Enums;
using TapsasEngine.Utilities;

namespace ZA6
{
    public abstract class SceneTransition
    {
        public bool Done { get; protected set; }
        public SceneManager SceneManager;
        protected Scene _scene1;
        protected Scene _scene2;
        protected Player _player;
        public abstract void Start(Scene scene1, Player player, Direction direction);
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);


        public class Pan : SceneTransition
        {
            private const float _LOAD_TIME = 0.5f;
            private const float _CHANGE_TIME = 0.8f;
            private float _elapsedTime;
            private Vector2 _scene1OffsetBefore;
            private Vector2 _scene1OffsetAfter;
            private Vector2 _newSceneOffsetBefore;
            private Vector2 _newSceneOffsetAfter;
            private Vector2 _playerPositionBefore;
            private Vector2 _playerPositionAfter;

            public override void Start(Scene scene1, Player player, Direction direction)
            {
                _scene1 = scene1;
                _scene1.Paused = true;
                _scene2 = SceneManager.LoadNextScene();
                _player = player;

                const int playerLength = 14;

                switch (direction)
                {
                    case Direction.Up:
                        _playerPositionBefore = new Vector2(_player.Position.X, _scene2.Height);
                        _playerPositionAfter = new Vector2(_playerPositionBefore.X, _scene2.Height - playerLength);
                        _scene1OffsetBefore = _scene1.Camera.Offset;
                        _scene1OffsetAfter = new Vector2(_scene1.Camera.Offset.X, Static.NativeHeight);
                        _scene2.Camera.Offset = new Vector2(_scene1.Camera.Offset.X, -Static.NativeHeight);
                        _newSceneOffsetBefore = _scene2.Camera.Offset;
                        _newSceneOffsetAfter = new Vector2(_scene1.Camera.Offset.X, 0);
                        break;
                    case Direction.Down:
                        _playerPositionBefore = new Vector2(_player.Position.X, -playerLength);
                        _playerPositionAfter = new Vector2(_playerPositionBefore.X, 0);
                        _scene1OffsetBefore = _scene1.Camera.Offset;
                        _scene1OffsetAfter = new Vector2(_scene1.Camera.Offset.X, _scene1.Camera.Offset.Y - Static.NativeHeight);
                        _scene2.Camera.Offset = new Vector2(_scene1.Camera.Offset.X, Static.NativeHeight);
                        _newSceneOffsetBefore = _scene2.Camera.Offset;
                        _newSceneOffsetAfter = new Vector2(_scene1.Camera.Offset.X, 0);
                        break;
                    case Direction.Right:
                        _playerPositionBefore = new Vector2(-playerLength, _player.Position.Y);
                        _playerPositionAfter = new Vector2(0, _playerPositionBefore.Y);
                        _scene1OffsetBefore = _scene1.Camera.Offset;
                        _scene1OffsetAfter = new Vector2(_scene1.Camera.Offset.X - Static.NativeWidth, _scene1.Camera.Offset.Y);
                        _scene2.Camera.Offset = new Vector2(Static.NativeWidth, _scene1.Camera.Offset.Y);
                        _newSceneOffsetBefore = _scene2.Camera.Offset;
                        _newSceneOffsetAfter = new Vector2(0, _newSceneOffsetBefore.Y);
                        break;
                    case Direction.Left:
                        _playerPositionBefore = new Vector2(_scene2.Width, _player.Position.Y);
                        _playerPositionAfter = new Vector2(_scene2.Width - playerLength, _playerPositionBefore.Y);
                        _scene1OffsetBefore = _scene1.Camera.Offset;
                        _scene1OffsetAfter = new Vector2(Static.NativeWidth, _scene1.Camera.Offset.Y);
                        _scene2.Camera.Offset = new Vector2(Static.NativeWidth - _scene2.Width - Static.NativeWidth, _scene1.Camera.Offset.Y);
                        _newSceneOffsetBefore = _scene2.Camera.Offset;
                        _newSceneOffsetAfter = new Vector2(Static.NativeWidth - _scene2.Width, _newSceneOffsetBefore.Y);
                        break;
                }
            }

            public override void Update(GameTime gameTime)
            {
                _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (_elapsedTime < _LOAD_TIME)
                {
                    return;
                }
                else if (_elapsedTime < _LOAD_TIME + _CHANGE_TIME)
                {
                    //Sys.Log(_elapsedTime.ToString());
                    //Sys.Log("running slow = " + gameTime.IsRunningSlowly);
                    float changePercentage = (_elapsedTime - _LOAD_TIME) / _CHANGE_TIME;
                    _scene1.Camera.Offset = Vector2.Lerp(_scene1OffsetBefore, _scene1OffsetAfter, changePercentage);
                    _scene2.Camera.Offset = Vector2.Lerp(_newSceneOffsetBefore, _newSceneOffsetAfter, changePercentage);
                    _player.Position = Vector2.Lerp(_playerPositionBefore, _playerPositionAfter, changePercentage);
                }
                else
                {
                    _scene2.Camera.Offset = _newSceneOffsetAfter;
                    _scene2.OverlayOffset = _scene1OffsetAfter * 0.5f; //- _scene1OffsetAfter * 0.5f + Static.NativeSize;
                    _player.Position = _playerPositionAfter;
                    Done = true;
                }
            }

            public override void Draw(SpriteBatch spriteBatch)
            {

                if (_elapsedTime < _LOAD_TIME)
                {   
                    _scene1.DrawGround(spriteBatch);
                    _scene1.DrawTop(spriteBatch);
                    _scene1.DrawOverlay(spriteBatch);
                }
                else
                {
                    _scene1.DrawGround(spriteBatch);

                    Static.Scene = _scene2;
                    _scene2.DrawGround(spriteBatch);
                    Static.Scene = _scene1;

                    _scene1.DrawTop(spriteBatch);

                    Static.Scene = _scene2;
                    _scene2.DrawTop(spriteBatch);
                    //_scene2.DrawOverlay(spriteBatch);
                    Static.Scene = _scene1;

                    _scene1.DrawOverlay(spriteBatch);
                }
            }
        }

        public class FadeToBlack : SceneTransition
        {
            private const float _LOAD_TIME = 1f;
            private const float _FADE_TIME = 1f;
            private float _elapsedTime;
            private Vector2 _playerPositionAfter;
            private Direction _direction;
            private Texture2D _overlay;
            private float _colorMultiplier = 0f;
            private Animations.Walk _walkAnimation;

            public override void Start(Scene scene1, Player player, Direction direction)
            {
                _direction = direction;
                _scene1 = scene1;
                _player = player;
                _overlay = Utility.CreateColorTexture(Static.NativeWidth, Static.NativeHeight, Color.Black);

                _player.NoClip = true;
                const int playerLength = 14;
                _walkAnimation = new Animations.Walk(_player, direction.ToVector() * playerLength);
                _walkAnimation.Enter();
            }

            public override void Update(GameTime gameTime)
            {
                if (!_walkAnimation.IsDone)
                {
                    _walkAnimation.Update(gameTime);
                    _scene1.Update(gameTime);
                    return;
                }

                _scene1.Paused = true;
                _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                
                if (_elapsedTime < _FADE_TIME)
                {
                    _colorMultiplier = (_elapsedTime) / _FADE_TIME;
                }
                else if (_elapsedTime < _FADE_TIME + _LOAD_TIME)
                {
                    if (_scene2 == null)
                    {
                        _player.NoClip = false;
                        _scene2 = SceneManager.LoadNextScene();
                        _playerPositionAfter = new Vector2(_scene2.Width - 14, _player.Position.Y);

                        const int playerLength = 14;

                        switch (_direction)
                        {
                            case Direction.Up:
                                _playerPositionAfter = new Vector2(_player.Position.X, Static.NativeHeight - playerLength);
                                break;
                            case Direction.Down:
                                _playerPositionAfter = new Vector2(_player.Position.X, 0);
                                break;
                            case Direction.Right:
                                _playerPositionAfter = new Vector2(0, _player.Position.Y);
                                break;
                            case Direction.Left:
                                _playerPositionAfter = new Vector2(_scene2.Width - 14, _player.Position.Y);
                                break;
                        }

                        _player.Position = _playerPositionAfter;
                        _scene2.UpdateCamera(_player.Position);
                        
                        //if (_scene1.Theme != _scene2.Theme)
                        Music.FadeOutTo(_scene2.Theme, _FADE_TIME + _LOAD_TIME);
                    }
                }
                else if (_elapsedTime < _FADE_TIME * 2 + _LOAD_TIME)
                {
                    _colorMultiplier = (_FADE_TIME - (_elapsedTime - _FADE_TIME - _LOAD_TIME)) / _FADE_TIME;
                }
                else
                {
                    _overlay.Dispose();
                    Done = true;
                }
            }

            public override void Draw(SpriteBatch spriteBatch)
            {
                if (_elapsedTime < _FADE_TIME)
                {
                    _scene1.DrawGround(spriteBatch);
                    _scene1.DrawTop(spriteBatch);
                    _scene1.DrawOverlay(spriteBatch);
                }
                else if (_elapsedTime >= _FADE_TIME + _LOAD_TIME)
                {
                    _scene2.DrawGround(spriteBatch);
                    _scene2.DrawTop(spriteBatch);
                    _scene2.DrawOverlay(spriteBatch);
                }

                spriteBatch.Draw(_overlay, Vector2.Zero, Color.Black * _colorMultiplier);
            }
        }

        public class Doorway : SceneTransition
        {
            private const float _LOAD_TIME = 1f;
            private float _elapsedTime;
            private Animations.Spotlight _spotlightClose;
            private Animations.Spotlight _spotlightOpen;

            public override void Start(Scene scene1, Player player, Direction direction)
            {
                _scene1 = scene1;
                _player = player;
                _spotlightClose = new Animations.Spotlight(_player, true);
                _spotlightOpen = new Animations.Spotlight(_player, false);

                _spotlightClose.Enter();
            }

            public override void Update(GameTime gameTime)
            {
                if (!_spotlightClose.IsDone)
                {
                    _spotlightClose.Update(gameTime);
                    _scene1.Update(gameTime);
                    return;
                }

                _scene1.Paused = true;
                
                if (_scene2 == null)
                {
                    _scene2 = SceneManager.LoadNextScene();
                    
                    _player.Position = GetStartPosition(_scene2);
                    _scene2.UpdateCamera(_player.Position);
                    _spotlightOpen.Enter();

                    if (_scene2.Theme != null)
                        Music.FadeOutTo(_scene2.Theme, _LOAD_TIME);
                }
                else if (_elapsedTime < _LOAD_TIME)
                {
                    _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else if (!_spotlightOpen.IsDone)
                {
                    _spotlightOpen.Update(gameTime);
                }
                else
                {
                    Done = true;
                }
            }

            public override void Draw(SpriteBatch spriteBatch)
            {
                if (_scene2 == null)
                {
                    _scene1.DrawGround(spriteBatch);
                    _scene1.DrawTop(spriteBatch);
                    _scene1.DrawOverlay(spriteBatch);
                }
                else
                {
                    _scene2.DrawGround(spriteBatch);
                    _scene2.DrawTop(spriteBatch);
                    _scene2.DrawOverlay(spriteBatch);
                }
            }
        }

        private Vector2 GetStartPosition(Scene scene)
        {
            foreach (var obj in scene.TileMap.Objects)
            {
                if (obj.TypeName == "Doorway" && obj.TextProperty == _scene1.TileMap.Name)
                    return obj.Position;
            }

            return scene.TileMap.PlayerStartPosition;
        }

        public class GoToRitual : SceneTransition
        {
            private const float _LOAD_TIME = 1f;
            private const float _FADE_TIME = 1f;
            private float _elapsedTime;
            private Texture2D _overlay;
            private float _colorMultiplier = 0f;
            private Animations.Walk _walkAnimation;

            public override void Start(Scene scene1, Player player, Direction direction)
            {
                _scene1 = scene1;
                _player = player;
                _overlay = Utility.CreateColorTexture(Static.NativeWidth, Static.NativeHeight, Color.Black);

                _player.NoClip = true;
                var playerLength = 14;
                _walkAnimation = new Animations.Walk(_player, direction.ToVector() * playerLength);
                _walkAnimation.Enter();
            }

            public override void Update(GameTime gameTime)
            {
                if (!_walkAnimation.IsDone)
                {
                    _walkAnimation.Update(gameTime);
                    _scene1.Update(gameTime);
                    return;
                }

                _scene1.Paused = true;
                _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                
                if (_elapsedTime < _FADE_TIME)
                {
                    _colorMultiplier = (_elapsedTime) / _FADE_TIME;
                }
                else if (_elapsedTime < _FADE_TIME + _LOAD_TIME)
                {
                    if (_scene2 == null)
                    {
                        _player.NoClip = false;
                        _scene2 = SceneManager.LoadNextScene();
                        _player.Position = _scene2.TileMap.PlayerStartPosition;
                        _scene2.UpdateCamera(_player.Position);
                        
                        //if (_scene1.Theme != _scene2.Theme)
                        //Music.FadeOutTo(_scene2.Theme, _FADE_TIME + _LOAD_TIME);
                    }
                }
                else if (_elapsedTime < _FADE_TIME * 2 + _LOAD_TIME)
                {
                    _colorMultiplier = (_FADE_TIME - (_elapsedTime - _FADE_TIME - _LOAD_TIME)) / _FADE_TIME;
                }
                else
                {
                    _overlay.Dispose();
                    Done = true;
                }
            }

            public override void Draw(SpriteBatch spriteBatch)
            {
                if (_elapsedTime < _FADE_TIME)
                {
                    _scene1.DrawGround(spriteBatch);
                    _scene1.DrawTop(spriteBatch);
                    _scene1.DrawOverlay(spriteBatch);
                }
                else if (_elapsedTime >= _FADE_TIME + _LOAD_TIME)
                {
                    _scene2.DrawGround(spriteBatch);
                    _scene2.DrawTop(spriteBatch);
                    _scene2.DrawOverlay(spriteBatch);
                }

                spriteBatch.Draw(_overlay, Vector2.Zero, Color.Black * _colorMultiplier);
            }
        }
    }
}