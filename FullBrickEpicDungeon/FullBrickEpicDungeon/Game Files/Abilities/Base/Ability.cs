using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// the super basic class for ability, contains stuff that every type of ability HAS to do
abstract class Ability
{
    Character owner;
    protected ClassType classType;
    protected bool isProjectileAbility; 
    protected GameObjectList projectileList;
    protected int damage;
    protected Vector2 pushVector;
    protected List<Monster> monstersHitList;
    protected Ability(Character owner, ClassType classType)
    {
        this.classType = classType;
        this.owner = owner;
        // Ability needs the main projectile list (defined in LevelMain.cs) 
        /*try
        {
            projectileList = projectileList.GameWorld.Find("projectileLIST") as GameObjectList;
        }
        catch
        {
            throw new ArgumentNullException("the projectile list was not found, most of the times this is because you tried to make the Ability before the Level was initialized");
        }*/

    }

    // Empty methods that will be defined better in subclasses
    public virtual void Update(GameTime gameTime)
    {

    }
    public virtual void Reset()
    {

    }
    public virtual void Use(Weapon weapon, string id)
    {
        monstersHitList = new List<Monster>();
        //weapon.PlayAnimation(id);
    }

    // Adds a projectile to the ability, only a basic one, for advanced other subclasses of projectile override this class
    public virtual void AddProjectile(string projectileAsset, bool piercing)
    {
        if (isProjectileAbility)
        {
           // Projectile proj = new Projectile(owner.CurrentWeapon.AttackDamage + owner.Attributes.Attack, projectileAsset, );
           // projectileList.Add(proj);
        }
        else
        {
            throw new ArgumentException("this is not a projectile ability and this method can therefore not be used");
        }
    }

    public virtual void attackHit(Monster monster)
    {

    }

    // Returns a boolean that says if this ability is a projectile ability
    public bool ProjectileAbility
    {
        get { return isProjectileAbility; }
        protected set { isProjectileAbility = value; }
    }

    public ClassType Type
    {
        get { return classType; }
    }

    public Character Owner
    {
        get { return owner; }
    }

    public int damageAA
    {
        get { return damage; }
        set { damage = value; }
    }

        public Vector2 pushBackVector
    {
        get { return pushVector; }
        set { pushVector = value; }
    }

    public List<Monster> monsterHit
    {
        get { return monstersHitList; }
    }

    public void monsterAdd(Monster monster)
    {
        monsterHit.Add(monster);
    }
}
