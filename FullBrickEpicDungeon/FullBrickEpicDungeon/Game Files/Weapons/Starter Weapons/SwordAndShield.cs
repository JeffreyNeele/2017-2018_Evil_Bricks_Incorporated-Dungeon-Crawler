using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

/// <summary>
/// Main and starting weapon of the ShieldMaiden
/// </summary>
class SwordAndShield : Weapon
{
    /// <summary>
    /// </summary>
    /// <param name="owner">Defines the owner of the weapon</param>
    public SwordAndShield(Character owner) : base(owner, "Swordandshield", "putassetnamehere")
    {
        AttackDamage = 10;
        GoldWorth = 100;

        //Defines the id for the ability animations
        idBaseAA = "SwordAndShieldAA";
        idMainAbility = "SwordAndShieldMainAbility";
        idSpecialAbility = "SwordAndShieldSpecialAbility";

        //Basic attack of the weapon
        BasicAttack = new BasicAttackAbility(owner, AttackDamage)
        {
            PushBackVector = new Vector2(30, 0),
            PushFallOff = new Vector2(2, 0),
            PushTimeCount = 8
        };

        //Basic ability of the weapon: ShieldBash
        mainAbility = new ShieldBashAbility(owner, AttackDamage * 4, 8);

    }

    //Method that checks collision for the mainAbility with the monsters
    public override void AnimationMainCheck()
    {
        bool hit = false;
        foreach (Monster m in monsterObjectList.Children)
        {
            if (mainAbility.MonsterHit.Count < 1 && m.CollidesWith(Owner))
            {
                mainAbility.AttackHit(m, fieldList);
                hit = true;
            }
        }

        if (hit)
            prevAttackHit = true;
        else
            prevAttackHit = false;
    }
}
