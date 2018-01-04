using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
class SwordAndShield : Weapon
{
    //ShieldBashAbility abilityMain;
    public SwordAndShield(Character owner) : base(owner, ClassType.ShieldMaiden, "Swordandshield", "putassetnamehere")
    {
        this.AttackDamage = 50;
        this.GoldWorth = 100;

        this.idBaseAA = "SwordAndShieldAA";
        this.idMainAbility = "SwordAndShieldMainAbility";
        this.idSpecialAbility = "SwordAndShieldSpecialAbility";

        //Basic attack of the weapon
        BasicAttack = new BasicAttackAbility(owner, ClassType.ShieldMaiden, this, "assetName", idBaseAA, 10, true);
        BasicAttack.pushBackVector = new Vector2(20, 0);

        //Basic ability of the weapon: ShieldBash
        mainAbility = new ShieldBashAbility(owner, ClassType.ShieldMaiden, this, "assetName", idMainAbility, 8);

    }

    //Method for the basic attack
    public override void Attack(GameObjectList monsterList)
    {
        BasicAttack.Use(this, idBaseAA);
        //base.Attack(monsterList);
        idAnimation = idBaseAA;
        monsterObjectList = monsterList;
        AnimationAttackCheck();
    }

    //Method that checks the collision between the weapon and the monster
    public void AnimationAttackCheck()
    {
        foreach (Monster m in monsterObjectList.Children)
            if (m.CollidesWith(Owner))
                BasicAttack.attackHit(m);
    }

    //Method for the use of the main ability
    public override void UseMainAbility(GameObjectList monsterList)
    {
        if(!mainAbility.isOnCooldown)
        {
            this.mainAbility.Use(this, idMainAbility);
            this.idAnimation = idMainAbility;
            //base.UseMainAbility(monsterList);
            monsterObjectList = monsterList;
            AnimationMainCheck();
        }
    }

    //Method that checks collision for the mainAbility with the monsters
    public void AnimationMainCheck()
    {
        foreach (Monster m in monsterObjectList.Children)
        {
            if (mainAbility.monsterHit.Count < 1 && m.CollidesWith(Owner))
                mainAbility.attackHit(m);
        }
    }

    public override void Update(GameTime gameTime)
    {
        //Because the mainAbility has a timer, the update of the mainAbility should be called
        mainAbility.Update(gameTime);

        base.Update(gameTime);
    }

}
