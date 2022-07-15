using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TiledCS;

namespace MonoGameTestGame
{
    public abstract class TileMap
    {
        protected TiledMap _map;
        protected TiledTileset _tileset;
        protected Texture2D _tilesetTexture;
        private int _tileWidth;
        private int _tileHeight;
        private int _tilesetTilesWide;
        private int _tilesetTilesHeight;

        /*
        Should load fields _map, _tileset, _tilesetTexture
        */
        public abstract void Load();

        public TileMap()
        {
            Load();

            _tileWidth = _tileset.TileWidth;
            _tileHeight = _tileset.TileHeight;

            // Amount of tiles on each row (left right)
            _tilesetTilesWide = _tileset.Columns;
            // Amount of tiels on each column (up down)
            _tilesetTilesHeight = _tileset.TileCount / _tileset.Columns;

            // Print "Sun" to the debug console. This is an object in "Object Layer 1".
            //System.Diagnostics.Debug.WriteLine(_map.Layers[1].objects[0].name);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (var i = 0; i < _map.Layers[0].data.Length; i++)
            {
                int gid = _map.Layers[0].data[i];

                // Empty tile, do nothing
                if (gid == 0)
                {

                }
                else
                {
                    // Tileset tile ID
                    // Looking at the exampleTileset.png
                    // 0 = Blue
                    // 1 = Green
                    // 2 = Dark Yellow
                    // 3 = Magenta
                    int tileFrame = gid - 1;

                    // Print the tile type into the debug console.
                    // This assumes only one (1) `tiled tileset` is being used, so getting the first one.
                    var tile = _map.GetTiledTile(_map.Tilesets[0], _tileset, gid);
                    if (tile != null) {
                        // This should print "Grass" for each grass tile in the map each draw call
                        // so six (6) times.
                        //System.Diagnostics.Debug.WriteLine(tile.type);
                    }
                 
                    int column = tileFrame % _tilesetTilesWide;
                    int row = (int)Math.Floor((double)tileFrame / (double)_tilesetTilesWide);

                    float x = (i % _map.Width) * _map.TileWidth;
                    float y = (float)Math.Floor(i / (double)_map.Width) * _map.TileHeight;

                    Rectangle tilesetRec = new Rectangle(_tileWidth * column, _tileHeight * row, _tileWidth, _tileHeight);

                    spriteBatch.Draw(_tilesetTexture, new Rectangle((int)x, (int)y, _tileWidth, _tileHeight), tilesetRec, Color.White);
                }
            }
        }

        public bool CheckCollision(Vector2 position)
        {
            int x = (int)Math.Floor(position.X / _tileWidth);
            int y = (int)Math.Floor(position.Y / _tileHeight);
            if (x < 0 || x >= _map.Width || y < 0 || y >= _map.Height)
                return false;
            int index = x + y * _map.Width;
            int gid = _map.Layers[0].data[index];
            var tile = _map.GetTiledTile(_map.Tilesets[0], _tileset, gid);
            //Console.WriteLine(x + "," + y + "=" + index + "::" + gid + "::" + (tile != null ? tile.ToString() : "null"));
            if (tile != null && tile.properties.Length != 0 && tile.properties[0].name == "IsBlocking")
            {
                Console.WriteLine(tile.ToString());
                return true;
            }
            return false; //gid == 1;
        }

        public bool CheckCollision(int x, int y)
        {
            if (x < 0 || x >= _map.Width || y < 0 || y >= _map.Height)
                return false;
            int index = x + y * _map.Width;
            int gid = _map.Layers[0].data[index];
            var tile = _map.GetTiledTile(_map.Tilesets[0], _tileset, gid);
            if (tile != null && tile.properties.Length != 0 && tile.properties[0].name == "IsBlocking")
            {
                //Console.WriteLine(tile.ToString());
                return true;
            }
            return false;
        }

        public bool CheckHorizontalCollision(int x, int topY, int bottomY)
        {
            //Console.WriteLine("CheckHorizontalCollision");
            for (int i = topY; i < bottomY + 1; i++)
            {
                if (CheckCollision(x, i)) {
                    return true;
                }
            }
            return false;
        }

        public bool CheckVerticalCollision(int y, int leftX, int rightX)
        {
            //Console.WriteLine("CheckVerticalCollision");
            for (int i = leftX; i < rightX + 1; i++)
            {
                if (CheckCollision(i, y)) {
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
    }
}