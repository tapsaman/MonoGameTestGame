using System.Collections.Generic;
namespace MonoGameTestGame
{
    public class MapC1 : TileMap
    {
        public override TiledCS.TiledMap Load()
        {
            _tileset = new TiledCS.TiledTileset(StaticData.TiledProjectDirectory + "\\linktothepast-tiles.tsx");
            _tilesetTexture = StaticData.TileTexture;

            Exits = new Dictionary<Direction, Exit>()
            {
                { Direction.Left, new Exit(Direction.Left, MapCode.B1, TransitionType.Pan) }
            };

            return new TiledCS.TiledMap(StaticData.TiledProjectDirectory + "\\C1.tmx");
        }
    }
}