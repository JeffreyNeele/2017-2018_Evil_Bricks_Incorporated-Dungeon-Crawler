using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

class LittlePenguin : AImonster
{
    float slideSpeed;
    Vector2 targetPos, movementVector;
    int idleCounter;
    public LittlePenguin(Level currentLevel) : base(0f, currentLevel, "LittlePenguin")
    {
        this.baseattributes.HP = 80;
        this.baseattributes.Armour = 0;
        this.baseattributes.Attack = 0;
        this.baseattributes.Gold = 75;
        attributes = baseattributes;

        //The total speed of movement of the penguin
        slideSpeed = 100f;

        //Because the penguin will be idle for a while after sliding, the movement vector will have to be set (0,0) from time to time
        movementVector = new Vector2(0, 0);
        idleCounter = 3;
        //Load the animations, for now a bunny animation is taken.
        LoadAnimation("Assets/Sprites/Enemies/bunny_default", "idle", false);
        PlayAnimation("idle");
    }

    float totalDistance;
    //Method that defines the movementVector of the penguin.
    public void slideDirection()
    {
        //Take the position of the target
        GameObjectList players = GameWorld.Find("playerLIST") as GameObjectList;
        foreach(Character player in players.Children)
            if(player == AI.Target)
            {
                targetPos = player.Position;
            }

        Vector2 differencePos = position - targetPos;
        movementVector = getMovementVector(differencePos);
    }

    //Method for checking if the penguin collides with a wall
    public bool CheckCollision()
    {
        //Take the grid to check for all tiles that are solid or doors
        GameObjectGrid field = GameWorld.Find("TileField") as GameObjectGrid;

        return false;
    }

    //Method that calculates the movement vector of the penguin
    public Vector2 getMovementVector(Vector2 difference)
    {
        totalDistance = (float)Math.Sqrt(difference.X * difference.X + difference.Y * difference.Y);
        float scaling = totalDistance / slideSpeed;

        return new Vector2(difference.X * scaling, difference.Y * scaling);
    }


}

