using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

// Basic AI for monsters will be defined here
abstract partial class Monster : SpriteGameObject
{
    Character targetCharacter;

    public void LineOfSightChecker(float sightRange)
    {
        GameObjectList playerList = GameWorld.Find("playerLIST") as GameObjectList;
        Circle lineOfSight = new Circle(sightRange + sprite.Width / 2, this.origin);
    }
    public void TargetRandomPlayer(float chance)
    {
        GameObjectList playerList = GameWorld.Find("playerLIST") as GameObjectList;
        foreach (Character player in playerList.Children)
        {
            int selectedNumber = GameEnvironment.Random.Next(0, 101);
            if (selectedNumber <= chance)
            {
                this.targetCharacter = player;
                break;
            }
        }

        if(targetCharacter == null)
        {
            TargetRandomPlayer(chance);
        }
    }

    public void MoveToPlayer(Vector2 playerPosition)
    {
        Point[] path = pathFinder.FindPath(this.position, playerPosition);

    }

    public Character TargetCharacter
    {
        get { return targetCharacter; }
        set { targetCharacter = value; }
    }
}