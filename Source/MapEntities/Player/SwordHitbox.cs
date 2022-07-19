using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TiledCS;

namespace MonoGameTestGame
{
    public class SwordHitbox : Hitbox
    {
        public const float MoveTime = 0.3f;
        public const float MoveLength = 21f;
        private Vector2 _moveStartPosition;
        private float _elapsedMoveTime = 0f;
        private int _setWidth;
        private int _setHeight;
        private Direction _direction;
        public SwordHitbox(int width, int height)
            : base(width, height)
        {
            Enabled = false;
            _setWidth = width;
            _setHeight = height;
        }

        public void StartHit(FloatRectangle playerRectangle, Direction direction)
        {
            Enabled = true;
            _direction = direction;
            _elapsedMoveTime = 0;

            switch (_direction)
            {
                case Direction.Up:
                    Load(_setWidth, _setHeight);
                    Position = new Vector2(playerRectangle.Center.X + MoveLength / 2 - _setWidth / 2, playerRectangle.Top - _setHeight);
                    break;
                case Direction.Down:
                    Load(_setWidth, _setHeight);
                    Position = new Vector2(playerRectangle.Center.X - MoveLength / 2  - _setWidth / 2, playerRectangle.Bottom);
                    break;
                case Direction.Right:
                    Load(_setHeight, _setWidth);
                    Position = new Vector2(playerRectangle.Right, playerRectangle.Center.Y - MoveLength / 2 - _setWidth / 2);
                    break;
                case Direction.Left:
                    Load(_setHeight, _setWidth);
                    Position = new Vector2(playerRectangle.Left - _setHeight, playerRectangle.Center.Y - MoveLength / 2 - _setWidth / 2);
                    break;
            }

            _moveStartPosition = Position;
        }

        public void Update(GameTime gameTime)
        {
            if (Enabled)
            {
                _elapsedMoveTime = Math.Min(_elapsedMoveTime + (float)gameTime.ElapsedGameTime.TotalSeconds, MoveTime);
                //Position.Y = _moveStartPosition.Y + MoveLength * (_elapsedMoveTime / MoveTime);
                switch (_direction)
                {
                    case Direction.Up:
                        Position.X = _moveStartPosition.X - MoveLength * (_elapsedMoveTime / MoveTime);
                        break;
                    case Direction.Down:
                        Position.X = _moveStartPosition.X + MoveLength * (_elapsedMoveTime / MoveTime);
                        break;
                    case Direction.Right:
                    case Direction.Left:
                        Position.Y = _moveStartPosition.Y + MoveLength * (_elapsedMoveTime / MoveTime);
                        break;

                }
            }
        }

        public void EndHit()
        {
            Enabled = false;
        }
    }
}