using System;
using System.Collections.Generic;
using RoyT.AStar;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// Basic AI used for characters and monsters make sure to set the isMonster boolean correctly
class BaseAI
{
    protected GameObjectGrid levelGrid;
    protected GameObjectList targetList;
    protected AnimatedGameObject targetedObject, owner;
    protected float sightRange, movementSpeed;
    protected bool isMonster, isAttacking;
    protected Level currentLevel;
    protected Vector2 direction;
    protected Timer idleTimer;
    public BaseAI(AnimatedGameObject owner, float movementSpeed, Level currentLevel, bool isMonster = true, float idleTime = 1.25F, float sightRange = 200)
    {
        idleTimer = new Timer(idleTime)
        {
            IsExpired = true
        };
        this.currentLevel = currentLevel;
        this.isMonster = isMonster;
        this.owner = owner;
        this.movementSpeed = movementSpeed;
        this.sightRange = sightRange;
        levelGrid = currentLevel.TileField;
        
        if (isMonster)
            targetList = currentLevel.GameWorld.Find("playerLIST") as GameObjectList;
        else
            targetList = currentLevel.GameWorld.Find("monsterLIST") as GameObjectList;
    }

    public void Update(GameTime gameTime)
    {
        if (targetedObject == null)
        {
            if (isMonster)
            {
                LineOfSightChecker(sightRange);
            }
            else 
            {
                TargetRandomObject(50);
            }
        }
        else if (idleTimer.IsExpired)
        {
            List<Vector2> waypointList = FindPath(targetedObject.Position, owner.Position);
            if (owner.BoundingBox.Intersects(targetedObject.BoundingBox))
            {
                MoveToPosition(targetedObject.Position - targetedObject.Origin, (float)gameTime.ElapsedGameTime.TotalSeconds);
                if (targetedObject.BoundingBox.Contains(owner.Position))
                {
                    if (isMonster)
                    {
                        isAttacking = true;
                        Monster owner_cast = owner as Monster;
                        Character target_cast = targetedObject as Character;
                        owner_cast.Attack(target_cast);
                        if (target_cast.IsDowned)
                        {
                            targetedObject = null;
                        }
                        idleTimer.Reset();
                    }
                    else
                    {
                        isAttacking = true;
                        Character owner_cast = owner as Character;
                        Monster target_cast = targetedObject as Monster;
                        GameObjectList monsterList = currentLevel.GameWorld.Find("monsterLIST") as GameObjectList;
                        owner_cast.CurrentWeapon.Attack(monsterList, currentLevel.TileField);
                        if (target_cast.IsDead)
                        {
                            targetedObject = null;
                        }
                        idleTimer.Reset();
                    }
                }
            }
            else if (waypointList.Count > 0 && this.owner.Position == waypointList[0])
            {
                waypointList.RemoveAt(0);

            }
            if (waypointList.Count > 0)
            {
                MoveToPosition(waypointList[0], (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
        }
        else
        {
            if (owner.CurrentAnimation.AnimationEnded)
            {
                isAttacking = false;
            }
            idleTimer.Update(gameTime);
        }
    }



    public void MoveToPosition(Vector2 targetGridPosition, float elapsedGameTime)
    {
        targetGridPosition = new Vector2((targetGridPosition.X + levelGrid.CellWidth / 2), (targetGridPosition.Y + levelGrid.CellHeight));
        // Find the direction we have to go in
        direction = Vector2.Normalize(targetGridPosition - this.owner.Position);
        // AI moves to the direction with it's movementspeed and GameTime 
        this.owner.Position += direction * movementSpeed * elapsedGameTime;
        // this is for if we moved past the position, in this case we want go back to that position


        // NOTE: this code block currently for some reason deletes the bunny or overflow a variable without error, removing this makes the AI way less efficient
        // but with the code block it doesn't work at all
        /*
        if (Vector2.Distance(direction, Vector2.Normalize(this.owner.Position - targetGridPosition)) < 0.1f)
        {
            this.owner.Position = targetGridPosition;
        }
        */
    }

    // Method that returns a list with points for the AI to follow.
    public List<Vector2> FindPath(Vector2 endPosition, Vector2 startPosition)
    {
        Point gridStartPosition = new Point((int)startPosition.X / levelGrid.CellWidth, (int)startPosition.Y / levelGrid.CellHeight);
        Point gridEndPosition = new Point((int)endPosition.X / levelGrid.CellWidth, (int)(endPosition.Y / levelGrid.CellHeight));

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
        

        Circle lineOfSight = new Circle(sightRange, owner.Origin);
        foreach(AnimatedGameObject obj in targetList.Children)
        {
            if (lineOfSight.CollidesWithRectangle(obj, owner))
            {
                targetedObject = obj;
            }
        }
    }

    public void TargetRandomObject(float chance)
    {
        foreach (AnimatedGameObject target in targetList.Children)
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
            TargetRandomObject(chance);
        }
    }

    public bool IsAttacking
    {
        get { return isAttacking; }
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

    public SpriteGameObject CurrentTarget
    {
        get { return targetedObject; }
    }
}
