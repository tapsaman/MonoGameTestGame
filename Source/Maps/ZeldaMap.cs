using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TiledCS;

namespace MonoGameTestGame.Managers
{
    public class ZeldaMap : TileMap
    {
        public override void Load()
        {
            _map = new TiledMap(StaticData.Content.RootDirectory + "\\TiledProject\\linktothepast\\map1.tmx");
            _tileset = new TiledTileset(StaticData.Content.RootDirectory + "\\TiledProject\\linktothepast\\linktothepast.tsx");
            _tilesetTexture = StaticData.Content.Load<Texture2D>("linktothepast-tiles");
        }
    }
}