using Microsoft.Xna.Framework;

/// <summary>
/// Class for a door
/// </summary>
class Door : OpenableObject
{
    private bool DoorOpen = false;
    protected Vector2 positionUpperLeft;

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

        foreach (var teacup in objectList.Children)
        {
            if (teacup is Teacup && ((Teacup)teacup).Objectnumber == this.Objectnumber)
            {
                if (((Teacup)teacup).TeacupSet == true)
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

    //Bounding Box voor de collisie. Als de deur open is, moet er geen collisie zijn.
    public override Rectangle BoundingBox
    {
        get
        {
            int left = (int)upperLeftPosition.X;
            int top = (int)upperLeftPosition.Y;
            if (sprite.SheetIndex == 0)
                return new Rectangle(left, top, 100, 100);
            else
                return new Rectangle(left, top, 0, 0);
        }
    }

    public Vector2 upperLeftPosition
    {
        get { return positionUpperLeft; }
        set { positionUpperLeft = value; }
    }
}

