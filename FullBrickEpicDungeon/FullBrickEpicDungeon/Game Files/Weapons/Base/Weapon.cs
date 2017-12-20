
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

abstract class Weapon : AnimatedGameObject
{

    protected ClassType classType;
    // var for the owner of this weapon
    Character owner;
    // vars for abilities that will be called in methods such as MainAbility
    protected Ability BasicAttack;
    protected TimedAbility mainAbility;
    protected SpecialAbility specialAbility;
    private int attack, goldCost;
    protected Weapon(Character owner, ClassType classType, string id, string assetName): base(1, id)
    {
        this.owner = owner;
        this.classType = classType;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    // This is the base attack method of the weapon,, which will be also defined as an ability
    public virtual void Attack()
    {
        BasicAttack.Use();
        CollisionChecker(this.CurrentAnimation);
    }

    // Uses the main ability
    public virtual void UseMainAbility()
    {
        mainAbility.Use();
        CollisionChecker(this.CurrentAnimation);
    }

    // uses the special ability if it is ready
    public virtual void UseSpecialAbility()
    {
        specialAbility.Use();
        CollisionChecker(this.CurrentAnimation);
    }

    // Checks for collision with mosnters
    protected void CollisionChecker(Animation animation)
    {
        while (!(animation.AnimationEnded))
        {
            GameObjectList monsterList = GameWorld.Find("monsterLIST") as GameObjectList;
            foreach (Monster monsterobj in monsterList.Children)
            {
                if (monsterobj.CollidesWith(this))
                {
                    monsterobj.TakeDamage(owner.Attributes.Attack + this.AttackDamage);
                    if (monsterobj.IsDead)
                    {
                        owner.Attributes.Gold += monsterobj.Attributes.Gold;
                    }
                }
            }
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

    public Character Owner
    {
        get { return owner; }
    }

    public ClassType Type
    {
        get { return classType; }
    }
}

