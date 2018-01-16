
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
    // integers for attack and goldcost
    private int attack, goldCost;
    // bool that returns if the player actually hit something after pressing the key
    protected bool prevAttackHit;
    // IDs for the abilities
    protected string idBaseAA, idMainAbility, idSpecialAbility, idAnimation;
    protected GameObjectList monsterObjectList;
    protected GameObjectGrid fieldList;
    protected string AttackDirection;
    protected bool isAttacking;
    Timer attackAnimationTimer;
    /// <summary>
    /// Creates a weapon for a character
    /// </summary>
    /// <param name="owner">The owner of the weapon</param>
    /// <param name="id">the id of the weapon object in the gameworld</param>
    /// <param name="assetName">Give the path where to find the asset in the content</param>
    protected Weapon(Character owner, string id, string assetName): base(1, id)
    {
        attackAnimationTimer = new Timer(0.66F)
        {
            IsExpired = true
        };

        this.owner = owner;
    }

    /// <summary>
    /// Updates the Shield and sword weapon
    /// </summary>
    /// <param name="gameTime">current gameTime</param>
    public override void Update(GameTime gameTime)
    {
        attackAnimationTimer.Update(gameTime);
        if (attackAnimationTimer.IsExpired)
        {
            isAttacking = false;
            attackAnimationTimer.Reset();
        }

        if (!this.CurrentAnimation.AnimationEnded)
            if (idAnimation == idBaseAA)
                AnimationAttackCheck();
            else if (idAnimation == idMainAbility)
                AnimationMainCheck();

        base.Update(gameTime);
    }

    // This is the base attack method of the weapon,, which will be also defined as an ability
    public virtual void Attack(GameObjectList monsterList, GameObjectGrid field)
    {
        monsterObjectList = monsterList;
        fieldList = field;
        BasicAttack.Use();
        idAnimation = idBaseAA;
        AnimationAttackCheck();
    }



    // Uses the main ability
    public virtual void UseMainAbility(GameObjectList monsterList, GameObjectGrid field)
    {
        if (!mainAbility.IsOnCooldown)
        {
            mainAbility.Use();
            idAnimation = idMainAbility;
            monsterObjectList = monsterList;
            fieldList = field;
            AnimationMainCheck();
        }
    }

    // uses the special ability if it is ready
    public virtual void UseSpecialAbility(GameObjectList monsterList)
    {
        //monsterObjectList = monsterList;
        //specialAbility.Use();
        //CollisionChecker(this.CurrentAnimation, monsterObjectList);
    }

    /// <summary>
    /// Checks if the owner currently collides with a monster, and if it does make the monster take damage
    /// </summary>
    /// <param name="animation">Current used animation</param>
    /// <param name="monsterList">monster list of the current level</param>
    //protected void CollisionChecker(Animation animation, GameObjectList monsterList)
    //{
    //    bool hit = false;
    //    foreach (Monster monsterobj in monsterList.Children)
    //    {
    //        if (monsterobj.CollidesWith(this))
    //        {
    //            monsterobj.TakeDamage(owner.Attributes.Attack + this.AttackDamage);
    //            hit = true;
    //            if (monsterobj.IsDead)
    //            {
    //                owner.Attributes.Gold += monsterobj.Attributes.Gold;
    //            }
    //        }
    //    }

    //    if (hit)
    //        prevAttackHit = true;
    //    else
    //        prevAttackHit = false;
    //}

    /// <summary>
    /// Collision checker for an animated weapon
    /// </summary>
    public void AnimationAttackCheck()
    {
        bool hit = false;
        foreach (Monster m in monsterObjectList.Children)
        {
            if (m.CollidesWith(Owner))
            {
                BasicAttack.AttackHit(m, fieldList);
                hit = true;

                if (m.IsDead)
                    owner.Attributes.Gold += m.Attributes.Gold;
                
            }
        }
        if (hit)
            prevAttackHit = true;
        else
            prevAttackHit = false;
    }

    abstract public void AnimationMainCheck();

    public void SwordDirectionChecker(Vector2 walkingdirection)
    {
        if (isAttacking)
        {
            if (Math.Abs(walkingdirection.X) >= Math.Abs(walkingdirection.Y))
            {
                if (walkingdirection.X > 0)
                {
                    AttackDirection = "Right";
                }
                else if (walkingdirection.X <= 0)
                {
                    AttackDirection = "Left";
                }
            }
            else if (Math.Abs(walkingdirection.Y) > Math.Abs(walkingdirection.X))
            {
                if (walkingdirection.Y > 0)
                {
                    AttackDirection = "Down";
                }
                else if (Math.Abs(walkingdirection.Y) > Math.Abs(walkingdirection.X))
                {
                    if (walkingdirection.Y > 0)
                    {
                        AttackDirection = "Down";
                    }

                    else if (walkingdirection.Y < 0)
                    {
                        AttackDirection = "Up";
                    }
                }
                else if (Math.Abs(walkingdirection.X) == Math.Abs(walkingdirection.Y) && walkingdirection.X > 0)
                {
                    AttackDirection = "Right";
                }
                else if (Math.Abs(walkingdirection.X) == Math.Abs(walkingdirection.Y) && walkingdirection.X < 0)
                {
                    AttackDirection = "Left";
                }
            }

            switch (AttackDirection)
            {
                case "Up":
                    Position = new Vector2(Owner.Position.X + 15, Owner.Position.Y - 12);
                    PlayAnimation("attack_up");
                    owner.PlayAnimation("attack_upwards");
                    break;
                case "Down":
                    Position = new Vector2(Owner.Position.X + 15, Owner.Position.Y - 30);
                    PlayAnimation("attack_down");
                    owner.PlayAnimation("attack_downwards");
                    break;
                case "Left":
                    Position = new Vector2(Owner.Position.X, Owner.Position.Y - 25);
                    PlayAnimation("attack_left");
                    owner.PlayAnimation("attack_fromleft");
                    break;
                case "Right":
                    Position = new Vector2(Owner.Position.X + 30, Owner.Position.Y - 25);
                    PlayAnimation("attack_right");
                    owner.PlayAnimation("attack_fromright");
                    break;
            }
        }
    }

    // Properties for the weapon
    public int AttackDamage
    {
        get { return attack; }
        protected set { attack = value; }
    }

    public bool IsAttacking
    {
        get { return isAttacking; }
        set { isAttacking = true; }
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

