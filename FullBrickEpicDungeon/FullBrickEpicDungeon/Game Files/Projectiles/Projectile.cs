using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

class Projectile : SpriteGameObject
{
    protected bool piercing;
    protected int damage;
    protected Projectile(int damage, string assetName, bool piercing = false) : base (assetName)
    {
        this.damage = damage;
        this.piercing = piercing;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        CollisionChecker();
    }
    protected void CollisionChecker()
    {

    }
}