using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

class Trap : InteractiveObject
{
    public Trap(string assetname, string id, int sheetIndex) : base(assetname, id, sheetIndex)
    {

    }

    protected override void Interact(Character targetCharacter)
    {

    }
}
