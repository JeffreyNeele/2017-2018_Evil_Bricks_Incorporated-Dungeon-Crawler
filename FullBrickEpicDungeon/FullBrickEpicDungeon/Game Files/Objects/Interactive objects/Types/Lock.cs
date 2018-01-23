
class Lock : InteractiveObject
{
    int objectnumber = 0;

    public Lock(string assetname, string id, int sheetIndex) : base(assetname, id, sheetIndex)
    {
    }

    protected override void Interact(Character targetCharacter)
    {
        if (targetCharacter.HasAKey == true && (this.Objectnumber == targetCharacter.PlayerNumber || this.Objectnumber == 5))
        {
            this.visible = false;
        }
    }
    public int Objectnumber
    {
        get { return this.objectnumber; }
        set { objectnumber = value; }
    }
}

