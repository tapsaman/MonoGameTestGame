using System.Collections.Generic;

namespace MonoGameTestGame
{
    public class MapA2 : TileMap
    {
        public override TiledCS.TiledMap Load()
        {
            _tileset = new TiledCS.TiledTileset(StaticData.TiledProjectDirectory + "\\linktothepast-tiles.tsx");
            _tilesetTexture = StaticData.TileTexture;

            Exits = new Dictionary<Direction, Exit>()
            {
                { Direction.Up, new Exit(Direction.Up, MapCode.A1, TransitionType.Pan) }
            };

            return new TiledCS.TiledMap(StaticData.TiledProjectDirectory + "\\A2.tmx");
        }
    }
}