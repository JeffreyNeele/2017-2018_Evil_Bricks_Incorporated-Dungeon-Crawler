using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

abstract partial class Character : AnimatedGameObject
{
    //baseattributes contains the standard base stats and should not be changed, the values in attributes may be changes are used during the remainder of the level
    protected ClassType classType;
    protected BaseAttributes attributes, baseattributes;
    protected Weapon weapon;
    protected List<Equipment> inventory;
    protected Timer reviveTimer;
    protected Vector2 startPosition, movementSpeed, iceSpeed;
    protected int playerNumber;
    protected float hitCounter;
    protected Dictionary<Buttons, Buttons> xboxControls;
    protected bool xboxControlled = false, playerControlled;
    protected Vector2 walkingdirection;
    protected BaseAI AI;

    //Constructor: sets up the controls given to the constructor for each player (xbox or keyboard)
    protected Character(int playerNumber, Level currentLevel, bool xboxControlled, ClassType classType, string id = "") : base(0, id)
    {
        playerControlled = true;
        this.classType = classType;
        baseattributes = new BaseAttributes();
        inventory = new List<Equipment>();
        attributes = new BaseAttributes();
        reviveTimer = new Timer(10);
        this.velocity = Vector2.Zero;
        this.movementSpeed = new Vector2(4, 4);
        AI = new BaseAI(this, 200F, currentLevel, false);
        this.hitCounter = 0;
        this.playerNumber = playerNumber;
        this.xboxControlled = xboxControlled;
        this.iceSpeed = new Vector2(0, 0);
        this.keyboardControlled = true;

        if (playerNumber == 1)
        {
            if (this.keyboardControlled) //opgeslagen controls staan in de txt bestandjes
                keyboardControls = GameEnvironment.SettingsHelper.GenerateKeyboardControls("Assets/KeyboardControls/player1controls.txt");
            if (this.xboxControlled)
                xboxControls = GameEnvironment.SettingsHelper.GenerateXboxControls("Assets/KeyboardControls/XboxControls/player1Xbox.txt");
        }
        else if (playerNumber == 2)
        {
            if (this.keyboardControlled)
                keyboardControls = GameEnvironment.SettingsHelper.GenerateKeyboardControls("Assets/KeyboardControls/player2controls.txt");
            if (this.xboxControlled)
                xboxControls = GameEnvironment.SettingsHelper.GenerateXboxControls("Assets/KeyboardControls/XboxControls/player2Xbox.txt");
        }
        else if (playerNumber == 3)
        {
            if (this.keyboardControlled)
                throw new ArgumentOutOfRangeException("Only Player 1 and 2 can play with a keyboard");
            if (this.xboxControlled)
                xboxControls = GameEnvironment.SettingsHelper.GenerateXboxControls("Assets/KeyboardControls/XboxControls/player3Xbox.txt");
        }
        else if (playerNumber == 4)
        {
            if (this.keyboardControlled)
                throw new ArgumentOutOfRangeException("Only Player 1 and 2 can play with a keyboard");
            if (this.xboxControlled)
                xboxControls = GameEnvironment.SettingsHelper.GenerateXboxControls("Assets/KeyboardControls/XboxControls/player4Xbox.txt");
        }
    }



   

    // Calculates the new movementVector for a character for keyboard
    public Vector2 MovementVector(Vector2 movementSpeed, float angle)
    {
        float adjacent = movementSpeed.X;
        float opposite = movementSpeed.Y;

        float hypotenuse = (float)Math.Sqrt(adjacent * adjacent + opposite * opposite);
        adjacent = (float)Math.Cos(angle * (Math.PI / 180)) * hypotenuse;
        opposite = (float)Math.Sin(angle * (Math.PI / 180)) * hypotenuse;

        return new Vector2(adjacent, opposite);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        this.weapon.Update(gameTime);
        IsOnIceChecker();
        if (IsDowned)
        {
            reviveTimer.IsPaused = false;
            if (reviveTimer.IsExpired)
            {
                this.Reset();
                // when the revivetimer expires, the character dies :( sadly he will lose some of his gold after dying (currently 25% might be higher in later versions)
                this.attributes.Gold = this.attributes.Gold - (this.attributes.Gold / 4);
            }
        }

        if (hitCounter >= 0)
        {
            Visible = !Visible;
            hitCounter -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        else
            Visible = true;
        if (!playerControlled)
        {
            AI.Update(gameTime);
        }
    }


   
    
    public override void Reset()
    {
        this.attributes.HP = this.baseattributes.HP;
        this.position = startPosition;
    }


    // Transfers money to another character
    public void TransferGold(int amount, Character target)
    {
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

    //Checks if the character collides with interactive objects
    public void ObjectCollisionChecker()
    {
        GameObjectList objectList = GameWorld.Find("objectLIST") as GameObjectList;
        // If a character collides with an interactive object, set the target character to this instance and tell the interactive object that it is currently interacting
        foreach (InteractiveObject intObj in objectList.Children)
        {
            if (intObj.CollidesWith(this))
            {
                intObj.TargetCharacter = this;
                intObj.IsInteracting = true;
            }
        }
    }

    //Dikke collision met muren/andere solid objects moet ervoor zorgen dat de player niet verder kan bewegen.
    public bool SolidCollisionChecker()
    {
        GameObjectGrid Field = GameWorld.Find("TileField") as GameObjectGrid;
        Rectangle quarterBoundingBox = new Rectangle((int)this.BoundingBox.X, (int)(this.BoundingBox.Y + 0.75 * Height), this.Width, (int)(this.Height / 4));
        foreach (Tile tile in Field.Objects)
        {
            if (tile.IsSolid && quarterBoundingBox.Intersects(tile.BoundingBox))
            {
                return false;
            }
        }
        return true;
    }

    public void IsOnIceChecker()
    {
        GameObjectGrid Field = GameWorld.Find("TileField") as GameObjectGrid;
        Rectangle feetBoundingBox = new Rectangle((int)(this.BoundingBox.X + 0.33 * Width),
            (int)(this.BoundingBox.Y + 0.9 * Height), (int)(this.Width / 3), (Height / 10));
        foreach (Tile tile in Field.Objects)
        {
            if (tile.IsIce && tile.BoundingBox.Intersects(feetBoundingBox))
            {
                isOnIce = true;
                return;
            }
        }
        isOnIce = false;
        blockinput = false;
    }

    // Changes the weapon of a Character and drops the weapon on the ground
    public void ChangeWeapon(Weapon newweapon)
    {
        DroppedItem droppedWeapon = new DroppedItem(this.weapon, "DROPPED" + weapon.Id);
        this.weapon = newweapon;
    }

    // Changes items in the characters inventory, also allows to remove it
    public void ChangeItems(Equipment item, bool remove = false)
    {
        if (item == null)
        {
            return;
        }

        if (remove)
        {
            inventory.Remove(item);
        }
        else
        {
            inventory.Add(item);
        }

    }

    // Checks if a character owns an item (only for equipment)
    public bool OwnsItem(Equipment item)
    {
        return inventory.Contains(item);
    }


    public void TakeDamage(int damage)
    {
        int totalitemdefense = 0;
        foreach (Equipment item in inventory)
        {
            totalitemdefense += item.Armour;
        }
        int takendamage = (damage - (int)(0.3F * this.attributes.Armour + totalitemdefense));
        if (takendamage < 5)
        {
            takendamage = 0;
        }
        this.attributes.HP -= takendamage;
        if (this.attributes.HP < 0)
        {
            this.attributes.HP = 0;
        }
    }

    

    public void GenerateControls(int playerNUMBER)
    {
        if (playerNUMBER == 1)
        {
            if (this.keyboardControlled) //opgeslagen controls staan in de txt bestandjes
                keyboardControls = GameEnvironment.SettingsHelper.GenerateKeyboardControls("Assets/KeyboardControls/player1controls.txt");
            if (this.xboxControlled)
                xboxControls = GameEnvironment.SettingsHelper.GenerateXboxControls("Assets/KeyboardControls/XboxControls/player1Xbox.txt");
        }
        else if (playerNUMBER == 2)
        {
            if (this.keyboardControlled)
                keyboardControls = GameEnvironment.SettingsHelper.GenerateKeyboardControls("Assets/KeyboardControls/player2controls.txt");
        }
    }

    // returns if the character has gone into the "downed" state
    public bool IsDowned
    {
        get { return this.attributes.HP == 0; }
    }

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
    // returns the attributes of the character
    public BaseAttributes Attributes
    {
        get { return attributes; }
        set { attributes = value; }
    }

    public ClassType Type
    {
        get { return classType; }
    }

    public bool PlayerControlled
    {
        get { return playerControlled; }
        set { playerControlled = value; }
    }

    public int PlayerNumber
    {
        get { return playerNumber; }
    }
    // returns the facing direction of the character
}