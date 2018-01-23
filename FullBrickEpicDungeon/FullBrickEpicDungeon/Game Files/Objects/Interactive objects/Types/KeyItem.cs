
class KeyItem : InteractiveObject
{
    int objectnumber;
    bool used;
    public KeyItem(string assetname, string id, int sheetIndex) : base(assetname, id, sheetIndex)
    {
        used = false;
    }

    protected override void Interact(Character targetCharacter)
    {
        //pakt sleutel op als nummer gelijk is of als het een ALL key is
        if (this.Objectnumber == targetCharacter.PlayerNumber || this.Objectnumber == 5)
        {
            this.position.X = targetCharacter.Position.X - 12;
            this.position.Y = targetCharacter.Position.Y - 115;
        }
    }

    public void useKey()
    {
        GameObjectList objectList = GameWorld.Find("objectLIST") as GameObjectList;
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
}



