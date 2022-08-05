using System.Collections.Generic;

namespace MonoGameTestGame
{
    public class TestMap : TileMap
    {
        public override TiledCS.TiledMap Load()
        {
            _tileset = new TiledCS.TiledTileset(Static.TiledProjectDirectory + "\\linktothepast-tiles.tsx");
            _tilesetTexture = Img.TileTexture;

            Exits = new Dictionary<Direction, Exit>()
            {
                { Direction.Right, new Exit(Direction.Right, MapCode.B1, TransitionType.FadeToBlack) },
                { Direction.Down, new Exit(Direction.Down, MapCode.A2, TransitionType.Pan) }
            };

            return  new TiledCS.TiledMap(Static.TiledProjectDirectory + "\\A1.tmx");
        }
    }
}