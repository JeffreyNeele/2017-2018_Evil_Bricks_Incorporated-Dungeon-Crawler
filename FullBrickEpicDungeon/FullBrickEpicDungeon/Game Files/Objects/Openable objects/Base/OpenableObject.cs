
abstract class OpenableObject: SpriteGameObject
{
    //this is an abstract class for for instance doors, trapdoors, chests, etc.

    int objectnumber;
    protected OpenableObject(string assetName, string id, int sheetIndex, int layer = 2) : base(assetName, layer, id, sheetIndex)
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

