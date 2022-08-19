using System;
using Microsoft.Xna.Framework;
using TapsasEngine.Enums;

namespace TapsasEngine.Maps
{
    public abstract class Map
    {
        // Height in tiles
        public int Width { get; protected set; }
        // Width in tiles
        public int Height { get; protected set; }
    }
}