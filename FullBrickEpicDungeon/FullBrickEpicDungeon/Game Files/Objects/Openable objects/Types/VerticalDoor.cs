using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

class VerticalDoor : Door
{
    public VerticalDoor(string assetname, string id, int sheetIndex) : base(assetname, id, sheetIndex)
    {
    }

    public override Rectangle BoundingBox
    {
        get
        {
            int left = (int)upperLeftPosition.X;
            int top = (int)upperLeftPosition.Y;
            if (sprite.SheetIndex == 0)
                return new Rectangle(left, top, 20, 150);
            else
                return new Rectangle(left, top, 100, 85);
        }
    }

    public  Rectangle BoundingBox2
    {
        get
        {
            int left = (int)upperLeftPosition.X;
            int top = (int)upperLeftPosition.Y;
            if (sprite.SheetIndex == 0)
                return new Rectangle(left, top, 100, 50);
            else
                return BoundingBox;
        }
    }
}

