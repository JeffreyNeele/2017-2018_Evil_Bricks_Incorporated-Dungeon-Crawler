
class Lock : InteractiveObject
{
    int objectnumber = 0;

    public Lock(string assetname, string id, int sheetIndex) : base(assetname, id, sheetIndex)
    {
    }

    protected override void Interact(Character targetCharacter)
    {
        //if (targetCharacter.carriedKey != null && (this.Objectnumber == targetCharacter.PlayerNumber || (targetCharacter.carriedKey.Objectnumber == 5 && this.objectnumber == 5)))
        //{
        //    this.visible = false;
        //}
    }
    public int Objectnumber
    {
        get { return this.objectnumber; }
        set { objectnumber = value; }
    }
}

