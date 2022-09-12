using System;
using Microsoft.Xna.Framework;
using TapsasEngine.Enums;

namespace TapsasEngine.Maps
{
    public abstract class CollisionTileMap : Map
    {
        public bool Infinite;
        public abstract CollisionType GetCollisionType(int tileX, int tileY);
        public int TileWidth { get; protected set; }
        public int TileHeight { get; protected set; }

        // Used so character don't get stuck on corners
        private const int _DIAGONAL_COLLISION_PUSH = 2;
        
        public int ConvertY(float y)
        {
            return (int)Math.Floor(y / TileHeight);
        }
        public int ConvertX(float x)
        {
            return (int)Math.Floor(x / TileWidth);
        }

        public CollisionType GetTopCollision(FloatRectangle hitbox, Vector2 velocity, ref Direction mapBorder)
        {
            if (Infinite)
                return CollisionType.None;
            
            hitbox = hitbox.AddY(velocity.Y);
            int y = ConvertY(hitbox.Top);

            if (!Infinite && y < 0)
            {
                mapBorder = Direction.Up;
                return CollisionType.Full;
            }

            int leftX = ConvertX(hitbox.Left);
            int centerX = ConvertX(hitbox.Center.X);
            int rightX = ConvertX(hitbox.Right);
            CollisionType topLeft = GetCollisionType(leftX, y);
            CollisionType topCenter = GetCollisionType(centerX, y);
            CollisionType topRight = GetCollisionType(rightX, y);

            if (topLeft == CollisionType.Full || topRight == CollisionType.Full
             || topCenter == CollisionType.Full
             || topLeft == CollisionType.SouthWest || topRight == CollisionType.SouthEast)
            {
                return CollisionType.Full;
            }
            if (topLeft == CollisionType.None && topRight == CollisionType.None)
            {
                return CollisionType.None;
            }

            float yOnTile = hitbox.Top % TileHeight;
            float xOnLeftTile = hitbox.Left % TileWidth - _DIAGONAL_COLLISION_PUSH;
            float xOnRightTile = hitbox.Right % TileWidth + _DIAGONAL_COLLISION_PUSH;

            if (topLeft == CollisionType.SouthEast && yOnTile < TileWidth - xOnLeftTile)
            {
                return CollisionType.SouthEast;
            }
            if (topRight == CollisionType.SouthWest && yOnTile < xOnRightTile)
            {
                return CollisionType.SouthWest;
            }

            return CollisionType.None;
        }

        public CollisionType GetBottomCollision(FloatRectangle hitbox, Vector2 velocity, ref Direction mapBorder)
        {
            if (Infinite)
                return CollisionType.None;
            
            hitbox = hitbox.AddY(velocity.Y);
            var y = ConvertY(hitbox.Bottom);

            if (!Infinite && y >= Height)
            {
                mapBorder = Direction.Down;
                return CollisionType.Full;
            }

            int leftX = ConvertX(hitbox.Left);
            int centerX = ConvertX(hitbox.Center.X);
            int rightX = ConvertX(hitbox.Right);
            CollisionType bottomLeft = GetCollisionType(leftX, y);
            CollisionType bottomCenter = GetCollisionType(centerX, y);
            CollisionType bottomRight = GetCollisionType(rightX, y);

            if (bottomLeft == CollisionType.Full || bottomRight == CollisionType.Full
             || bottomCenter == CollisionType.Full
             || bottomLeft == CollisionType.NorthWest || bottomRight == CollisionType.NorthEast)
            {
                return CollisionType.Full;
            }
            if (bottomLeft == CollisionType.None && bottomRight == CollisionType.None)
            {
                return CollisionType.None;
            }

            float yOnTile = hitbox.Bottom % TileHeight;
            float xOnLeftTile = hitbox.Left % TileWidth - _DIAGONAL_COLLISION_PUSH;
            float xOnRightTile = hitbox.Right % TileWidth + _DIAGONAL_COLLISION_PUSH;

            if (bottomLeft == CollisionType.NorthEast && yOnTile >= xOnLeftTile)
            {
                return CollisionType.NorthEast;
            }
            if (bottomRight == CollisionType.NorthWest && yOnTile >= TileWidth - xOnRightTile)
            {
                return CollisionType.NorthWest;
            }

            return CollisionType.None;
        }

        public CollisionType GetRightCollision(FloatRectangle hitbox, Vector2 velocity, ref Direction mapBorder)
        {
            if (Infinite)
                return CollisionType.None;
            
            hitbox = hitbox.AddX(velocity.X);
            int x = ConvertX(hitbox.Right);

            if (!Infinite && x >= Width)
            {
                mapBorder = Direction.Right;
                return CollisionType.Full;
            }

            int topY = ConvertY(hitbox.Top);
            int centerY = ConvertY(hitbox.Center.Y);
            int bottomY = ConvertY(hitbox.Bottom);
            CollisionType topRight = GetCollisionType(x, topY);
            CollisionType centerRight = GetCollisionType(x, centerY);
            CollisionType bottomRight = GetCollisionType(x, bottomY);

            if (topRight == CollisionType.Full || bottomRight == CollisionType.Full
             || centerRight == CollisionType.Full
             || topRight == CollisionType.NorthWest || bottomRight == CollisionType.SouthWest)
            {
                return CollisionType.Full;
            }
            if (topRight == CollisionType.None && bottomRight == CollisionType.None)
            {
                return CollisionType.None;
            }

            float xOnTile = hitbox.Right % TileWidth;
            float yOnTopTile = hitbox.Top % TileHeight - _DIAGONAL_COLLISION_PUSH;
            float yOnBottomTile = hitbox.Bottom % TileHeight + _DIAGONAL_COLLISION_PUSH;

            if (topRight == CollisionType.SouthWest && xOnTile > yOnTopTile)
            {
                return CollisionType.SouthWest;
            }
            if (bottomRight == CollisionType.NorthWest && xOnTile > TileHeight - yOnBottomTile)
            {
                return CollisionType.NorthWest;
            }

            return CollisionType.None;
        }

        public CollisionType GetLeftCollision(FloatRectangle hitbox, Vector2 velocity, ref Direction mapBorder)
        {
            if (Infinite)
                return CollisionType.None;
            
            hitbox = hitbox.AddX(velocity.X);
            int x = ConvertX(hitbox.Left);

            if (!Infinite && x < 0)
            {
                mapBorder = Direction.Left;
                return CollisionType.Full;
            }

            int topY = ConvertY(hitbox.Top);
            int centerY = ConvertY(hitbox.Center.Y);
            int bottomY = ConvertY(hitbox.Bottom);
            CollisionType topLeft = GetCollisionType(x, topY);
            CollisionType centerLeft = GetCollisionType(x, centerY);
            CollisionType bottomLeft = GetCollisionType(x, bottomY);

            if (topLeft == CollisionType.Full || bottomLeft == CollisionType.Full
             || centerLeft == CollisionType.Full
             || topLeft == CollisionType.NorthEast || bottomLeft == CollisionType.SouthEast)
            {
                return CollisionType.Full;
            }
            if (topLeft == CollisionType.None && bottomLeft == CollisionType.None)
            {
                return CollisionType.None;
            }

            float xOnTile = hitbox.Left % TileWidth;
            float yOnTopTile = hitbox.Top % TileHeight - _DIAGONAL_COLLISION_PUSH;
            float yOnBottomTile = hitbox.Bottom % TileHeight + _DIAGONAL_COLLISION_PUSH;

            if (topLeft == CollisionType.SouthEast && xOnTile < TileHeight - yOnTopTile)
            {
                return CollisionType.SouthEast;
            }
            if (bottomLeft == CollisionType.NorthEast && xOnTile < yOnBottomTile)
            {
                return CollisionType.NorthEast;
            }

            return CollisionType.None;
        }
    }
}