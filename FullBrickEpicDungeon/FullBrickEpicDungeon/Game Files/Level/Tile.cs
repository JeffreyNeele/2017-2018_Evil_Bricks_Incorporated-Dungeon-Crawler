using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

// Add more tiletypes in the Enumerators class!

class Tile : SpriteGameObject
{
    TileType type;
    public Tile(TileType type, string assetname, int layer = 0, string id = "", int sheetindex = 0) : base(assetname, layer, id, sheetindex)
    {
        this.type = type;
    }

    public TileType Type
    {
        get { return this.type; }
    }

    public bool IsSolid
    {
        get { return type == TileType.Brick || type == TileType.RockIce || type == TileType.Water ; }
    }

    public bool IsIce
    {
        get { return type == TileType.Ice; }
    }
}
