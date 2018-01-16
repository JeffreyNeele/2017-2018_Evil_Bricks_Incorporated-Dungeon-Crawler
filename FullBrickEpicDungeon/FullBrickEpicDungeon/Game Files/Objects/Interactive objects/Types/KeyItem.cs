
class KeyItem : InteractiveObject
{
    public KeyItem(string assetname, string id, int sheetIndex) : base(assetname, id, sheetIndex)
    {
    }

    protected override void Interact(Character targetCharacter)
    {
        this.position.X = targetCharacter.Position.X - 12;
        this.position.Y = targetCharacter.Position.Y - 115;
        if (targetCharacter.HasAKey == true)
        {
            GameObjectList objectList = GameWorld.Find("objectLIST") as GameObjectList;
            foreach (var keylock in objectList.Children)
            {
                if (keylock is Lock)
                {
                    if (keylock.Visible == false)
                    {
                        this.visible = false;
                    }
                }

            }
        }
    }
}

