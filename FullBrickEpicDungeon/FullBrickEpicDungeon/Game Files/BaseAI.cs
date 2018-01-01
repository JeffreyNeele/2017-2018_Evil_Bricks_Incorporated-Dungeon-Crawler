using System;
using System.Collections.Generic;
using RoyT.AStar;
using Microsoft.Xna.Framework;

class BaseAI : GameObject
{
    GameObjectGrid levelGrid;
    SpriteGameObject targetedObject, owner;
    Vector2 movementSpeed;
    public BaseAI(SpriteGameObject owner, Vector2 movementSpeed, Level currentLevel)
    {
        this.owner = owner;
        this.movementSpeed = movementSpeed;
        levelGrid = currentLevel.TileField;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    // Method that returns a list with points for the AI to follow.
    public Point[] FindPath(Vector2 endPosition, Vector2 startPosition)
    {
        levelGrid = GameWorld.Find("TileField") as GameObjectGrid;
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

    public void MoveToTarget()
    {
        Point[] path = FindPath(this.position, targetedObject.Position);
        
    }

    public void LineOfSightChecker(float sightRange)
    {
        GameObjectList playerList = GameWorld.Find("playerLIST") as GameObjectList;
        Circle lineOfSight = new Circle(sightRange + owner.Sprite.Width / 2, owner.Origin);
    }
    public void TargetRandomPlayer(float chance)
    {
        GameObjectList playerList = GameWorld.Find("playerLIST") as GameObjectList;
        foreach (Character player in playerList.Children)
        {
            int selectedNumber = GameEnvironment.Random.Next(0, 101);
            if (selectedNumber <= chance)
            {
                this.targetedObject = player;
                break;
            }
        }

        if (targetedObject == null)
        {
            TargetRandomPlayer(chance);
        }
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

    public Vector2 AImovementSpeed
    {
        get { return movementSpeed; }
        set { movementSpeed = value; }
    }
    public SpriteGameObject Owner
    {
        get { return owner; }
    }
}
