
    abstract class OpenableObject: Tile
    {
    //this is an abstract class for for instance doors, trapdoors, chests, etc.

    int objectnumber;
    //TileType type, string assetname, int layer = 0, string id = ""
    protected OpenableObject(TileType type, string assetName, string id, int sheetIndex, int layer = 2) : base(type, assetName, layer, id, sheetIndex)
    {
    }
    public void Open()
    {
        this.ChangeSpriteIndex(1);
    }
    public void Close()
    {
        this.ChangeSpriteIndex(0);
    }

    public int Objectnumber
    {
        get { return this.objectnumber; }
        set { objectnumber = value; }
    }
}

