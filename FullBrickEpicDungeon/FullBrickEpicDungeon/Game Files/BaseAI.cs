using System;
using System.Collections.Generic;
// Library for pathfinding
using RoyT.AStar;
using Microsoft.Xna.Framework;

class BaseAI
{
    // the current level Grid
    protected GameObjectGrid levelGrid;
    // List of potential targets for the AI
    protected GameObjectList targetList;
    // variables that define the owner of the object, and the currently targeted object
    protected AnimatedGameObject targetedObject, owner;
    protected float sightRange, movementSpeed;
    protected bool isMonster, isAttacking, randomTargeting;
    protected Level currentLevel;
    protected Vector2 direction;
    protected Timer idleTimer;
    
    /// <summary>
    /// Class that defines a Basic AI for the Dungeon Crawler game. Both for Characters and Monsters. 
    /// </summary>
    /// <param name="owner">The owner of this AI (and the thing that the AI perfoms it's operations on)</param>
    /// <param name="movementSpeed">Variable that determines how fast the AI moves</param>
    /// <param name="currentLevel">The current level the owner of the AI is currently in</param>
    /// <param name="isMonster">Bool that determines whether this AI follows monster or player logic</param>
    /// <param name="idleTime">Amount of time an AI waits after attacking</param>
    /// <param name="sightRange">Distance of the LOS of the AI</param>
    public BaseAI(AnimatedGameObject owner, float movementSpeed, Level currentLevel, bool isMonster = true, float sightRange = 200, float idleTime = 1.4F)
    {
        idleTimer = new Timer(idleTime)
        {
            IsExpired = true
        };
        // assign the given variables to the local variables
        this.currentLevel = currentLevel;
        this.isMonster = isMonster;
        this.owner = owner;
        this.movementSpeed = movementSpeed;
        this.sightRange = sightRange;
        levelGrid = currentLevel.TileField;
        // Find the potential targets of this AI
        if (isMonster)
            targetList = currentLevel.GameWorld.Find("playerLIST") as GameObjectList;
        else
            targetList = currentLevel.GameWorld.Find("monsterLIST") as GameObjectList;
        randomTargeting = false;
    }

    /// <summary>
    /// Update logic for the AI
    /// </summary>
    /// <param name="gameTime">current game time</param>
    public void Update(GameTime gameTime)
    {
        //find a target if the current target is null
        if (targetedObject == null)
        {
            LineOfSightChecker(sightRange);
            //if we do not find a target as a character AI and we are currently in a wall, get out of this wall
            if (!isMonster)
            {
                LineOfSightChecker(sightRange);
                GetOutWallChecker();
            }
        }

        // if the AI is not waiting for the idleTimer to expire it enters this code block
        else if (idleTimer.IsExpired)
        {
            //Keep checking if there is a target closer than the current target
            LineOfSightChecker(sightRange);
            // generate a waypointlist towards the target
            List<Vector2> waypointList = FindPath(targetedObject.Position + new Vector2(0, (targetedObject.Height / 4) + 1), owner.Position);
            // checks if the AI is already at it's target
            if (owner.BoundingBox.Intersects(targetedObject.BoundingBox))
            {
                AIAttackManager(gameTime);
            }
            // If we have reached a waypoint, remove it
            else if (waypointList.Count > 0 && this.owner.Position == waypointList[0])
            {
                waypointList.RemoveAt(0);
            }
            // Moves the AI towards it's target
            if (waypointList.Count > 0)
            {
                MoveToPosition(waypointList[0], (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
        }
        // if the idle animation is not expired the AI enters this code block
        else
        {
            // if the animation ended, we are no longer attacking and thus we set the variable to false.
            if (owner.CurrentAnimation.AnimationEnded)
            {
                isAttacking = false;
            }
            // update the time in the idletimer
            idleTimer.Update(gameTime);
        }
    }

    /// <summary>
    /// Method that puts the AI out of a wall, is used because the AI does not have a complete obstacle avoidance,
    /// and this method makes it so the player can always switch to the AI and not get stuck
    /// </summary>
    public void GetOutWallChecker()
    {
        Character owner_cast = owner as Character;
        if (!owner_cast.SolidCollisionChecker())
        {
            GameObjectGrid field = currentLevel.GameWorld.Find("TileField") as GameObjectGrid;
            // Rectangle that is set to zero but will be assigned to the tile the character is colloding with
            Rectangle collidingBoundingBox = new Rectangle(0, 0, 0, 0);
            for (int x = 0; x < field.Columns; x++)
            {
                for (int y = 0; y < field.Rows; y++)
                {
                    if ((field.Objects[x, y] as Tile).IsSolid && (field.Objects[x, y] as Tile).BoundingBox.Intersects(owner_cast.IsometricBoundingBox))
                    {
                        // if we find the bounding box the AI collides with, assign it to the colliding bounding box
                        collidingBoundingBox = (field.Objects[x, y] as Tile).BoundingBox;
                        break;
                    }
                }
            }
            // Calculate the collisiondepth
            Vector2 collisionDepth = Collision.CalculateIntersectionDepth(owner_cast.IsometricBoundingBox, collidingBoundingBox);
            // Place the character outside the wall
            owner_cast.Position += collisionDepth;
        }
    }

    /// <summary>
    /// Method that manages the AI attack logic methods. First moves to the target and then calls the correct attack method.
    /// </summary>
    /// <param name="gameTime">Current game time</param>
    private void AIAttackManager(GameTime gameTime)
    {
        // if the AI is inside the target it attacks
        if (targetedObject.BoundingBox.Contains(owner.Position))
        {
            if (isMonster)
            {
                isAttacking = true;
                MonsterAttackLogic();
            }
            else
            {
                CharacterAttackLogic();
            }
            idleTimer.Reset();
        }
        else
            MoveToPosition(targetedObject.Position, (float)gameTime.ElapsedGameTime.TotalSeconds);
    }

    /// <summary>
    /// Attack logic for Characters, makes the Character use abilities, attacks and retargets if the target it was attacking died;
    /// </summary>
    private void CharacterAttackLogic()
    {
        // cast both the owner and target so we can access the Character / Monster properties
        Character owner_cast = owner as Character;
        Monster target_cast = targetedObject as Monster;
        owner_cast.CurrentWeapon.IsAttacking = true;
        GameObjectList monsterList = currentLevel.GameWorld.Find("monsterLIST") as GameObjectList;
        // if our ability is not on cooldown we use it.
        if (!owner_cast.CurrentWeapon.AbilityMain.IsOnCooldown)
        {
            owner_cast.CurrentWeapon.IsShieldAA = true;
            owner_cast.CurrentWeapon.UseMainAbility(monsterList, currentLevel.TileField);
            owner_cast.PlaySFX("basic_ability");
        }
        // otherwise perform a normal strike
        else
        {
            owner_cast.CurrentWeapon.IsBaseAA = true;
            owner_cast.CurrentWeapon.Attack(monsterList, currentLevel.TileField);
            owner_cast.PlaySFX("attack_hit");
        }

        // if the target is dead we set the targetedobject to null and wait for another monster to enter our LOS again
        if (target_cast.IsDead)
        {
            targetedObject = null;
        }
    }

    /// <summary>
    /// Attack Logic method for monsters, attacks the character and finds a new target if the character it waas attacking got downed
    /// </summary>
    private void MonsterAttackLogic()
    {
        // cast both the owner and target so we can access the Character / Monster properties
        Monster owner_cast = owner as Monster;
        Character target_cast = targetedObject as Character;
        owner_cast.Attack();
        // The Character is downed so we want to target a new Character
        if (target_cast.IsDowned)
        {
            foreach (Character target in targetList.Children)
            {
                // If the target is down, we do not want to target it so we continue to the next iteration in the loop.
                if (target.IsDowned || FindPath(target.Position, this.owner.Position).Count == 0)
                {
                    continue;
                }
                else
                {
                    targetedObject = target;
                }
            }
        }
    }

    /// <summary>
    /// Method that moves the AI in one unit of movementspeed towards the targetposition
    /// </summary>
    /// <param name="targetGridPosition">The targeted position in the Grid</param>
    /// <param name="elapsedGameTime">GameTime that elapsed since the last update</param>
    public void MoveToPosition(Vector2 targetGridPosition, float elapsedGameTime)
    {
        targetGridPosition = new Vector2((targetGridPosition.X + levelGrid.CellWidth / 2), (targetGridPosition.Y + levelGrid.CellHeight / 2));
        // Find the direction we have to go in
        direction = Vector2.Normalize(targetGridPosition - this.owner.Position);
        // AI moves to the direction with it's movementspeed and GameTime 
        this.owner.Position += direction * movementSpeed * elapsedGameTime;
    }

    /// <summary>
    /// Method that generates a waypoint list towards the targeted end position
    /// </summary>
    /// <param name="endPosition">The current position of the owner of this AI</param>
    /// <param name="startPosition">The current position of the target of this AI</param>
    /// <returns></returns>
    public List<Vector2> FindPath(Vector2 endPosition, Vector2 startPosition)
    {
        // Translate the given positions to grid positions (because we work in a grid with the library)
        Point gridStartPosition = new Point((int)startPosition.X / levelGrid.CellWidth, (int)startPosition.Y / levelGrid.CellHeight);
        Point gridEndPosition = new Point((int)endPosition.X / levelGrid.CellWidth, (int)(endPosition.Y / levelGrid.CellHeight));

        // With the library, generate a new Grid and block the tiles that are marked as solid or as ice
        Grid pathGrid = new Grid(levelGrid.Columns, levelGrid.Rows);
        for (int x = 0; x < levelGrid.Columns; x++)
        {
            for (int y = 0; y < levelGrid.Rows; y++)
            {
                Tile currenttile = levelGrid.Objects[x, y] as Tile;
                if (currenttile.IsSolid || currenttile.Type == TileType.Ice)
                {
                    pathGrid.BlockCell(new Position(x, y));
                }
            }
        }
        // Find the path using a method from the library
        Position[] pathArray = pathGrid.GetSmoothPath(new Position(gridStartPosition.X, gridStartPosition.Y), new Position(gridEndPosition.X, gridEndPosition.Y), MovementPatterns.LateralOnly);
        List<Vector2> pathList = new List<Vector2>();
        // translate the waypoints and add them to a usable pathlist
        for (int i = 0; i < pathArray.Length; i++)
        {
            Vector2 realWaypoint = new Vector2((pathArray[i].X * levelGrid.CellWidth), (pathArray[i].Y * levelGrid.CellHeight));
            pathList.Add(realWaypoint);
        }
        // The library puts the first position to be the start position, which would mess with our AI's logic and thus it is removed (unless the only position is its startposition)
        if (pathList.Count > 0)
        {
            pathList.RemoveAt(0);
        }
        return pathList;
    }

    /// <summary>
    /// LOS checker, checks if the Line of sight intersects with a rectangle of a possible target.
    /// </summary>
    /// <param name="sightRange">radius of the Circle of this line of sight</param>
    public void LineOfSightChecker(float sightRange)
    {
        List<AnimatedGameObject> targets = new List<AnimatedGameObject>();
        targets.Clear();
        Circle lineOfSight = new Circle(sightRange, owner.Origin);
        foreach(AnimatedGameObject obj in targetList.Children)
        {
            if((isMonster && obj is Character) || (!isMonster && obj is Monster))
                if (lineOfSight.CollidesWithRectangle(obj, owner))
                {
                    targets.Add(obj);
                }
        }
        if(randomTargeting)
            TargetRandomNearby(targets);
        else
            TargetClosest(targets);
    }

    /// <summary>
    ///  Method that sets the targetedobject to the closest target currently from the owner
    /// </summary>
    /// <param name="targets">list with the potential targets to attack</param>
    public void TargetClosest(List<AnimatedGameObject> targets)
    {
        float closestTargetDistance = 0;
        foreach (AnimatedGameObject target in targets)
        {
            Vector2 positionDifference = owner.Position - target.Position;
            float targetDistance = (float)Math.Sqrt(Math.Pow(positionDifference.X, 2) + Math.Pow(positionDifference.Y, 2));
            if (targetDistance > sightRange)
                continue;

            if (closestTargetDistance == 0 || targetDistance < closestTargetDistance)
            {
                // If a target is found we check if the target is reachable.
                if(FindPath(target.Position, owner.Position).Count > 0)
                {
                    closestTargetDistance = targetDistance;
                    targetedObject = target;
                }
            }
        }

    }
    Random random = new Random();
    int highestChance, chance;
    public void TargetRandomNearby(List<AnimatedGameObject> targets)
    {
        highestChance = 0;
        chance = 0;
        foreach(AnimatedGameObject target in targets)
        {
            if (isMonster)
            {
                if (((Character)target).Attributes.HP == 0)
                    continue;
            }

            else
            {
                if (((Monster)target).Attributes.HP == 0)
                    continue;
            }

            chance = random.Next(0, 51);
            if (chance > highestChance)
            {
                targetedObject = target;
                highestChance = chance;
            }
        }
    }

    /// <summary>
    /// AI properties
    /// </summary>
    public bool IsAttacking
    {
        get { return isAttacking; }
    }

    public bool RandomTargeting
    {
        get { return randomTargeting; }
        set { randomTargeting = value; }
    }
    public float AImovementSpeed
    {
        get { return movementSpeed; }
        set { movementSpeed = value; }
    }

    public Vector2 DirectionAI
    {
        get { return direction; }
    }

    public SpriteGameObject Owner
    {
        get { return owner; }
    }

    public AnimatedGameObject CurrentTarget
    {
        get { return targetedObject; }
        set { targetedObject = value; }
    }
}
