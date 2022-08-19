using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TiledCS;
using TapsasEngine.Enums;
using TapsasEngine.Maps;

namespace ZA6
{
    public class TileMap : CollisionTileMap
    {
        // Width in pixels
        public string Name;
        public int DrawWidth { get; private set; }
        // Height in pixels
        public int DrawHeight { get; private set; }
        public Vector2 TileSize { get; private set; }
        public ushort GroundLayer { get; protected set; } = 0;
        public ushort TopLayer { get; protected set; } = 1;
        public ushort ObjectLayer { get; protected set; } = 2;
        public Vector2 PlayerStartPosition { get; protected set; } = Vector2.Zero;
        public Dictionary<Direction, MapExit> Exits;
        public TiledTileset Tileset;
        public Texture2D TilesetTexture;
        public Texture2D CollisionTileTexture;
        private int TilesetTilesWide;
        private int TilesetTilesHeight;
        public Dictionary<int, Dictionary<int, Tile>> Tiles;
        public List<Object> Objects;
        private Dictionary<int, TileAnimation> _tileAnimations;
        private static Dictionary<CollisionType, Rectangle> _collisionTypeRectangles;
        private static Color _collisionTileColor = new Color(120, 120, 120, 120);
    
        public void Load(TiledCS.TiledMap map)
        {
            CollisionTileTexture = Static.Content.Load<Texture2D>("CollisionTiles");
            _collisionTypeRectangles = new Dictionary<CollisionType, Rectangle> 
            {
                { CollisionType.None, new Rectangle(0, 0, 8, 8) },
                { CollisionType.Full, new Rectangle(0, 8, 8, 8) },
                { CollisionType.NorthWest, new Rectangle(8, 0, 8, 8) },
                { CollisionType.NorthEast, new Rectangle(16, 0, 8, 8) },
                { CollisionType.SouthWest, new Rectangle(8, 8, 8, 8) },
                { CollisionType.SouthEast, new Rectangle(16, 8, 8, 8) }
            };

            Width = map.Width;
            Height = map.Height;
            TileWidth = Tileset.TileWidth;
            TileHeight = Tileset.TileHeight;
            TileSize = new Vector2(TileWidth, TileHeight);
            DrawWidth = Width * TileWidth;
            DrawHeight = Height * TileHeight;

            // Amount of tiles on each row (left right)
            TilesetTilesWide = Tileset.Columns;
            // Amount of tiles on each column (up down)
            TilesetTilesHeight = Tileset.TileCount / Tileset.Columns;

            Tiles = new Dictionary<int, Dictionary<int, Tile>>();
            _tileAnimations = new Dictionary<int, TileAnimation>();

            LoadTiles(map, GroundLayer);
            LoadTiles(map, TopLayer);
            LoadObjects(map, ObjectLayer);
        }

        private Rectangle TileFrameToSourceRectangle(int tileFrame)
        {
            int column = tileFrame % TilesetTilesWide;
            int row = (int)Math.Floor((double)tileFrame / (double)TilesetTilesWide);

            return new Rectangle(TileWidth * column, TileHeight * row, TileWidth, TileHeight);
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
                spriteBatch.Draw(TilesetTexture, tile.Position + drawOffset, tile.SourceRectangle, Color.White);
            }
            if (Static.RenderCollisionMap)
            {
                foreach (Tile tile in Tiles[layerIndex].Values)
                {
                    spriteBatch.Draw(CollisionTileTexture, tile.Position + drawOffset, tile.CollisionTileRectangle, _collisionTileColor);
                }
            }
        }

        public override CollisionType GetCollisionType(int x, int y)
        {
            int index = x + y * Width;

            if (Tiles[GroundLayer].ContainsKey(index))
            {
                return Tiles[GroundLayer][index].CollisionShape;
            }

            return CollisionType.None;
        }
        
        public float ConvertTileX(int tileX)
        {
            return tileX * TileWidth;
        }
        
        public float ConvertTileY(int tileY)
        {
            return tileY * TileHeight;
        }
        public Vector2 ConvertTileXY(int tileX, int tileY)
        {
            return new Vector2(ConvertTileX(tileX), ConvertTileY(tileY));
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

                var tiledTile = map.GetTiledTile(map.Tilesets[0], Tileset, gid);
                Rectangle sourceRectangle = TileFrameToSourceRectangle(gid - 1);

                float x = (i % Width) * TileWidth;
                float y = (float)Math.Floor(i / (double)Width) * TileHeight;

                Tile tile = new Tile()
                {
                    Position = new Vector2((int)x, (int)y),
                    DrawRectangle = new Rectangle((int)x, (int)y, TileWidth, TileHeight),
                    SourceRectangle = sourceRectangle
                };

                Tiles[layerIndex][i] = tile;

                if (tiledTile != null)
                {
                    // Process Tiled custom properties
                    foreach(var item in tiledTile.properties)
                    {
                        /*if (item.name == "IsBlocking" && item.value == "true")
                        {
                            tile.IsBlocking = true;
                        }*/
                        if (item.name == "CollisionShape")
                        {
                            tile.CollisionShape = (CollisionType)Int16.Parse(item.value);
                            tile.CollisionTileRectangle = _collisionTypeRectangles[tile.CollisionShape];
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
                if (item.name == "PlayerStart")
                    PlayerStartPosition = new Vector2(item.x, item.y);
                
                Objects.Add(new Object()
                {
                    TypeName = item.name,
                    Position = new Vector2(item.x, item.y),
                    TextProperty = GetPropertyValue(item.properties, "TextProperty")
                });
            }
        }


        private void CollisionTypeToSourceRectangle()
        {

        }

        public class Tile
        {
            public Vector2 Position;
            public Rectangle DrawRectangle;
            public Rectangle SourceRectangle;
            public bool IsBlocking = false;
            public CollisionType CollisionShape;
            public Rectangle CollisionTileRectangle;
        }

        public class Object
        {
            public string TypeName;
            public Vector2 Position;
            public string TextProperty;
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