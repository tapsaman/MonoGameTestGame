using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TiledCS;
using System.Linq;

namespace MonoGameTestGame
{
    public abstract class TileMap
    {
        // Width in tiles
        public int Width { get; private set; }
        // Height in tiles
        public int Height { get; private set; }
        // Width in pixels
        public int DrawWidth { get; private set; }
        // Height in pixels
        public int DrawHeight { get; private set; }
        public ushort GroundLayer { get; protected set; } = 0;
        public ushort TopLayer { get; protected set; } = 1;
        public ushort ObjectLayer { get; protected set; } = 2;
        public Dictionary<Direction, Exit> Exits { get; protected set; }
        protected TiledTileset _tileset;
        protected Texture2D _tilesetTexture;
        //protected Dictionary<Direction, MapCode> _nextMaps;
        private int _tileWidth;
        private int _tileHeight;
        private int _tilesetTilesWide;
        private int _tilesetTilesHeight;
        public Dictionary<int, Dictionary<int, Tile>> Tiles;
        public List<Object> Objects;
        private Dictionary<int, TileAnimation> _tileAnimations;

        /*
        Should load fields _map, _tileset, _tilesetTexture, _nextMaps
        */
        public abstract TiledCS.TiledMap Load();

        public TileMap()
        {
            TiledCS.TiledMap map = Load();
            
            _tileWidth = _tileset.TileWidth;
            _tileHeight = _tileset.TileHeight;

            // Amount of tiles on each row (left right)
            _tilesetTilesWide = _tileset.Columns;
            // Amount of tiles on each column (up down)
            _tilesetTilesHeight = _tileset.TileCount / _tileset.Columns;

            Tiles = new Dictionary<int, Dictionary<int, Tile>>();
            _tileAnimations = new Dictionary<int, TileAnimation>();

            Width = map.Width;
            Height = map.Height;
            DrawWidth = Width * _tileWidth;
            DrawHeight = Height * _tileHeight;

            LoadTiles(map, GroundLayer);
            LoadTiles(map, TopLayer);
            LoadObjects(map, ObjectLayer);
        }

        private Rectangle TileFrameToSourceRectangle(int tileFrame)
        {
            int column = tileFrame % _tilesetTilesWide;
            int row = (int)Math.Floor((double)tileFrame / (double)_tilesetTilesWide);

            return new Rectangle(_tileWidth * column, _tileHeight * row, _tileWidth, _tileHeight);
        }

        private string GetPropertyValue(TiledCS.TiledProperty[] properties, string propertyName)
        {
            foreach (var property in properties)
            {
                if (property.name == propertyName)
                    return property.value;
            }
            return null;
        }

        public void Update(GameTime gameTime)
        {
            foreach (var tileAnimation in _tileAnimations.Values)
            {
                tileAnimation.Update(gameTime, Tiles[0]);
            }
        }

        public void Draw(SpriteBatch spriteBatch, int layerIndex, Vector2 drawOffset)
        {
            foreach (var tile in Tiles[layerIndex].Values)
            {
                spriteBatch.Draw(_tilesetTexture, tile.Position + drawOffset, tile.SourceRectangle, Color.White);
            }
        }

        public bool CheckCollision(int x, int y)
        {
            int index = x + y * Width;

            if (Tiles[GroundLayer].ContainsKey(index))
            {
                return Tiles[GroundLayer][index].IsBlocking;
            }

            return false;
        }

        public bool CheckHorizontalCollision(int x, int topY, int bottomY, ref Direction mapBorder)
        {
            if (x < 0)
            {
                mapBorder = Direction.Left;
                return true;
            }
            if (x >= Width)
            {
                mapBorder = Direction.Right;
                return true;
            }

            for (int i = topY; i < bottomY + 1; i++)
            {
                if (CheckCollision(x, i))
                {
                    return true;
                }
            }

            return false;
        }

        public bool CheckVerticalCollision(int y, int leftX, int rightX, ref Direction mapBorder)
        {
            if (y < 0)
            {
                mapBorder = Direction.Up;
                return true;
            }
            if (y >= Height)
            {
                mapBorder = Direction.Down;
                return true;
            }

            for (int i = leftX; i < rightX + 1; i++)
            {
                if (CheckCollision(i, y))
                {
                    return true;
                }
            }
            
            return false;
        }

        public int ConvertX(int x)
        {
            return x / _tileWidth;
        }
        public int ConvertX(float x)
        {
            return (int)Math.Floor(x / _tileWidth);
        }
        public float ConvertTileX(int tileX)
        {
            return tileX * _tileWidth;
        }
        public int ConvertY(int y)
        {
            return y / _tileHeight;
        }
        public int ConvertY(float y)
        {
            return (int)Math.Floor(y / _tileHeight);
        }
        public float ConvertTileY(int tileY)
        {
            return tileY * _tileHeight;
        }
        public Vector2 ConvertTileXY(int tileX, int tileY)
        {
            return new Vector2(ConvertTileX(tileX), ConvertTileY(tileY));
        }

        public class Tile
        {
            public Vector2 Position;
            public Rectangle DrawRectangle;
            public Rectangle SourceRectangle;
            public bool IsBlocking = false;
        }

        public class Object
        {
            public string TypeName;
            public Vector2 Position;
            public string TextProperty;
        }

        public class Exit
        {
            public Direction Direction { get; private set; }
            public MapCode MapCode { get; private set; }
            public TransitionType TransitionType { get; private set; }

            public Exit(Direction direction, MapCode mapCode, TransitionType transitionType)
            {
                Direction = direction;
                MapCode = mapCode;
                TransitionType = transitionType;
            }
        }

        private void LoadTiles(TiledCS.TiledMap map, int layerIndex)
        {
            Tiles[layerIndex] = new Dictionary<int, Tile>();

            for (var i = 0; i < map.Layers[layerIndex].data.Length; i++)
            {
                int gid = map.Layers[layerIndex].data[i];

                // Empty tile, do nothing
                if (gid == 0)
                    continue;

                var tiledTile = map.GetTiledTile(map.Tilesets[0], _tileset, gid);
                Rectangle sourceRectangle = TileFrameToSourceRectangle(gid - 1);

                float x = (i % Width) * _tileWidth;
                float y = (float)Math.Floor(i / (double)Width) * _tileHeight;

                Tile tile = new Tile()
                {
                    Position = new Vector2((int)x, (int)y),
                    DrawRectangle = new Rectangle((int)x, (int)y, _tileWidth, _tileHeight),
                    SourceRectangle = sourceRectangle
                };

                Tiles[layerIndex][i] = tile;

                if (tiledTile != null)
                {
                    // Process Tiled custom properties
                    foreach(var item in tiledTile.properties)
                    {
                        if (item.name == "IsBlocking" && item.value == "true")
                        {
                            tile.IsBlocking = true;
                        }
                    }

                    // Process Tiled animations
                    if (tiledTile.animation.Length != 0)
                    {
                        if (!_tileAnimations.ContainsKey(gid))
                        {
                            // Create new animation
                            var frames = new List<Rectangle>();

                            foreach (var item in tiledTile.animation)
                            {
                                frames.Add(TileFrameToSourceRectangle(item.tileid));
                            }

                            var tileAnimation = new TileAnimation()
                            {
                                FrameDuration = (float)tiledTile.animation[0].duration / 1000,
                                AnimatedTileIndexes = new List<int>() { i },
                                Frames = frames
                            };
                            _tileAnimations[gid] = tileAnimation;
                        }
                        else
                        {
                            // Add tile index to previous animation
                            _tileAnimations[gid].AnimatedTileIndexes.Add(i);
                        }
                    }
                }
            }
        }

        private void LoadObjects(TiledCS.TiledMap map, int layerIndex)
        {
            Objects = new List<Object>();

            foreach (var item in map.Layers[layerIndex].objects)
            {
                Objects.Add(new Object()
                {
                    TypeName = item.name,
                    Position = new Vector2(item.x, item.y),
                    TextProperty = GetPropertyValue(item.properties, "TextProperty")
                });
            }
        }

        private class TileAnimation
        {
            public List<int> AnimatedTileIndexes;
            public List<Rectangle> Frames;
            public float FrameDuration;
            private float _elapsedTime = 0;
            private int _currentIndex = 0;

            public void Update(GameTime gameTime, Dictionary<int, Tile> tiles)
            {
                _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (_elapsedTime > FrameDuration)
                {
                    _elapsedTime = 0;
                    _currentIndex++;

                    if (_currentIndex >= Frames.Count)
                    {
                        _currentIndex = 0;
                    }

                    var frame = Frames[_currentIndex];

                    foreach (var index in AnimatedTileIndexes)
                    {
                        tiles[index].SourceRectangle = frame;
                    }
                }
            }
        }
    }
}