
    abstract class OpenableObject: Tile
    {
    //this is an abstract class for for instance doors, trapdoors, chests, etc.

    protected Level currentlevel;
    int objectnumber;
    //TileType type, string assetname, int layer = 0, string id = ""
    protected OpenableObject(TileType type, string assetName, Level currentlevel, string id, int sheetIndex) : base(type, assetName, 2, id, sheetIndex)
    {
        this.currentlevel = currentlevel;
    }
    public void Open()
    {
        this.sprite.SheetIndex = 1;
    }
    public void Close()
    {
        this.sprite.SheetIndex = 0;
    }

    public int Objectnumber
    {
        get { return this.objectnumber; }
        set { objectnumber = value; }
    }
}

