using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

class Projectile : SpriteGameObject
{
    protected bool piercing;
    protected int damage;
    protected Projectile(int damage, string assetName, bool facingLeft, Vector2 travellingSpeed, bool piercing = false) : base (assetName)
    {
        this.damage = damage;
        this.piercing = piercing;
        if (facingLeft)
            this.velocity -= travellingSpeed;
        else
            this.velocity += travellingSpeed;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        CollisionChecker();
    }
    protected void CollisionChecker()
    {
        GameObjectList projectileList = GameWorld.Find("projectileLIST") as GameObjectList;
        GameObjectList monsterList = GameWorld.Find("monsterLIST") as GameObjectList;
        // TODO: Add Tilefield collision with walls puzzles etc, (not doable atm as it isn't programmed as of writing this)
        foreach (Monster monsterobj in monsterList.Children)
        {
            if (monsterobj.CollidesWith(this))
            {
                monsterobj.TakeDamage(damage);
                if (!piercing)
                {
                    projectileList.Remove(this);
                }
            }
        }
    }

}