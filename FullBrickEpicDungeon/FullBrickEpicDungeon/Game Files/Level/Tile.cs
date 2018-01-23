/// <summary>
/// Enum for tile types
/// </summary>
enum TileType
{
    BasicTile,
    Brick,
    Ice,
    RockIce,
    Water,
    CobbleStone,
    Wood,
    Grass,
    Trapdoor,
    DoorTile
}

class Tile : SpriteGameObject
{
    TileType type;
    /// <summary>
    /// Class that defines a simple Tile
    /// </summary>
    /// <param name="type">The tiletype this Tile should be classified as</param>
    /// <param name="assetname">path to the sprite of the asset</param>
    /// <param name="layer">Layer this tile is on</param>
    /// <param name="id">ID of the tile</param>
    /// <param name="sheetindex">sheetindex of the tile</param>
    public Tile(TileType type, string assetname, int layer = 0, string id = "", int sheetindex = 0) : base(assetname, layer, id, sheetindex)
    {
        this.type = type;
    }

    public TileType Type
    {
        get { return this.type; }
    }

    // Certain tile types are marked as solid, and this is returned as true if the tile is one of these TileTypes
    public bool IsSolid
    {
        get {
            if (type == TileType.Brick || type == TileType.RockIce || type == TileType.Water)
                return true;
            // Doors are special as in that they can be open (and passable for AI and players) OR they can be closed (and not passable) as such we check here if the door is passable
            else if (type == TileType.DoorTile)
            {
                return DoorTileChecker();
            }
            else
                return false;
            }
               
    }

    private bool DoorTileChecker()
    {
        if (this is VerticalDoor)
        {
            if ((this as VerticalDoor).sprite.SheetIndex == 0)
                return true;
            else if ((this as VerticalDoor).sprite.SheetIndex == 1 && this.BoundingBox.Intersects((this as VerticalDoor).BoundingBox))
                return true;
            else
                return false;
        }
        else if ((this as Door).sprite.SheetIndex == 0)
        {
            return true;
        }
        else
            return false;
    }
}
