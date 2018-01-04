using System;
using System.Collections.Generic;
using RoyT.AStar;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class BaseAI
{
    protected GameObjectGrid levelGrid;
    protected SpriteGameObject targetedObject, owner;
    protected float sightRange, movementSpeed;
    protected bool isMonster;
    protected Level currentLevel;
    protected Vector2 direction;

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

            List<Vector2> waypointList = FindPath(targetedObject.Position, owner.Position);
            if (waypointList.Count > 0 && this.owner.Position == waypointList[0])
            {
                waypointList.RemoveAt(0);
            }

            if (waypointList.Count > 0)
            {
                MoveToPosition(waypointList[0], (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
        }
    }



    public void MoveToPosition(Vector2 targetGridPosition, float elapsedGameTime)
    {
        // Find the direction we have to go in
        direction = Vector2.Normalize(targetGridPosition - this.owner.Position);
        // AI moves to the direction with it's movementspeed and GameTime 
        this.owner.Position += direction * movementSpeed * elapsedGameTime;
        // this is for if we moved past the position, in this case we want go back to that position
        if (Math.Abs(Vector2.Dot(direction, Vector2.Normalize(targetGridPosition - this.owner.Position)) + 1) < 0.1F)
            this.owner.Position = targetGridPosition;
    }

    // Method that returns a list with points for the AI to follow.
    public List<Vector2> FindPath(Vector2 endPosition, Vector2 startPosition)
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
            }
        }
        Position[] pathArray = pathGrid.GetSmoothPath(new Position(gridStartPosition.X, gridStartPosition.Y), new Position(gridEndPosition.X, gridEndPosition.Y), MovementPatterns.LateralOnly);
        List<Vector2> pathList = new List<Vector2>();
        for (int i = 0; i < pathArray.Length; i++)
        {
            Vector2 realWaypoint = new Vector2((pathArray[i].X * levelGrid.CellWidth), (pathArray[i].Y * levelGrid.CellHeight));
            pathList.Add(realWaypoint);
        }
        if (pathList.Count > 0)
        {
            pathList.RemoveAt(0);
        }
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

    public Vector2 Direction
    {
        get { return direction; }
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
}
