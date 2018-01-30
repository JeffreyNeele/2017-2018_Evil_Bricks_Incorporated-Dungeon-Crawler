using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;


abstract partial class Character : AnimatedGameObject
{
    // variables for Timers
    private Timer deathTimer, reviveTimer, stepSoundTimer, hitTimer, switchCharacterTimer;
    // Dictionary for SFX paths
    protected Dictionary<string, string> characterSFX;
    // attributes for the character
    protected BaseAttributes attributes, baseattributes;
    protected Weapon weapon;
    // Vector2s for startposition, movementspeed and icespeed
    protected Vector2 startPosition, movementSpeed, iceSpeed;
    //controllernumber is the number of the XboxController, controlsNumber is 0 to 5, 0 and 1, keyboard. 2-5, xbox.
    protected int playerNumber, controllerNumber, controlsnumber;
    protected bool playerControlled = true;
    protected Vector2 walkingdirection, previousWalkingDirection, previousPosition = new Vector2(0, 0);
    protected BaseAI AI;
    protected Healthbar healthbar;
    protected KeyItem characterKey = null;
    protected string playerColor;
    //Constructor: sets up the controls given to the constructor for each player (xbox or keyboard)
    protected Character(int playerNumber, int controlsNumber, Level currentLevel, string id = "") : base(0, id)
    {
        attributes = new BaseAttributes();
        baseattributes = new BaseAttributes();
        // load paths into the characterSFX dictionary
        characterSFX = new Dictionary<string, string>
        {
            { "ice_slide", "Assets/SFX/ice_slide" },
            { "ability_not_ready", "Assets/SFX/ability_not_ready" },
            { "switch_wrong", "Assets/SFX/switch_wrong" }
        };
        // make a new healthbar
        healthbar = new Healthbar(this);
        // initialize all the timers
        deathTimer = new Timer(7)
        {
            IsPaused = false
        };
        stepSoundTimer = new Timer(0.5F)
        {
            IsExpired = true
        };
        reviveTimer = new Timer(3);
        hitTimer = new Timer(0.5f)
        {
            IsExpired = true
        };
        switchCharacterTimer = new Timer(0.1f)
        {
            IsExpired = true
        };
        // Define speeds on ice and land
        this.iceSpeed = new Vector2(0, 0);
        this.movementSpeed = new Vector2(4, 4);
        // Make a new AI
        AI = new BaseAI(this, 200F, currentLevel, false, 600, 1);
        this.playerNumber = playerNumber;
        controllerNumber = playerNumber;
        ControlsInitializer(controlsNumber);
        controlsnumber = controlsNumber;
        // Generates controls for the keyboard if the character is not controlled by xbox, keyboard is only used for 2 players so as a safeguard player 3 and 4 will become AI if this is called them.

    }

    private void ControlsInitializer(int controlsNumber)
    {
        if (controlsNumber == 0)
        {
            xboxControlled = false;
            keyboardControls = GameEnvironment.SettingsHelper.GenerateKeyboardControls("Assets/Controls/player1controls.txt");
        }
        else if (controlsNumber == 1)
        {
            xboxControlled = false;
            keyboardControls = GameEnvironment.SettingsHelper.GenerateKeyboardControls("Assets/Controls/player2controls.txt");
        }
        else if (controlsNumber >= 2 && controlsNumber <= 5)
        {
            controllerNumber = controlsNumber - 1;
            xboxControlled = true;
        }
        else
        {
            playerControlled = false;
        }
    }


    /// <summary>
    /// Calculates the movement vector for a character (this makes sure that going to a lateral direction is the same speed as going to a diagonal direction)
    /// </summary>
    /// <param name="movementSpeed">How fast the character moves</param>
    /// <param name="angle">What angle/direction the character is moving to</param>
    /// <returns></returns>
    public Vector2 MovementVector(Vector2 movementSpeed, float angle)
    {
        float adjacent = movementSpeed.X;
        float opposite = movementSpeed.Y;

        float hypotenuse = (float)Math.Sqrt(adjacent * adjacent + opposite * opposite);
        adjacent = (float)Math.Cos(angle * (Math.PI / 180)) * hypotenuse;
        opposite = (float)Math.Sin(angle * (Math.PI / 180)) * hypotenuse;

        return new Vector2(adjacent, opposite);
    }

    /// <summary>
    /// Updates the character
    /// </summary>
    /// <param name="gameTime">the current game time</param>
    public override void Update(GameTime gameTime)
    {
        // if the character is not downed, it gets updated according to normal rules, so this code block is entered
        if (!IsDowned)
        {
            // Timer updates
            reviveTimer.Update(gameTime);
            stepSoundTimer.Update(gameTime);
            switchCharacterTimer.Update(gameTime);
            // Updates the weapon and healthbar
            this.weapon.Update(gameTime);
            healthbar.Update(gameTime);
            // Update for if the player is AI
            if (!playerControlled)
            {
                if(this.position != previousPosition)
                    previousPosition = this.position;
                AI.Update(gameTime);
                if (!(previousPosition == this.position) && stepSoundTimer.IsExpired)
                {
                    PlaySFX("walk");
                    stepSoundTimer.Reset();
                }
                // Play animations for the AI
                PlayAnimationDirection(position - previousPosition);
                if(weapon.IsBaseAA)
                    weapon.SwordDirectionCheckerManager(AI.DirectionAI);
                else if(weapon.IsShieldAA)
                    weapon.ShieldDirectionCheckerManager(AI.DirectionAI);
            }
            
            //When a character takes damage, let the character blink as an indication.
            if (!hitTimer.IsExpired)
            {
                Visible = !Visible;
                hitTimer.Update(gameTime);
            }
            else
                Visible = true;

            // Checks if the character is on ice
            IsOnIceChecker();
            base.Update(gameTime);
        }
        else
        {
            PlayAnimation("die");
            // Updates the death timer and if it expires, the character gets reset and loses some gold
            deathTimer.Update(gameTime);
            if (deathTimer.IsExpired)
            {
                this.Reset();
                this.attributes.Gold = this.attributes.Gold - (this.attributes.Gold / 4);
                deathTimer.Reset();
            }
        }
    }

    /// <summary>
    /// Draws things attached to the character
    /// </summary>
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        // draw the healthbar if the characterr is not down
        if(this.attributes.HP > 0)
        {
            healthbar.Draw(gameTime, spriteBatch);
            if (weapon.IsAttacking)
            {
                weapon.Draw(gameTime, spriteBatch);
            }
        }
        // draw the weapon if an attack animation is being played
        base.Draw(gameTime, spriteBatch);
    }

    /// <summary>
    /// Resets the character
    /// </summary>
    public override void Reset()
    {
        this.weapon.AbilityMain.IsOnCooldown = false;
        this.attributes.HP = this.baseattributes.HP;
        this.position = startPosition;
    }


   /// <summary>
   /// Transfers gold to another character
   /// </summary>
   /// <param name="amount">amount of gold to be transferred</param>
   /// <param name="target">the character that will receve this gold</param>
    public void TransferGold(int amount, Character target)
    {
        // This method is also used for if normal gold is added with no target, so this code block is for that
        if (target == this)
        {
            this.attributes.Gold += amount;
        }
        else
        {
            this.attributes.Gold -= amount;
            target.attributes.Gold += amount;
        }
    }

    /// <summary>
    /// Collision checker that checks for collisions with interactive objects
    /// </summary>
    private void InteractCollisionChecker()
    {
        // Code that handles a player trying to revive his friend
        GameObjectList charList = GameWorld.Find("playerLIST") as GameObjectList;
        foreach (Character c in charList.Children)
        {
            if(this.CollidesWith(c) && c.IsDowned)
            {
                if (reviveTimer.IsPaused)
                {
                    reviveTimer.Reset();
                }
                else if (reviveTimer.IsExpired)
                {
                    c.attributes.HP = c.baseattributes.HP;
                    reviveTimer.IsPaused = true;
                }
            }
        }

        GameObjectList objectList = GameWorld.Find("objectLIST") as GameObjectList;
        // If a character collides with an interactive object, set the target character to this instance and tell the interactive object that it is currently interacting
        foreach (var intObj in objectList.Children)
        {
            if (intObj is InteractiveObject)
            {
                InteractiveObject intObj_cast = intObj as InteractiveObject;
                if (intObj_cast.CollidesWith(this))
                {
                    // if the intobj is a key set the carried key to that key
                    if (intObj is KeyItem)
                    {
                        if (!((KeyItem)intObj).keyOwned && characterKey == null && (((KeyItem)intObj).Objectnumber == playerNumber || ((KeyItem)intObj).Objectnumber == 5))
                        {
                            intObj_cast.TargetCharacter = this;
                            intObj_cast.IsInteracting = true;
                            CarriedKey = (KeyItem)intObj;
                        }
                    }
                    if(intObj is Handle)
                    {
                        intObj_cast.TargetCharacter = this;
                        intObj_cast.IsInteracting = true;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Checks whether a character collides with a tile
    /// </summary>
    /// <returns>whether the position the character is going is valid</returns>
    public bool SolidCollisionChecker()
    {
        GameObjectGrid Field = GameWorld.Find("TileField") as GameObjectGrid;

        foreach (Tile tile in Field.Objects)
        {
            if (tile.IsSolid && IsometricBoundingBox.Intersects(tile.BoundingBox))
            {
                return false;
            }
            if (tile is VerticalDoor)
                if(IsometricBoundingBox.Intersects(((VerticalDoor)tile).BoundingBox2))
                return false;
        }
        return true;
    }

    /// <summary>
    /// Checker that checks whether a character is on ice
    /// </summary>
    public bool IsOnIceChecker()
    {
        GameObjectGrid Field = GameWorld.Find("TileField") as GameObjectGrid;
        // Bounding box for only the feet
        Rectangle feetBoundingBox = new Rectangle((int)(this.BoundingBox.X + 0.33 * Width),
            (int)(this.BoundingBox.Y + 0.9 * Height), (int)(this.Width / 3), (Height / 10));
        foreach (Tile tile in Field.Objects)
        {
            if (tile.Type == TileType.Ice && tile.BoundingBox.Intersects(feetBoundingBox))
            {
                return true;
            }
        }
        blockinput = false;
        return false;
    }

    /// <summary>
    /// Makes the character take damage
    /// </summary>
    /// <param name="damage">the amount of damage to take</param>
    public void TakeDamage(int damage)
    {
        // damage is reduced by this characters armour
        int takendamage = (damage - (int)(0.3F * this.attributes.Armour));
        if (takendamage < 5)
        {
            takendamage = 1;
        }
        this.attributes.HP -= takendamage;

        if (this.attributes.HP < 0)
        {
            this.attributes.HP = 0;
            hitTimer.IsExpired = true;
        }
        else
            hitTimer.Reset();
    }
    /// <summary>
    /// Method that plays a sound effect from the SFX dictionary
    /// </summary>
    /// <param name="sfx">name of the SFX</param>
    public void PlaySFX(string sfx)
    {
        if(FullBrickEpicDungeon.DungeonCrawler.SFX)
            GameEnvironment.AssetManager.PlaySound(characterSFX[sfx]);
    }

    // returns if the character has gone into the "downed" state
    public bool IsDowned
    {
        get { return this.attributes.HP == 0; }
    }

    /// <summary>
    /// Character properties
    /// </summary>
    // returns the startPosition of the character. Will be newly set when entering a level
    public Vector2 StartPosition
    {
        get { return startPosition; }
        set { startPosition = value; }
    }
    public Vector2 MovementSpeed
    {
        get { return movementSpeed; }
        set { movementSpeed = value; }
    }
    // returns the weapon of the character
    public Weapon CurrentWeapon
    {
        get { return weapon; }
        set { weapon = value; }
    }
    /// <summary>
    /// Isometric bounding box used for collision and tiles
    /// </summary>
    public Rectangle IsometricBoundingBox
    {
        get { return new Rectangle((int)this.BoundingBox.X, (int)(this.BoundingBox.Y + 0.75 * Height), this.Width, (int)(this.Height / 4)); }
    }
    // returns the attributes of the character
    public BaseAttributes Attributes
    {
        get { return attributes; }
        set { attributes = value; }
    }

    // property for saying if a character is AI or player controlled
    public bool PlayerControlled
    {
        get { return playerControlled; }
        set { playerControlled = value; }
    }

    public int PlayerNumber
    {
        get { return playerNumber; }
    }

    public string PlayerColor
    {
        get { return playerColor; }
    }

    public bool XBOXcontrolled
    {
        get { return xboxControlled; }
        set { xboxControlled = value; }
    }

    public KeyItem CarriedKey
    {
        get { return characterKey; }
        set { characterKey = value; }
    }
}