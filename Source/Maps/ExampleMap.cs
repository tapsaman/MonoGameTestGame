using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TiledCS;

namespace MonoGameTestGame.Managers
{
    public class ExampleMap : TileMap
    {
        public override void Load()
        {
            // Set the "Copy to Output Directory" property of these two files to `Copy if newer`
            // by clicking them in the solution explorer.
            _map = new TiledMap(StaticData.Content.RootDirectory + "\\Tilemap\\exampleMap.tmx");
            _tileset = new TiledTileset(StaticData.Content.RootDirectory + "\\Tilemap\\exampleTileset.tsx");

            // Not the best way to do this but it works. It looks for "exampleTileset.xnb" file
            // which is the result of building the image file with "Content.mgcb".
            _tilesetTexture = StaticData.Content.Load<Texture2D>("Tilemap/exampleTileset");
        }
    }
}