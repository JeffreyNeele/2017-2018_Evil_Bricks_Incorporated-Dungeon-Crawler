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
    protected Ability(Character owner, ClassType classType)
    {
        this.classType = classType;
        this.owner = owner;
        try
        {
            projectileList = projectileList.GameWorld.Find("projectileLIST") as GameObjectList;
        }
        catch
        {
            throw new ArgumentNullException("the projectile list was not found, most of the times this is because you tried to make the Ability before the Level was initialized");
        }

    }

    public virtual void Use()
    {

    }

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
    public virtual void Update(GameTime gameTime)
    {
        
    }

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
}
