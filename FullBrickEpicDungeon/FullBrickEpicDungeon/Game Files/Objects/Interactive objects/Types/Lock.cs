﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class Lock : InteractiveObject
{

    public Lock(string assetname, string id, int sheetIndex) : base(assetname, id, sheetIndex)
    {
    }

    protected override void Interact(Character targetCharacter)
    {
        if (targetCharacter.HasAKey == true)
        {
            this.visible = false;
        }
    }
}
