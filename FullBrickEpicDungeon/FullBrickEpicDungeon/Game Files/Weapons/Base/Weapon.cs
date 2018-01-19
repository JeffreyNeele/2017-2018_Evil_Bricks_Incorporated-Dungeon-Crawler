using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

/// <summary>
/// Defines the basic attributes of the weapon
/// </summary>
abstract class Weapon : AnimatedGameObject
{

    // var for the owner of this weapon
    Character owner;
    // vars for abilities that will be called in methods such as MainAbility
    protected BasicAttackAbility BasicAttack;
    protected TimedAbility mainAbility;
    protected SpecialAbility specialAbility;
    private int attack, goldCost;
    protected bool prevAttackHit;
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
        //MainAbility should be updates because of timer. Both should be updated in the case of pushBack (or any other effects)
        BasicAttack.Update(gameTime);
        mainAbility.Update(gameTime);

        //if(!CurrentAnimation.AnimationEnded)
        //    if (idAnimation == idBaseAA)
        //        AnimationAttackCheck();
        //    else if (idAnimation == idMainAbility)
        //        AnimationMainCheck();

        base.Update(gameTime);
    }

    /// <summary>
    /// The base attack method of the weapon. The base AA is an ability of the weapon
    /// </summary>
    /// <param name="monsterList">Give the current list of all the monsters currently in the level</param>
    /// <param name="field">Give the tilefield of the level, important for replacement attacks</param>
    public virtual void Attack(GameObjectList monsterList, GameObjectGrid field)
    {
        monsterObjectList = monsterList;
        fieldList = field;
        BasicAttack.Use();
        idAnimation = idBaseAA;
        //PlayAnimation(idAnimation);
        AnimationAttackCheck();
    }

    /// <summary>
    /// Method that checks the collision between the weapon and the monster
    /// </summary>
    public void AnimationAttackCheck()
    {
        bool hit = false;
        foreach (Monster m in monsterObjectList.Children)
            if (m.CollidesWith(Owner))
            {
                BasicAttack.AttackHit(m, fieldList);
                hit = true;
            }

        if (hit)
            prevAttackHit = true;
        else
            prevAttackHit = false;
    }


    /// <summary>
    /// Uses the main ability of the weapon.
    /// </summary>
    /// <param name="monsterList">Give the current list of all the monsters currently in the level</param>
    /// <param name="field">Give the tilefield of the level, important for replacement attacks</param>
    public virtual void UseMainAbility(GameObjectList monsterList, GameObjectGrid field)
    {
        if (!mainAbility.IsOnCooldown)
        {
            mainAbility.Use();
            idAnimation = idMainAbility;
            monsterObjectList = monsterList;
            fieldList = field;
            //PlayAnimation(idAnimation);
            AnimationMainCheck();
        }
    }


    //Will be made in the weapon class as the main ability may be different for each weapon
    /// <summary>
    /// Method that checks collision for the mainAbility with the monsters
    /// </summary>
    abstract public void AnimationMainCheck();

    /// <summary>
    /// uses the special ability if it is ready
    /// </summary>
    /// <param name="monsterList">Give the current list of all the monsters currently in the level</param>
    public virtual void UseSpecialAbility(GameObjectList monsterList)
    {
        //monsterObjectList = monsterList;
        //specialAbility.Use(this, idSpecialAbility);
        //CollisionChecker(CurrentAnimation, monsterObjectList);
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

