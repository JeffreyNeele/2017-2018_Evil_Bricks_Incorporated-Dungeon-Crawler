using System;
using System.Collections.Generic;

// Add more tiletypes here as more get added!
enum TileType
{
    Wall
}

class Tile
{
    TileType type;
    public Tile(TileType type)
    {
        type = type;
    }

    public TileType Type
    {
        get { return this.type; }
    }
}

// Hieronder specifiekere Tiles definiëren bv wall op zo'n manier:
class Wall_Tile
{

}
