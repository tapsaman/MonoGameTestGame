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
        public const float MoveLength = 30f;
        private Vector2 _moveStartPosition;
        private float _elapsedMoveTime = 0f;
        public SwordHitbox(GraphicsDeviceManager graphics, int width, int height)
            : base(graphics, width, height)
        {
            Enabled = false;
        }

        public void StartHit(Vector2 position, string direction)
        {
            Enabled = true;
            Position = position;
            _moveStartPosition = Position;
            _elapsedMoveTime = 0;
            //if (Direction) 
        }

        public void Update(GameTime gameTime)
        {
            if (Enabled)
            {
                Position.Y = _moveStartPosition.Y + MoveLength * (_elapsedMoveTime / MoveTime);
                _elapsedMoveTime = Math.Min(_elapsedMoveTime + (float)gameTime.ElapsedGameTime.TotalSeconds, MoveTime);
            }
        }

        public void EndHit()
        {
            Enabled = false;
        }
    }
}