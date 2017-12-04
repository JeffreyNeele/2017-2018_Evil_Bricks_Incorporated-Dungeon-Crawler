using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

class ShieldMaiden : Character
{
    public ShieldMaiden(Weapon weapon) : base(weapon, "Shieldmaiden")
    {
        // baseattributes worden gebruikt om de super basic stats permanent te storen in het geheugen zodat ze gereset kunnen worden
        // pas op: attack is de attack ZONDER een wapen vast te hebben (dat je dit weet)
        this.baseattributes.HP = 500;
        this.baseattributes.Armour = 50;
        this.baseattributes.Attack = 10;
    }
}

