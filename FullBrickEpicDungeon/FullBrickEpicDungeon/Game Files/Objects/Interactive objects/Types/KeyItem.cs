
class KeyItem : InteractiveObject
{
    int objectnumber;
    bool used;
    bool taken;
    public KeyItem(string assetname, Level currentlevel, string id, int sheetIndex) : base(assetname, currentlevel, id, sheetIndex)
    {
        used = false;
        taken = false;
    }

    protected override void Interact(Character targetCharacter)
    {
        //pakt sleutel op als nummer gelijk is of als het een ALL key is
        if (this.Objectnumber == targetCharacter.PlayerNumber || this.Objectnumber == 5)
        {
            this.position.X = targetCharacter.Position.X - 12;
            this.position.Y = targetCharacter.Position.Y - 115;
            taken = true;
        }
    }

    public void useKey()
    {
        GameObjectList objectList = currentlevel.GameWorld.Find("objectLIST") as GameObjectList;
        foreach (var keylock in objectList.Children)
        {
            if (used)
                continue;

            if(keylock is Lock)
                if(((Lock)keylock).Objectnumber == this.Objectnumber && keylock.BoundingBox.Intersects(TargetCharacter.BoundingBox))
                {
                    keylock.Visible = false;
                    this.visible = false;
                    used = true;

                }

        }

        if (used)
        {
            TargetCharacter.carriedKey = null;
            objectList.Remove(this);
        }
    }

    public int Objectnumber
    {
        get { return this.objectnumber; }
        set { objectnumber = value; }
    }

    public bool keyOwned
    {
        get { return taken; }
    }
}



