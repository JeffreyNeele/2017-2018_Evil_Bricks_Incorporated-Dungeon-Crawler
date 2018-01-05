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
    protected GameObjectGrid fieldGrid;
    protected int damage;
    protected Vector2 pushVector;
    protected int pushCounts;
    protected Dictionary<Monster, int> affectedMonsters;
    protected List<Monster> monstersHitList;
    protected Dictionary<Monster, bool> directionPush;
    protected Ability(Character owner, ClassType classType)
    {
        this.classType = classType;
        this.owner = owner;
        pushCounts = 8;
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
        //projectileList.Update(gameTime);
        if (pushBackVector != new Vector2(0, 0) && monstersHitList != null)
            pushBack();
    }
    public virtual void Reset()
    {

    }

    public virtual void Use(Weapon weapon, string id)
    {
        monstersHitList = new List<Monster>();
        if(affectedMonsters == null)
            affectedMonsters = new Dictionary<Monster, int>();
        if (directionPush == null)
            directionPush = new Dictionary<Monster, bool>();
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

    public virtual void attackHit(Monster monster, GameObjectGrid field)
    {

    }

    public virtual void pushBack()
    {
        //Get the knockback vector of the ability;
        Vector2 push = pushBackVector;

        //Check only for the monsters in the monstersHitList, as it should only affect these monsters
        foreach(Monster monster in monstersHitList)
        {
            //In case with solid collision, take the previous pos.
            Vector2 previousPos = monster.Position;

            //If the dictionary does not contain the monster, go to the next monster
            if (!affectedMonsters.ContainsKey(monster))
                continue;

            //Get the pushCount of the current monster, this affects the knockback distance
            int pushCount = affectedMonsters[monster];

            push = givePushVector(pushCount, push);
            moveMonster(monster, push);

            //Check of het monster niet in een muur zit, anders zorg ervoor dat het monster wordt gepusht tot het uiterste punt dat mogelijk is
            if(knockedInWall(monster))
            {
                monster.Position = previousPos;
                for(int i = pushCount; i >= 0; i--)
                {
                    moveMonster(monster, givePushVector(i, pushBackVector));

                    if (knockedInWall(monster))
                        monster.Position = previousPos;
                    else
                        pushCount = 0;
                    
                }
            }
            affectedMonsters[monster] -= 1;

            if (pushCount <= 0)
            {
                affectedMonsters.Remove(monster);
                directionPush.Remove(monster);
            }
        }


    }

    // Returns a boolean that says if this ability is a projectile ability
    public bool ProjectileAbility
    {
        get { return isProjectileAbility; }
        protected set { isProjectileAbility = value; }
    }

    //Calculates the push vector
    protected Vector2 givePushVector(int pushCount, Vector2 push)
    {
        if (pushCount == pushCounts)
            push = push / 2;
        else
            push = push / 2 - new Vector2(2, 0) * (pushCounts - pushCount);

        if (push.X <= 0)
            push.X = 0;
        return push;
    }

    //Does the calculation of the position movement of the monster
    protected void moveMonster(Monster monster, Vector2 push)
    {
        if (directionPush[monster])
            monster.Position += push;
        else
            monster.Position -= push;
    }

    //Method that states if an enemy is knocked inside a wall.
    protected bool knockedInWall(Monster monster)
    {
        foreach (Tile tile in fieldGrid.Objects)
        {
            if (tile.IsSolid && tile.BoundingBox.Intersects(monster.BoundingBox))
            {
                return true;
            }
        }

        return false;
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

    public void monsterAdd(Monster monster, bool direction)
    {
        monsterHit.Add(monster);
        if (!affectedMonsters.ContainsKey(monster))
        {
            affectedMonsters.Add(monster, pushCounts);
            directionPush.Add(monster, direction);
        }
        else
            affectedMonsters[monster] = pushCounts;

        if (directionPush[monster] != direction)
            directionPush[monster] = direction;
            
    }
}
