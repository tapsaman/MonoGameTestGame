using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using TiledCS;

namespace MonoGameTestGame
{
    public class MapB1 : TileMap
    {
        public override void Load()
        {
            _map = new TiledMap(StaticData.TiledProjectDirectory + "\\B1.tmx");
            _tileset = new TiledTileset(StaticData.TiledProjectDirectory + "\\linktothepast-tiles.tsx");
            _tilesetTexture = StaticData.TileTexture;
            _nextMaps = new Dictionary<Direction, MapCode>()
            {
                { Direction.Left, MapCode.A1 }
            };
        }
    }
}