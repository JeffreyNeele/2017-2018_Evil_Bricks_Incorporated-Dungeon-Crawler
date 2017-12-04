using System;
using System.Collections.Generic;

// Add more tiletypes in the Enumerators class!

class Tile : SpriteGameObject
{
    TileType type;
    public Tile(TileType type, string assetname, int layer = 0, string id = "") : base(assetname, layer, id)
    {
        this.type = type;
    }

    public TileType Type
    {
        get { return this.type; }
    }
}

// Hieronder specifiekere Tiles definiëren bv wall op zo'n manier:
// class Wall_Tile : Tile
