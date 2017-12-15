using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

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

class Basic_Tile : Tile
{
    Basic_Tile(string assetname, int layer = 0, string id = "") : base(TileType.BasicTile, assetname, layer, id)
    {

    }
}