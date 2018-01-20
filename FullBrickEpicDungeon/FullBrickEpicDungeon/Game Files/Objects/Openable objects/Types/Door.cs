using Microsoft.Xna.Framework;

/// <summary>
/// Class for a door
/// </summary>
class Door : OpenableObject
{
    private bool DoorOpen = false;


    /// <param name="assetname">Path to be able to load in the sprite</param>
    /// <param name="id">defined id to be able to find the door</param>
    /// <param name="sheetIndex">Defines which picture of the animation will be shown</param>
    public Door(string assetname, string id, int sheetIndex) : base(TileType.DoorTile, assetname, id, sheetIndex)
    {
    }

    //If a door has no locks, update the door to the open picture
    public override void Update(GameTime gameTime)
    {
        DoorLockedChecker();
        if (DoorOpen == true)
            this.sprite.SheetIndex = 1;
    }

    //Check if the door still has any locks on it
    public void DoorLockedChecker()
    {
        GameObjectList objectList = GameWorld.Find("objectLIST") as GameObjectList;
        foreach (var keylock in objectList.Children)
        {
            if (keylock is Lock)
            {
                //if (keylock.Visible == false && this.Objectnumber == ((Lock)keylock).Objectnumber)
                if(keylock.Visible == false && keylock.BoundingBox.Intersects(this.BoundingBox))
                {
                    DoorOpen = true;
                    openUnlockedDoor(this);
                }
            }
        }
    }

    public void openUnlockedDoor(Door openDoor)
    {
        GameObjectGrid tileField = GameWorld.Find("TileField") as GameObjectGrid;
        foreach (var door in tileField.Objects)
            if(door is Door)
                if (((Door)door).Objectnumber == openDoor.Objectnumber && openDoor.DoorOpen)
                    ((Door)door).DoorOpen = true;
    }
}

