using System;
using System.Collections.Generic;
using RoyT.AStar;
using Microsoft.Xna.Framework;

class BaseAI : GameObject
{
    protected GameObjectGrid levelGrid;
    protected SpriteGameObject targetedObject, owner;
    protected Vector2 movementSpeed;
    protected float sightRange;
    protected bool isMonster;
    protected Level currentLevel;
    public BaseAI(SpriteGameObject owner, Vector2 movementSpeed, Level currentLevel, bool isMonster = true, float sightRange = 50)
    {
        this.currentLevel = currentLevel;
        this.isMonster = isMonster;
        this.owner = owner;
        this.movementSpeed = movementSpeed;
        this.sightRange = sightRange;
        levelGrid = currentLevel.TileField;
    }

    public override void Update(GameTime gameTime)
    {
        LineOfSightChecker(sightRange);
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

    public void MoveToTarget()
    {
        Point[] path = FindPath(this.position, targetedObject.Position);
        
    }

    public void LineOfSightChecker(float sightRange)
    {
        GameObjectList targetList;
        if (isMonster)
            targetList = currentLevel.GameWorld.Find("playerLIST") as GameObjectList;
        else
            targetList = GameWorld.Find("monsterLIST") as GameObjectList;

        Circle lineOfSight = new Circle(sightRange + owner.Sprite.Width / 2, owner.Origin);
        foreach(SpriteGameObject obj in targetList.Children)
        {
            if (lineOfSight.CollidesWithRectangle(obj.BoundingBox))
            {
                TargetRandomObject(50, targetList);
            }
        }
    }

    public void TargetRandomObject(float chance, GameObjectList targetList)
    {
        foreach (SpriteGameObject target in targetList.Children)
        {
            int selectedNumber = GameEnvironment.Random.Next(0, 101);
            if (selectedNumber <= chance)
            {
                this.targetedObject = target;
                break;
            }
        }

        if (targetedObject == null)
        {
            TargetRandomObject(chance, targetList);
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
