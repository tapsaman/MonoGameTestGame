using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using TiledCS;

namespace MonoGameTestGame
{
    public class MapB1 : TileMap
    {
        public override void Load()
        {
            _map = new TiledMap(StaticData.Content.RootDirectory + "\\TiledProjects\\testmap\\B1.tmx");
            _tileset = new TiledTileset(StaticData.Content.RootDirectory + "\\TiledProjects\\testmap\\testmap.tsx");
            _tilesetTexture = StaticData.Content.Load<Texture2D>("linktothepast-tiles");
            _nextMaps = new Dictionary<Direction, MapCode>()
            {
                { Direction.Left, MapCode.A1 }
            };
        }
    }
}