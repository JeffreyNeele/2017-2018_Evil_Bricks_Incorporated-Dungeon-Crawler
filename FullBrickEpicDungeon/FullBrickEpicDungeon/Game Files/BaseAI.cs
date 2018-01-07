using System;
using System.Collections.Generic;
using RoyT.AStar;
using Microsoft.Xna.Framework;

class BaseAI
{
    protected GameObjectGrid levelGrid;
    protected SpriteGameObject targetedObject, owner;
    protected float sightRange, movementSpeed;
    protected bool isMonster;
    protected Level currentLevel;

    public BaseAI(SpriteGameObject owner, float movementSpeed, Level currentLevel, bool isMonster = true, float sightRange = 50)
    {
        this.currentLevel = currentLevel;
        this.isMonster = isMonster;
        this.owner = owner;
        this.movementSpeed = movementSpeed;
        this.sightRange = sightRange;
        levelGrid = currentLevel.TileField;
    }

    public void Update(GameTime gameTime)
    {
        if (targetedObject == null)
        {
            // LineOfSightChecker(sightRange);
            TargetRandomObject(100, levelGrid.GameWorld.Find("playerLIST") as GameObjectList);
        }
        else
        {
            List<Point> waypointList = FindPath(targetedObject.Position, owner.Position);
            if(waypointList.Count > 0 && MoveToPosition(waypointList[0], (float)gameTime.ElapsedGameTime.TotalSeconds))
            {
                waypointList.RemoveAt(0);
            }
        }

        
    }

    public bool MoveToPosition(Point targetGridPosition, float elapsedGameTime)
    {
        // Position given is a grid position and not a real position, so we have to add cellwidth and cellheight
        Vector2 realTargetPosition = new Vector2((targetGridPosition.X * levelGrid.CellWidth), (targetGridPosition.Y * levelGrid.CellHeight));
        if (this.owner.Position.Y == realTargetPosition.Y)
            return true;

        // Find the direction we have to go in
        Vector2 direction = Vector2.Normalize(realTargetPosition - this.owner.Position);
        // AI moves to the direction with it's movementspeed and GameTime 
        this.owner.Position += direction * movementSpeed * elapsedGameTime;

        // this is for if we moved past the position, in this case we want go back to that position
        if (Math.Abs(Vector2.Dot(direction, Vector2.Normalize(realTargetPosition - this.owner.Position)) + 1) < 0.1F)
            this.owner.Position = realTargetPosition;

        return this.owner.Position == realTargetPosition;
    }

    // Method that returns a list with points for the AI to follow.
    public List<Point> FindPath(Vector2 endPosition, Vector2 startPosition)
    {
        Point gridStartPosition = new Point((int)startPosition.X / levelGrid.CellWidth, (int)startPosition.Y / levelGrid.CellHeight);
        Point gridEndPosition = new Point((int)endPosition.X / levelGrid.CellWidth, (int)endPosition.Y / levelGrid.CellHeight);
        Grid pathGrid = new Grid(levelGrid.Columns, levelGrid.Rows);
        for (int x = 0; x < levelGrid.Columns; x++)
        {
            for (int y = 0; y < levelGrid.Rows; y++)
            {
                Tile currenttile = levelGrid.Objects[x, y] as Tile;
                if (currenttile.IsSolid)
                {
                    pathGrid.BlockCell(new Position(x, y));
                }
                if (currenttile.Type == TileType.Water)
                {
                    pathGrid.BlockCell(new Position(x, y));
                }
            }
        }
        Position[] pathArray = pathGrid.GetSmoothPath(new Position(gridStartPosition.X, gridStartPosition.Y), new Position(gridEndPosition.X, gridEndPosition.Y));
        List<Point> pathList = new List<Point>();
        for (int i = 0; i < pathArray.Length; i++)
        {
            pathList.Add(new Point(pathArray[i].X, pathArray[i].Y));
        }
        // The library automatically also counts the current tile the enemy is standing at, we do not want that in our method so we remove that here
        if(pathList.Count > 0)
            pathList.RemoveAt(0);
        return pathList;
    }

    public void LineOfSightChecker(float sightRange)
    {
        GameObjectList targetList;
        if (isMonster)
            targetList = currentLevel.GameWorld.Find("playerLIST") as GameObjectList;
        else
            targetList = currentLevel.GameWorld.Find("monsterLIST") as GameObjectList;

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

    public float AImovementSpeed
    {
        get { return movementSpeed; }
        set { movementSpeed = value; }
    }
    public SpriteGameObject Owner
    {
        get { return owner; }
    }

    public SpriteGameObject Target
    {
        get { return targetedObject; }
    }
}
