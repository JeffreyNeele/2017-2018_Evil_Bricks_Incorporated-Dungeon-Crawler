using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// the super basic class for ability, contains stuff that every type of ability HAS to do
abstract class Ability
{
    protected bool isProjectileAbility;
    protected ClassType classType;
    protected Ability(ClassType classType)
    {
        this.classType = classType;
    }

    public virtual void Update(GameTime gameTime)
    {
        
    }

    public virtual void Use()
    {

    }

    public bool ProjectileAbility
    {
        get { return isProjectileAbility; }
        protected set { isProjectileAbility = value;}
    }

    public ClassType Type
    {
        get { return classType; }
    }
}
