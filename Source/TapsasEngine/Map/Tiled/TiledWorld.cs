using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework.Graphics;

namespace ZA6
{
    public class TiledWorld
    {
        public WorldMap[] Maps;
        public string Directory;

        public TiledWorld(string directory, string worldFileName)
        {
            Directory = directory + "\\";
            Maps = Loader.LoadMaps(Directory + worldFileName);
        }

        public TileMap LoadTileMap(string mapName)
        {
            WorldMap worldMap = FindWorldMap(mapName);

            TileMap map = new TileMap();
            map.Name = mapName;
            
            var tiledMap = new TiledCS.TiledMap(Directory + worldMap.FileName);
            map.Tileset = new TiledCS.TiledTileset(Directory + tiledMap.Tilesets[0].source);
            map.TilesetTexture = Img.TilesetImageToTexture(map.Tileset.Image);
            map.Load(tiledMap);
            map.Exits = GenerateExits(worldMap);

            return map;
        }

        private WorldMap FindWorldMap(string mapName)
        {
            for (int i = 0; i < Maps.Length; i++)
            {
                if (Maps[i].Name == mapName)
                {
                    return Maps[i];
                }
            }

            throw new System.Exception("Map name " + mapName + " not defined in tiled world");
        }

        private Dictionary<Direction, MapExit> GenerateExits(WorldMap worldMap)
        {
            var exists = new Dictionary<Direction, MapExit>();

            for (int i = 0; i < Maps.Length; i++)
            {
                var map = Maps[i];

                if (worldMap.LeftCollidesWith(map))
                {
                    exists[Direction.Left] = new MapExit()
                    {
                        Direction = Direction.Left,
                        MapName = map.Name
                    };
                    break;
                }
            }
            for (int i = 0; i < Maps.Length; i++)
            {
                var map = Maps[i];

                if (worldMap.RightCollidesWith(map))
                {
                    exists[Direction.Right] = new MapExit()
                    {
                        Direction = Direction.Right,
                        MapName = map.Name
                    };
                    break;
                }
            }
            for (int i = 0; i < Maps.Length; i++)
            {
                var map = Maps[i];

                if (worldMap.TopCollidesWith(map))
                {
                    exists[Direction.Up] = new MapExit()
                    {
                        Direction = Direction.Up,
                        MapName = map.Name
                    };
                    break;
                }
            }
            for (int i = 0; i < Maps.Length; i++)
            {
                var map = Maps[i];

                if (worldMap.BottomCollidesWith(map))
                {
                    exists[Direction.Down] = new MapExit()
                    {
                        Direction = Direction.Down,
                        MapName = map.Name
                    };
                    break;
                }
            }

            return exists;
        }

        public class WorldMap
        {
            public string Name; 
            public string FileName;
            public int X;
            public int Y;
            public int Width;
            public int Height;

            public bool TopCollidesWith(WorldMap map)
            {
                return Y == map.Y + map.Height
                    && ((X >= map.X && X + Width <= map.X + map.Width)
                    || (map.X >= X && map.X + map.Width <= X + Width));
            }

            public bool BottomCollidesWith(WorldMap map)
            {
                return Y + Height == map.Y
                    && ((X >= map.X && X + Width <= map.X + map.Width)
                    || (map.X >= X && map.X + map.Width <= X + Width));
            }

            public bool RightCollidesWith(WorldMap map)
            {
                return X + Width == map.X
                    && ((Y >= map.Y && Y + Height <= map.Y + map.Height)
                    || (map.Y >= Y && map.Y + map.Height <= Y + Height));
            }

            public bool LeftCollidesWith(WorldMap map)
            {
                return X  == map.X + map.Width
                    && ((Y >= map.Y && Y + Height <= map.Y + map.Height)
                    || (map.Y >= Y && map.Y + map.Height <= Y + Height));
            }
        }

        public static class Loader
        {
            public static WorldMap[] LoadMaps(string worldJsonPath)
            {
                StreamReader reader = new StreamReader(worldJsonPath);
                string json = reader.ReadToEnd();

                WorldJson worldData = JsonSerializer.Deserialize<WorldJson>(json);
                WorldMap[] maps = new WorldMap[worldData.maps.Count];

                for (int i = 0; i < worldData.maps.Count; i++)
                {
                    var mapData = worldData.maps[i];

                    maps[i] = new WorldMap()
                    {
                        X = mapData.x,
                        Y = mapData.y,
                        Width = mapData.width,
                        Height = mapData.height,
                        FileName = mapData.fileName,
                        Name = Regex.Replace(mapData.fileName, @"\.tmx$", "")
                    };
                }

                Array.Sort(maps, new WorldMapComparer());

                return maps;
            }

            public class WorldJson
            {
                public List<MapJson> maps { get; set; }
            }

            public class MapJson
            {
                public string fileName { get; set; }
                public int height { get; set; }
                public int width { get; set; }
                public int x { get; set; }
                public int y { get; set; }
            }

            private class WorldMapComparer : IComparer
            {
                public int Compare(object x, object y)
                {
                    return (new CaseInsensitiveComparer()).Compare(((WorldMap)x).Name, ((WorldMap)y).Name);
                }
            }
        }
    }
}