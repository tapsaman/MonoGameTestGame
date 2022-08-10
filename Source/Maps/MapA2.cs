using System.Collections.Generic;

namespace ZA6
{
    public class MapA2 : TileMap
    {
        public override TiledCS.TiledMap Load()
        {
            _tileset = new TiledCS.TiledTileset(Static.TiledProjectDirectory + "\\linktothepast-tiles.tsx");
            _tilesetTexture = Img.TileTexture;

            Exits = new Dictionary<Direction, Exit>()
            {
                { Direction.Up, new Exit(Direction.Up, MapCode.A1, TransitionType.Pan) }
            };

            return new TiledCS.TiledMap(Static.TiledProjectDirectory + "\\A2.tmx");
        }
    }
}