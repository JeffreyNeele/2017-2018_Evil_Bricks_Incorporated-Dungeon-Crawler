
class KeyItem : InteractiveObject
{
    int objectnumber;

    public KeyItem(string assetname, string id, int sheetIndex) : base(assetname, id, sheetIndex)
    {
    }

    protected override void Interact(Character targetCharacter)
    {
        //pakt sleutel op als nummer gelijk is of als het een ALL key is
        if (this.Objectnumber == targetCharacter.PlayerNumber || this.Objectnumber == 5)
        {
            this.position.X = targetCharacter.Position.X - 12;
            this.position.Y = targetCharacter.Position.Y - 115;
        }


        if (targetCharacter.HasAKey == true)
        {
            GameObjectList objectList = GameWorld.Find("objectLIST") as GameObjectList;
            foreach (var keylock in objectList.Children)
            {
                if (keylock is Lock && ((Lock)keylock).Objectnumber == this.Objectnumber)
                {
                    if (keylock.Visible == false)
                    {
                        this.visible = false;
                    }
                }

            }
        }
    }
    public int Objectnumber
    {
        get { return this.objectnumber; }
        set { objectnumber = value; }
    }
}



