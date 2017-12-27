using System;
using System.Collections.Generic;
using RoyT.AStar;
using Microsoft.Xna.Framework;

class BaseAI : GameObject
{
    GameObjectGrid levelGrid;
    Vector2 parentMovementSpeed;
    public BaseAI(GameObjectGrid levelGrid)
    {
        this.levelGrid = levelGrid;
        parentMovementSpeed = new Vector2(2.5F, 2.5F);
    }

    public void FollowPath()
    {

    }

    // Method that returns a list with points for the AI to follow.
    public Point[] FindPath(Vector2 endPosition, Vector2 startPosition)
    {
        Grid pathGrid = new Grid(levelGrid.Columns, levelGrid.CellWidth);
        for (int x = 0; x < levelGrid.Columns; x++)
        {
            for (int y = 0; y < levelGrid.Rows; y++)
            {
                Tile currenttile = levelGrid.Objects[x, y] as Tile;
                if (currenttile.isSolid)
                {
                    pathGrid.BlockCell(new Position(x, y));
                }
                if (currenttile.Type == TileType.Water)
                {
                    pathGrid.SetCellCost(new Position(x, y), 10);
                }
            }
        }
        Position[] pathArray = pathGrid.GetSmoothPath(new Position((int)startPosition.X, (int)startPosition.Y), new Position((int)endPosition.X, (int)endPosition.Y));
        Point[] pointArray = new Point[pathArray.Length];
        for (int i = 0; i < pathArray.Length; i++)
        {
            pointArray[i] = new Point(pathArray[i].X, pathArray[i].Y);
        }
        return pointArray;
    }

    public Vector2 MovementVector(Vector2 movementSpeed, float angle)
    {
        float adjacent = movementSpeed.X;
        float opposite = movementSpeed.Y;

        float hypotenuse = (float)Math.Sqrt(adjacent * adjacent + opposite * opposite);
        adjacent = (float)Math.Cos(angle * (Math.PI / 180)) * hypotenuse;
        opposite = (float)Math.Sin(angle * (Math.PI / 180)) * hypotenuse;

        return new Vector2(adjacent, opposite);
    }

    /*

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

        if (targetCharacter == null)
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
    */
    public Vector2 ParentMovementSpeed
    {
        get { return parentMovementSpeed; }
        set { parentMovementSpeed = value; }
    }
}
