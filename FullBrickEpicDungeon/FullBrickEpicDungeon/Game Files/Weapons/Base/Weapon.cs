
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

abstract class Weapon : AnimatedGameObject
{

    // var for the owner of this weapon
    Character owner;
    // vars for abilities that will be called in methods such as MainAbility
    protected BasicAttackAbility BasicAttack;
    protected TimedAbility mainAbility;
    protected SpecialAbility specialAbility;
    private int attack, goldCost;
    private bool prevAttackHit;
    protected string idBaseAA, idMainAbility, idSpecialAbility, idAnimation;
    protected GameObjectList monsterObjectList;
    protected GameObjectGrid fieldList;
    /// <summary>
    /// Creates a weapon for a character
    /// </summary>
    /// <param name="owner">The owner of the weapon</param>
    /// <param name="id">the id of the weapon object in the gameworld</param>
    /// <param name="assetName">Give the path where to find the asset in the content</param>
    protected Weapon(Character owner, string id, string assetName): base(1, id)
    {
        this.owner = owner;
    }

    public override void Update(GameTime gameTime)
    {
        /*if(!this.CurrentAnimation.AnimationEnded)
            CollisionChecker(this.CurrentAnimation, monsterObjectList);*/
        
        base.Update(gameTime);
    }

    // This is the base attack method of the weapon,, which will be also defined as an ability
    public virtual void Attack(GameObjectList monsterList, GameObjectGrid field)
    {
        
    }



    // Uses the main ability
    public virtual void UseMainAbility(GameObjectList monsterList, GameObjectGrid field)
    {
        monsterObjectList = monsterList;
        mainAbility.Use(this, idMainAbility);
        CollisionChecker(this.CurrentAnimation, monsterObjectList);
    }

    // uses the special ability if it is ready
    public virtual void UseSpecialAbility(GameObjectList monsterList)
    {
        monsterObjectList = monsterList;
        specialAbility.Use(this, this.idSpecialAbility);
        CollisionChecker(this.CurrentAnimation, monsterObjectList);
    }

    // Checks for collision with monsters
    protected void CollisionChecker(Animation animation, GameObjectList monsterList)
    {
        bool hit = false;
        foreach (Monster monsterobj in monsterList.Children)
        {
            if (monsterobj.CollidesWith(this))
            {
                monsterobj.TakeDamage(owner.Attributes.Attack + this.AttackDamage);
                hit = true;
                if (monsterobj.IsDead)
                {
                    owner.Attributes.Gold += monsterobj.Attributes.Gold;
                }
            }
        }

        if (hit)
        {
            prevAttackHit = true;
        }
        else
        {
            prevAttackHit = false;
        }

    }

    //Method that checks the collision between the weapon and the monster
    public void AnimationAttackCheck()
    {
        bool hit = false;
        foreach (Monster m in monsterObjectList.Children)
        {
            if (m.CollidesWith(Owner))
            {
                BasicAttack.AttackHit(m, fieldList);
                hit = true;
            }
        }

        if (hit)
        {
            prevAttackHit = true;
        }
        else
        {
            prevAttackHit = false;
        }
    }

    // attributes for attackdamage and how much gold this weapon is worth 
    public int AttackDamage
    {
        get { return attack; }
        protected set { attack = value; }
    }
    public int GoldWorth
    {
        get { return goldCost; }
        protected set { goldCost = value; }
    }
    public bool PreviousAttackHit
    {
        get { return prevAttackHit; }
    }
    public Character Owner
    {
        get { return owner; }
    }

    public BasicAttackAbility BaseAA
    {
        get { return BasicAttack; }
    }

    public TimedAbility AbilityMain
    {
        get { return mainAbility; }
    }

}

