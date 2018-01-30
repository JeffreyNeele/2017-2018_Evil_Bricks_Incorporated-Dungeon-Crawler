using System;
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
    protected bool isAttacking, shieldAnimation, SwordAnimation;
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
            SwordAnimation = false;
            shieldAnimation = false;
            attackAnimationTimer.Reset();
        }

        if (!this.CurrentAnimation.AnimationEnded)
        {
            if (idAnimation == idBaseAA)
            {
                AnimationAttackCheck();
            }
            else if (idAnimation == idMainAbility)
            {
                AnimationMainCheck();
            }
        }


        BasicAttack.Update(gameTime);
        mainAbility.Update(gameTime);

        base.Update(gameTime);
    }

    // This is the base attack method of the weapon,, which will be also defined as an ability
    public virtual void Attack(GameObjectList monsterList, GameObjectGrid field)
    {
        BasicAttack.Use();
        monsterObjectList = monsterList;
        fieldList = field;
        idAnimation = idBaseAA;
        AnimationAttackCheck();
    }



    // Uses the main ability
    public virtual void UseMainAbility(GameObjectList monsterList, GameObjectGrid field)
    {
        if (!mainAbility.IsOnCooldown)
        {
            mainAbility.Use();
            monsterObjectList = monsterList;
            fieldList = field;
            idAnimation = idMainAbility;
            AnimationMainCheck();
        }
    }

    /*
    public virtual void UseSpecialAbility(GameObjectList monsterList)
    {
        monsterObjectList = monsterList;
        specialAbility.Use();
        CollisionChecker(this.CurrentAnimation, monsterObjectList);
    }
    */

    /// <summary>
    /// Collision checker for an animated weapon
    /// </summary>
    public void AnimationAttackCheck()
    {
        bool hit = false;
        foreach (Monster m in monsterObjectList.Children)
        {
            if (m.BoundingBox.Intersects(this.BoundingBox))
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

    public void SwordDirectionCheckerManager(Vector2 walkingdirection)
    {
        string attackDirection = "";
        if (isAttacking && SwordAnimation)
        {
            if (Math.Abs(walkingdirection.X) >= Math.Abs(walkingdirection.Y))
            {
                attackDirection = HorizontalSwordDirection(walkingdirection);
            }
            else if (Math.Abs(walkingdirection.Y) > Math.Abs(walkingdirection.X))
            {
                attackDirection = VerticalSwordDirection(walkingdirection);
            }
            PlaySwordAnimations(attackDirection);
        }
    }

    private string HorizontalSwordDirection(Vector2 walkingdirection)
    {
        if (walkingdirection.X > 0)
        {
            return "Right";
        }
        else if (walkingdirection.X <= 0)
        {
            return "Left";
        }
        return "";
    }

    private string VerticalSwordDirection(Vector2 walkingdirection)
    {
        if (walkingdirection.Y > 0)
        {
            return "Down";
        }
        else if (Math.Abs(walkingdirection.Y) > Math.Abs(walkingdirection.X))
        {
            return "Up";
        }
        else if (Math.Abs(walkingdirection.X) == Math.Abs(walkingdirection.Y) && walkingdirection.X > 0)
        {
            return "Right";
        }
        else if (Math.Abs(walkingdirection.X) == Math.Abs(walkingdirection.Y) && walkingdirection.X <= 0)
        {
            return "Left";
        }
        return "";
    }

    /// <summary>
    /// PLays the correct animations for sword attacks
    /// </summary>
    /// <param name="attackDirection">string that represents the attack direction</param>
    private void PlaySwordAnimations(string attackDirection)
    {
        switch (attackDirection)
        {
            // If the string is empty, we have no animation to play so we return
            case "":
                return;
            case "Up":
                Position = new Vector2(Owner.Position.X, Owner.Position.Y - 30);
                PlayAnimation("attack_up");
                owner.PlayAnimation("attack_upwards");
                break;
            case "Down":
                Position = new Vector2(Owner.Position.X, Owner.Position.Y + 30);
                PlayAnimation("attack_down");
                owner.PlayAnimation("attack_downwards");
                break;
            case "Left":
                Position = new Vector2(Owner.Position.X - 30, Owner.Position.Y - 10);
                PlayAnimation("attack_left");
                owner.PlayAnimation("attack_fromleft");
                break;
            case "Right":
                Position = new Vector2(Owner.Position.X + 30, Owner.Position.Y - 10);
                PlayAnimation("attack_right");
                owner.PlayAnimation("attack_fromleft");
                break;
            default:
                throw new ArgumentException("No direction given to play animations for");
        }
    }

    public void ShieldDirectionCheckerManager(Vector2 walkingdirection)
    {
        string attackDirection = "";
        if (isAttacking && shieldAnimation)
        {
            if (Math.Abs(walkingdirection.X) >= Math.Abs(walkingdirection.Y))
            {
                attackDirection = HorizontalShieldDirection(walkingdirection);
            }
            else if (Math.Abs(walkingdirection.Y) > Math.Abs(walkingdirection.X))
            {
                attackDirection = VerticalShieldDirection(walkingdirection);
            }
            PlayShieldAnimations(attackDirection);
        }
    }

    private string HorizontalShieldDirection(Vector2 walkingdirection)
    {
        if (walkingdirection.X > 0)
        {
            return "Right";
        }
        else if (walkingdirection.X <= 0)
        {
            return "Left";
        }
        return "";
    }

    private string VerticalShieldDirection(Vector2 walkingdirection)
    {
        if (walkingdirection.Y > 0)
        {
            return "Down";
        }
        else if (Math.Abs(walkingdirection.Y) > Math.Abs(walkingdirection.X))
        {
            return "Up";
        }
        else if (Math.Abs(walkingdirection.X) == Math.Abs(walkingdirection.Y) && walkingdirection.X > 0)
        {
            return "Right";
        }
        else if (Math.Abs(walkingdirection.X) == Math.Abs(walkingdirection.Y) && walkingdirection.X <= 0)
        {
            return "Left";
        }
        return "";
    }

    /// <summary>
    /// PLays the correct animations for shield attacks
    /// </summary>
    /// <param name="attackDirection">string that represents the attack direction</param>
    private void PlayShieldAnimations(string attackDirection)
    {
        switch (attackDirection)
        {
            // If the string is empty, we have no animation to play so we return
            case "":
                return;
            case "Up":
                Position = new Vector2(Owner.Position.X, Owner.Position.Y - 30);
                PlayAnimation("shield_up");
                owner.PlayAnimation("shield_upwards");
                break;
            case "Down":
                Position = new Vector2(Owner.Position.X, Owner.Position.Y + 30);
                PlayAnimation("shield_down");
                owner.PlayAnimation("shield_downwards");
                break;
            case "Left":
                Position = new Vector2(Owner.Position.X - 30, Owner.Position.Y - 10);
                PlayAnimation("shield_left");
                owner.PlayAnimation("shield_fromleft");
                break;
            case "Right":
                Position = new Vector2(Owner.Position.X + 30, Owner.Position.Y - 10);
                PlayAnimation("shield_right");
                owner.PlayAnimation("shield_fromleft");
                break;
            default:
                throw new ArgumentException("No direction given to play animations for");
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

    public bool IsBaseAA
    {
        get { return SwordAnimation; }
        set { SwordAnimation = value; }
    }

    public bool IsShieldAA
    {
        get { return shieldAnimation; }
        set { shieldAnimation = value; }
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

