using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// the super basic class for ability, contains stuff that every type of ability HAS to do
abstract class Ability
{
    Character owner;
    protected GameObjectGrid fieldGrid;
    protected int damage;
    protected Vector2 pushVector, fallOff;
    protected int pushCounts;
    protected Dictionary<Monster, int> affectedMonsters;
    protected List<Monster> monstersHitList;
    protected Dictionary<Monster, bool> directionPush;
    protected Ability(Character owner)
    {
        this.owner = owner;
        PushTimeCount = 8;
        monstersHitList = new List<Monster>();
        affectedMonsters = new Dictionary<Monster, int>();
        directionPush = new Dictionary<Monster, bool>();
    }

    // Empty methods that will be defined better in subclasses
    public virtual void Update(GameTime gameTime)
    {
        if (PushBackVector != new Vector2(0, 0) && monstersHitList != null)
            PushBack();
    }
    public virtual void Reset()
    {

    }

    public virtual void Use(Weapon weapon, string id)
    {
        monstersHitList.Clear();
        if(affectedMonsters == null)
            affectedMonsters = new Dictionary<Monster, int>();
        if (directionPush == null)
            directionPush = new Dictionary<Monster, bool>();
        //weapon.PlayAnimation(id);
    }

    public virtual void AttackHit(Monster monster, GameObjectGrid field)
    {

    }

    public virtual void PushBack()
    {
        //Get the knockback vector of the ability;
        Vector2 push = PushBackVector;

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

            push = GivePushVector(pushCount, push);
            MoveMonster(monster, push);

            //Check of het monster niet in een muur zit, anders zorg ervoor dat het monster wordt gepusht tot het uiterste punt dat mogelijk is
            if(KnockedInWall(monster))
            {
                monster.Position = previousPos;
                for(int i = pushCount; i >= 0; i--)
                {
                    MoveMonster(monster, GivePushVector(i, PushBackVector));

                    if (KnockedInWall(monster))
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

    //Calculates the push vector
    protected Vector2 GivePushVector(int pushCount, Vector2 push)
    {
        if (pushCount == PushTimeCount)
            push = push / 2;
        else
            push = push / 2 - fallOff * (PushTimeCount - pushCount);

        if (push.X <= 0)
            push.X = 0;
        return push;
    }

    //Does the calculation of the position movement of the monster
    protected void MoveMonster(Monster monster, Vector2 push)
    {
        if (directionPush[monster])
            monster.Position += push;
        else
            monster.Position -= push;
    }

    //Method that states if an enemy is knocked inside a wall.
    protected bool KnockedInWall(Monster monster)
    {
        foreach (Tile tile in fieldGrid.Objects)
        {
            Rectangle quarterBoundingBox = new Rectangle((int)monster.BoundingBox.X, (int)(monster.BoundingBox.Y + 0.75 * monster.Height), monster.Width, (int)(monster.Height / 4));
            if (tile.IsSolid && tile.BoundingBox.Intersects(quarterBoundingBox))
            {
                return true;
            }
        }
        return false;
    }

    public Character Owner
    {
        get { return owner; }
    }

    public int DamageAA
    {
        get { return damage; }
        set { damage = value; }
    }

    public Vector2 PushBackVector
    {
        get { return pushVector; }
        set { pushVector = value; }
    }

    public Vector2 PushFallOff
    {
        get { return fallOff; }
        set { fallOff = value; }
    }

    public int PushTimeCount
    {
        get { return pushCounts; }
        set { pushCounts = value; }
    }

    public List<Monster> MonsterHit
    {
        get { return monstersHitList; }
    }

    public void MonsterAdd(Monster monster, bool direction)
    {
        MonsterHit.Add(monster);
        if (!affectedMonsters.ContainsKey(monster))
        {
            affectedMonsters.Add(monster, PushTimeCount);
            directionPush.Add(monster, direction);
        }
        else
            affectedMonsters[monster] = PushTimeCount;

        if (directionPush[monster] != direction)
            directionPush[monster] = direction;
            
    }
}
