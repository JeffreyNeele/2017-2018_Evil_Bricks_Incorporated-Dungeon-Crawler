using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

class Door : OpenableObject
{
    private bool DoorOpen = false;
    public Door(string assetname, string id, int sheetIndex) : base(TileType.DoorTile, assetname, id, sheetIndex)
    {
    }

    public override void Update(GameTime gameTime)
    {
        DoorLockedChecker();
        if (DoorOpen == true)
            this.sprite.SheetIndex = 1;
    }

    public void DoorLockedChecker()
    {
        GameObjectList objectList = GameWorld.Find("objectLIST") as GameObjectList;
        foreach (var keylock in objectList.Children)
        {
            if (keylock is Lock)
            {
                if (keylock.Visible == false && this.BoundingBox.Intersects(keylock.BoundingBox))
                {
                    DoorOpen = true;
                }
            }
        }
    }

    public bool IsOpen
    {
        get { return IsOpen; }
    }
}

