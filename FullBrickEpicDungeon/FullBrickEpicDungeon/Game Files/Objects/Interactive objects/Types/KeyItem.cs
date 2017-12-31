using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class KeyItem : InteractiveObject
{
    public KeyItem (string assetname, string id, int sheetIndex) : base(assetname, id, sheetIndex)
    {

    }

    protected override void Interact(Character targetCharacter)
    {
        this.position.X = targetCharacter.Position.X - 12;
        this.position.Y = targetCharacter.Position.Y -115 ;

    }
}

