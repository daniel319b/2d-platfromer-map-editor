using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Paths
{
    public enum TileCollision
    {
        Passable = 0,
        Impassable = 1,
        Platform = 2,
    }


    struct Tile
    {
   
        public static int Width = 32;
        public static int Height = 32;

        public static Vector2 Size = new Vector2(Width, Height);

    }
}
