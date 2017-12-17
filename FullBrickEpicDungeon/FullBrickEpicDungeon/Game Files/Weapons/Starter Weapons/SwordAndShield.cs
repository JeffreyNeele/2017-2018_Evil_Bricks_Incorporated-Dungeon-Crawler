using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
class SwordAndShield : Weapon
{
    public SwordAndShield(Character owner) : base(owner, ClassType.ShieldMaiden, "Swordandshield", "putassetnamehere")
    {
        this.AttackDamage = 50;
        this.GoldWorth = 100;
    }

}
