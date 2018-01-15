using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

class IronArmour : Equipment
{
    public IronArmour() : base("putassetnamehere", "Ironarmour")
    {
        this.MovementSpeedIncrease = 0;
        this.Armour = 30;
        this.GoldWorth = 200;
    }
}
