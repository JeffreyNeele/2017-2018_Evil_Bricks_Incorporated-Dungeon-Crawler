using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

class Door : OpenableObject
{
    public bool DoorOpen = false;
    public Door(TileType door, string assetname, string id, int sheetIndex) : base(door, assetname, id, sheetIndex)
    {
    }

    public override void Update(GameTime gameTime)
    {
        DoorLockedChecker();
        if (DoorOpen == true)
            this.ChangeSpriteIndex(1);
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

}

