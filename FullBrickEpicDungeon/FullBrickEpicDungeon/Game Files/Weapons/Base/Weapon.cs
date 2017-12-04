
using System;
using System.Collections.Generic;
abstract class Weapon : AnimatedGameObject
{
    Ability BasicAttack;
    TimedAbility mainAbility;
    SpecialAbility specialAbility;
    BaseAttributes weaponAttributes;
    protected int attack, goldCost;
    protected string animationID;
    protected Weapon(int attack, int goldCost, string id, string assetName, string animationID): base(1, id)
    {
        this.attack = attack;
        this.goldCost = goldCost;
        this.animationID = animationID;
        LoadAnimation(assetName, animationID, false);
    }

    public virtual void Attack()
    {
        BasicAttack.Use();
        PlayAnimation(animationID);
        CollisionChecker(this.CurrentAnimation);
    }

    public virtual void UseMainAbility()
    {
        mainAbility.Use();
        CollisionChecker(this.CurrentAnimation);
    }

    public virtual void UseSpecialAbility()
    {
        specialAbility.Use();
        CollisionChecker(this.CurrentAnimation);
    }

    protected void CollisionChecker(Animation animation)
    {
        while (!(animation.AnimationEnded))
        {
            GameObjectList monsterList = GameWorld.Find("monsterLIST") as GameObjectList;
            foreach (Monster monsterobj in monsterList.Children)
            {
                if (monsterobj.CollidesWith(this))
                {
                    monsterobj.TakeDamage(attack);
                }
            }
        }
    }

    public int AttackDamage
    {
        get { return attack; }
    }
    public int GoldWorth
    {
        get { return goldCost; }
    }

}

