using System.Collections.Generic;
namespace MonoGameTestGame
{
    public class MapB1 : TileMap
    {
        public override TiledCS.TiledMap Load()
        {
            _tileset = new TiledCS.TiledTileset(StaticData.TiledProjectDirectory + "\\linktothepast-tiles.tsx");
            _tilesetTexture = StaticData.TileTexture;

            Exits = new Dictionary<Direction, Exit>()
            {
                { Direction.Left, new Exit(Direction.Left, MapCode.A1, TransitionType.FadeToBlack) }
            };

            return new TiledCS.TiledMap(StaticData.TiledProjectDirectory + "\\B1.tmx");
        }
    }
}