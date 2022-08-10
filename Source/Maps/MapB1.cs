using System.Collections.Generic;
namespace ZA6
{
    public class MapB1 : TileMap
    {
        public override TiledCS.TiledMap Load()
        {
            _tileset = new TiledCS.TiledTileset(Static.TiledProjectDirectory + "\\linktothepast-tiles.tsx");
            _tilesetTexture = Img.TileTexture;

            Exits = new Dictionary<Direction, Exit>()
            {
                { Direction.Left, new Exit(Direction.Left, MapCode.A1, TransitionType.FadeToBlack) },
                { Direction.Right, new Exit(Direction.Right, MapCode.C1, TransitionType.Pan) },
            };

            return new TiledCS.TiledMap(Static.TiledProjectDirectory + "\\B1.tmx");
        }
    }
}