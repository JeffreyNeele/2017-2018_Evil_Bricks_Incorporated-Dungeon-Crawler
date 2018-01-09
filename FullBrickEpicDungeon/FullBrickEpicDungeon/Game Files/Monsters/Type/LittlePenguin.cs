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
    float idleCounter;
    public LittlePenguin(Level currentLevel) : base(0f, currentLevel, "LittlePenguin")
    {
        this.baseattributes.HP = 80;
        this.baseattributes.Armour = 0;
        this.baseattributes.Attack = 0;
        this.baseattributes.Gold = 75;
        attributes = baseattributes;

        //The total speed of movement of the penguin
        slideSpeed = 10f;

        //Because the penguin will be idle for a while after sliding, the movement vector will have to be set (0,0) from time to time
        movementVector = new Vector2(0, 0);
        idleCounter = 3;
        //Load the animations, for now a bunny animation is taken.
        LoadAnimation("Assets/Sprites/Enemies/bunny_default", "idle", false);
        PlayAnimation("idle");
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        if (AI.CurrentTarget != null && idleCounter <= 0 && movementVector == new Vector2(0, 0))
            SlideDirection();
        else if (movementVector != new Vector2(0, 0))
        {
            previousPos = Position;
            Position += movementVector;
            CheckCollision();
        }
        else if (idleCounter > 0)
            idleCounter -= (float)gameTime.ElapsedGameTime.TotalSeconds;
    }

    //The penguin is special in the way that it attacks when it is sliding from 1 point to another.
    //This means that the attack method for the penguin is obsolete. The AttackHit, however, is still important.
    public override void Attack()
    {
    }

    //Method for checking if the penguin collides with a wall or player
    public void CheckCollision()
    {
        //Take the grid to check for all tiles that are solid or doors, and player list for collision with players
        GameObjectGrid field = GameWorld.Find("TileField") as GameObjectGrid;
        GameObjectList players = GameWorld.Find("playerLIST") as GameObjectList;

        Rectangle tileBoundingBox = new Rectangle((int)this.BoundingBox.X, (int)(this.BoundingBox.Y + 0.75 * Height), this.Width, (int)(this.Height / 4));
        //First check if monster collides with player
        foreach (Character player in players.Children)
            if (this.BoundingBox.Intersects(player.BoundingBox) && !playersHit.Contains(player))
                AttackHit(player);

        //Then check if monster collides with solid tile
        foreach (Tile tile in field.Objects)
        {
            if (tile.IsSolid && tileBoundingBox.Intersects(tile.BoundingBox))
            {
                movementVector = new Vector2(0, 0);
                Position = previousPos;
                idleCounter = 3;
                AI.TargetRandomObject(50);
            }
        }
    }

    float totalDistance;
    //Method that defines the movementVector of the penguin.
    public void SlideDirection()
    {

        //Take the position of the target
        GameObjectList players = GameWorld.Find("playerLIST") as GameObjectList;
        foreach(Character player in players.Children)
            if(player == AI.CurrentTarget)
            {
                targetPos = player.Position;
            }

        Vector2 differencePos = targetPos - position;
        movementVector = getMovementVector(differencePos);
    }

    //Method that calculates the movement vector of the penguin
    public Vector2 getMovementVector(Vector2 difference)
    {
        totalDistance = (float)Math.Sqrt(difference.X * difference.X + difference.Y * difference.Y);
        float scaling = slideSpeed / totalDistance;

        return new Vector2(difference.X * scaling, difference.Y * scaling);
    }


}

