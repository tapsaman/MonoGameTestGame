using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TiledCS;
using TapsasEngine.Enums;
using TapsasEngine.Maps;
using System.Linq;
using TapsasEngine;

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
        //public ushort GroundLayer { get; protected set; } = 0;
        //public ushort TopLayer { get; protected set; } = 1;
        //public ushort ObjectLayer { get; protected set; } = 2;
        public Vector2 PlayerStartPosition { get; protected set; } = Vector2.Zero;
        public Dictionary<Direction, MapExit> Exits;
        public TiledTileset Tileset;
        public Texture2D TilesetTexture;
        public Texture2D CollisionTileTexture;
        private int TilesetTilesWide;
        private int TilesetTilesHeight;
        //public Dictionary<int, Dictionary<int, Tile>> Tiles;
        public Tile[] GroundTiles;
        public Tile[] TopTiles;
        public string[] UseAlternativeLayers;
        public Object[] Objects;
        private TiledMap _map;
        private Dictionary<int, TileAnimation> _tileAnimations;
        private static Dictionary<CollisionType, Rectangle> _collisionTypeRectangles;
        private static Color _collisionTileColor = new Color(120, 120, 120, 120);
    
        public void Load(TiledCS.TiledMap map)
        {
            CollisionTileTexture = Static.Content.Load<Texture2D>("TiledProject/CollisionTiles");
            _collisionTypeRectangles = new Dictionary<CollisionType, Rectangle> 
            {
                { CollisionType.None, new Rectangle(0, 0, 8, 8) },
                { CollisionType.Full, new Rectangle(0, 8, 8, 8) },
                { CollisionType.NorthWest, new Rectangle(8, 0, 8, 8) },
                { CollisionType.NorthEast, new Rectangle(16, 0, 8, 8) },
                { CollisionType.SouthWest, new Rectangle(8, 8, 8, 8) },
                { CollisionType.SouthEast, new Rectangle(16, 8, 8, 8) }
            };

            _map = map;
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

            _tileAnimations = new Dictionary<int, TileAnimation>();

            for (int i = 0; i < map.Layers.Length; i++)
            {
                var mapLayer = map.Layers[i];

                if (mapLayer.name == "Ground")
                {
                    var alternativeIndexes = UseAlternativeLayers.Select(
                        (string layerId) => {
                            int index = Array.FindIndex<TiledLayer>(map.Layers, (TiledLayer l) => l.name == "Alternative" + layerId);

                            if (index == -1)
                                throw new Exception("No layer 'Alternative" + layerId + "' found");
                        
                            return index;
                        }
                    );

                    GroundTiles = LoadTiles(map, i, alternativeIndexes.ToArray());
                }
                else if (mapLayer.name == "Top")
                {
                    TopTiles = LoadTiles(map, i);
                }
                else if (mapLayer.name == "Objects")
                {
                    Objects = LoadObjects(map, i);
                }
            }
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
                tileAnimation.Update(gameTime, GroundTiles);
            }
        }

        public void _DrawGround(SpriteBatch spriteBatch, Vector2 drawOffset)
        {
            foreach (Tile tile in GroundTiles)
            {
                if (tile != null)
                    spriteBatch.Draw(TilesetTexture, tile.Position + drawOffset, tile.SourceRectangle, Color.White);
            }
        }

        public void DrawJankyGround(SpriteBatch spriteBatch, Vector2 drawOffset)
        {
            // Only draw shown tiles

            float x = -drawOffset.X;
            float endX = x + Static.NativeWidth + TileWidth;
            int tileX = (int)(x < 0
                ? ((Static.NativeWidth + x) / TileWidth) % Width
                : (x / TileWidth) % Width);

            while (x < endX)
            {
                float y = -drawOffset.Y;
                float endY = y + Static.NativeHeight + TileHeight;
                int tileY = (int)((y < 0 ? Static.NativeHeight - y : y) / TileHeight) % Height;

                while (y < endY)
                {
                    Tile tile = GroundTiles[tileX + tileY * Width];

                    if (tile != null)
                    {
                        spriteBatch.Draw(TilesetTexture, new Vector2(x, y) + drawOffset, tile.SourceRectangle, Color.White);
                    }

                    y += TileHeight;
                    tileY = (tileY + 1) % Height;
                }

                x += TileWidth;
                tileX = (tileX + 1) % Width;
            }
        }


        public void DrawGround(SpriteBatch spriteBatch, Rectangle screen)
        {
            // Only draw shown tiles

            // TODO make do without local drawOffset
            Vector2 drawOffset = -screen.Location.ToVector2();
            int drawn = 0;
            float x, endX;
            int tileX;

            if (Infinite)
            {
                x = drawOffset.X > 0
                    ? (drawOffset.X % TileWidth) - TileWidth
                    : drawOffset.X % TileWidth;
                endX = x + screen.Width + TileWidth;
                tileX = (int)(drawOffset.X > 0
                    ? ((DrawWidth - (drawOffset.X % DrawWidth)) / TileWidth) % Width
                    : (-drawOffset.X / TileWidth) % Width);
            }
            else
            {
                x = drawOffset.X > 0 ? drawOffset.X : drawOffset.X % TileWidth;
                endX = screen.Width;
                tileX = drawOffset.X > 0 ? 0 : (int)-drawOffset.X / TileWidth;
            }

            while (x < endX)
            {
                float y, endY;
                int tileY;

                if (Infinite)
                {
                    y = drawOffset.Y > 0
                        ? (drawOffset.Y % TileHeight) - TileHeight
                        : drawOffset.Y % TileHeight;
                    endY = y + screen.Height + TileHeight;
                    tileY = (int)(drawOffset.Y > 0
                        ? ((DrawHeight - (drawOffset.Y % DrawHeight)) / TileHeight) % Height
                        : (-drawOffset.Y / TileHeight) % Height);
                }
                else 
                {
                    y = drawOffset.Y > 0 ? drawOffset.Y : drawOffset.Y % TileHeight;
                    endY = screen.Height;
                    tileY = drawOffset.Y > 0 ? 0 : (int)-drawOffset.Y / TileHeight;
                }

                

                //float y = drawOffset.Y % TileHeight;
                //float endY = y + Static.NativeHeight + TileHeight;
                //int tileY = (int)((y < 0 ? DrawHeight - y : y) / TileHeight) % Height;

                while (y < endY)
                {
                    Tile tile = GroundTiles[tileX + tileY * Width];
                    drawn++;

                    if (tile != null)
                    {
                        spriteBatch.Draw(TilesetTexture, new Vector2(x, y), tile.SourceRectangle, Color.White);
                    }

                    y += TileHeight;
                    tileY += 1;

                    if (tileY >= Height)
                    {
                        if (Infinite)
                            tileY = 0;
                        else
                            break;
                    }
                }

                x += TileWidth;
                tileX += 1;

                if (tileX >= Width)
                {
                    if (Infinite)
                        tileX = 0;
                    else
                        break;
                }
            }

            Sys.Log(Name + " drawn tiles count " + drawn);
        }

        public void DrawTop(SpriteBatch spriteBatch, Vector2 drawOffset)
        {
            foreach (Tile tile in TopTiles)
            {
                if (tile != null)
                    spriteBatch.Draw(TilesetTexture, tile.Position + drawOffset, tile.SourceRectangle, Color.White);
            }

            if (Static.RenderCollisionMap)
            {
                foreach (Tile tile in GroundTiles)
                {
                    if (tile != null)
                        spriteBatch.Draw(CollisionTileTexture, tile.Position + drawOffset, tile.CollisionTileRectangle, _collisionTileColor);
                }
            }
        }

        public override CollisionType GetCollisionType(int x, int y)
        {
            int index = x + y * Width;

            if (index < GroundTiles.Length)
            {
                return GroundTiles[index] == null
                    ? CollisionType.None
                    : GroundTiles[index].CollisionShape;
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
        
        public void LoadAlternativeTiles(string alternativeLayerName)
        {
            int index = Array.FindIndex<TiledLayer>(_map.Layers, (TiledLayer l) => l.name == alternativeLayerName);

            if (index == -1)
                throw new Exception("No layer '" + alternativeLayerName + "' found");
            
            Tile[] tileLayer = GroundTiles;

            for (var i = 0; i < _map.Layers[index].data.Length; i++)
            {
                int gid = _map.Layers[index].data[i];

                // Empty tile, do nothing
                if (gid == 0)
                    continue;

                //if (tileLayer[i] != null)
                //    DisposeTile(tileLayer[i]);

                tileLayer[i] = LoadTile(i, gid);
            }
        }

        private Tile[] LoadTiles(TiledCS.TiledMap map, int layerIndex, int[] alternativeIndexes = null)
        {
            var tiles = new Tile[map.Layers[layerIndex].data.Length];

            for (var i = 0; i < map.Layers[layerIndex].data.Length; i++)
            {
                int gid = map.Layers[layerIndex].data[i];

                // Empty tile, do nothing
                if (gid == 0)
                    continue;

                tiles[i] = LoadTile(i, gid);
            }

            return tiles;
        }

        private Tile LoadTile(int tileIndex, int gid)
        {
            Rectangle sourceRectangle = TileFrameToSourceRectangle(gid - 1);

            float x = (tileIndex % Width) * TileWidth;
            float y = (float)Math.Floor(tileIndex / (double)Width) * TileHeight;

            Tile tile = new Tile()
            {
                Position = new Vector2((int)x, (int)y),
                DrawRectangle = new Rectangle((int)x, (int)y, TileWidth, TileHeight),
                SourceRectangle = sourceRectangle
            };

            var tiledTile = _map.GetTiledTile(_map.Tilesets[0], Tileset, gid);

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
                            AnimatedTileIndexes = new List<int>() { tileIndex },
                            Frames = frames
                        };
                        _tileAnimations[gid] = tileAnimation;
                    }
                    else
                    {
                        // Add tile index to previous animation
                        _tileAnimations[gid].AnimatedTileIndexes.Add(tileIndex);
                    }
                }
            }

            return tile;
        }

        private Object[] LoadObjects(TiledCS.TiledMap map, int layerIndex)
        {
            var objects = new Object[map.Layers[layerIndex].objects.Length];

            for (var i = 0; i < map.Layers[layerIndex].objects.Length; i++)
            {
                var item = map.Layers[layerIndex].objects[i];

                if (item.name == "PlayerStart")
                    PlayerStartPosition = new Vector2(item.x, item.y);
                
                objects[i] = new Object()
                {
                    TypeName = item.name,
                    Position = new Vector2(item.x, item.y),
                    TextProperty = GetPropertyValue(item.properties, "TextProperty"),
                    BoolProperty = GetPropertyValue(item.properties, "BoolProperty") == "true"
                };
            }

            return objects;
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
            public bool BoolProperty;
        }

        private class TileAnimation
        {
            public List<int> AnimatedTileIndexes;
            public List<Rectangle> Frames;
            public float FrameDuration;
            private float _elapsedTime = 0;
            private int _currentIndex = 0;

            public void Update(GameTime gameTime, Tile[] tiles)
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