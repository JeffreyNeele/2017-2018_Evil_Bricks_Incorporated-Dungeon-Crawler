using Microsoft.Xna.Framework;

class Projectile : SpriteGameObject
{
    protected bool piercing;
    protected int damage;
    /// <summary>
    /// Class for a Projectile NOTE: Only usable for characters at the moment
    /// </summary>
    /// <param name="damage">Amount of damage this projectile does when it hits something</param>
    /// <param name="assetName">The assetname of this projectile (the image path)</param>
    /// <param name="facingLeft">Bool that makes the projectile go left</param>
    /// <param name="travellingSpeed">int that defines the speed of the projectile</param>
    /// <param name="piercing">bool for piercing, which means the projectile will not be deleted after hitting a monster</param>
    protected Projectile(int damage, string assetName, bool facingLeft, Vector2 travellingSpeed, bool piercing = false) : base (assetName)
    {
        this.damage = damage;
        this.piercing = piercing;
        if (facingLeft)
            this.velocity -= travellingSpeed;
        else
            this.velocity += travellingSpeed;
    }

    //updates the projectile
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        CollisionChecker();
    }

    /// <summary>
    /// Method that checks collision with walls and monsters
    /// </summary>
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