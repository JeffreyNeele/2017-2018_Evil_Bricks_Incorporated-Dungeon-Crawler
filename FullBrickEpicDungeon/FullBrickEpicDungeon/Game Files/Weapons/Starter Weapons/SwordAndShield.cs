using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
class SwordAndShield : Weapon
{
    /// <summary>
    /// Main weapon for the ShieldMaiden
    /// </summary>
    /// <param name="owner"></param>
    public SwordAndShield(Character owner) : base(owner, "Swordandshield", "putassetnamehere")
    {
        AttackDamage = 10;
        GoldWorth = 100;

        idBaseAA = "SwordAndShieldAA";
        idMainAbility = "SwordAndShieldMainAbility";
        idSpecialAbility = "SwordAndShieldSpecialAbility";

        //Basic attack of the weapon
        BasicAttack = new BasicAttackAbility(owner, this, "assetName", idBaseAA, AttackDamage)
        {
            PushBackVector = new Vector2(30, 0),
            PushFallOff = new Vector2(2, 0),
            PushTimeCount = 8
        };

        //Basic ability of the weapon: ShieldBash
        mainAbility = new ShieldBashAbility(owner, this, "assetName", idMainAbility, 8);

    }

    //Method for the basic attack
    public override void Attack(GameObjectList monsterList, GameObjectGrid field)
    {
        base.Attack(monsterList, field);
        monsterObjectList = monsterList;
        fieldList = field;
        AnimationAttackCheck();
        BasicAttack.Use(this, idBaseAA);
        idAnimation = idBaseAA;
    }


    //Method for the use of the main ability
    public override void UseMainAbility(GameObjectList monsterList, GameObjectGrid field)
    {
        if(!mainAbility.IsOnCooldown)
        {
            this.mainAbility.Use(this, idMainAbility);
            this.idAnimation = idMainAbility;
            //base.UseMainAbility(monsterList);
            monsterObjectList = monsterList;
            fieldList = field;
            AnimationMainCheck();
        }
    }

    //Method that checks collision for the mainAbility with the monsters
    public void AnimationMainCheck()
    {
        foreach (Monster m in monsterObjectList.Children)
        {
            if (mainAbility.MonsterHit.Count < 1 && m.CollidesWith(Owner))
                mainAbility.AttackHit(m, fieldList);
        }
    }

    public override void Update(GameTime gameTime)
    {
        //MainAbility should be updates because of timer. Both should be updates in the case of pushBack (or any other effects)
        BasicAttack.Update(gameTime);
        mainAbility.Update(gameTime);

        base.Update(gameTime);
    }

}
