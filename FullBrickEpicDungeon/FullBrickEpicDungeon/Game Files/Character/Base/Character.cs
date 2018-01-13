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
    protected Timer reviveTimer, stepSoundTimer;
    protected Vector2 startPosition, movementSpeed, iceSpeed;
    protected int playerNumber, relativePlayerNumber;
    protected float hitCounter;
    protected Dictionary<Buttons, Buttons> xboxControls;
    protected Dictionary<string, string> characterSFX;
    protected bool playerControlled;
    protected Vector2 walkingdirection;
    protected BaseAI AI;
    SpriteGameObject healthBar;


    //Constructor: sets up the controls given to the constructor for each player (xbox or keyboard)
    protected Character(int playerNumber, Level currentLevel, bool xboxControlled, ClassType classType, string id = "") : base(0, id)
    {
        playerControlled = true;
        this.classType = classType;
        baseattributes = new BaseAttributes();
        inventory = new List<Equipment>();
        characterSFX = new Dictionary<string, string>();
        characterSFX.Add("ice_slide", "Assets/SFX/ice_slide");
        characterSFX.Add("ability_not_ready", "Assets/SFX/ability_not_ready");
        characterSFX.Add("switch_wrong", "Assets/SFX/switch_wrong");
        attributes = new BaseAttributes();
        reviveTimer = new Timer(10);
        reviveTimer.Reset();
        reviveTimer.IsPaused = true;
        stepSoundTimer = new Timer(0.5F)
        {
            IsExpired = true
        };
        this.velocity = Vector2.Zero;
        this.movementSpeed = new Vector2(4, 4);
        AI = new BaseAI(this, 200F, currentLevel, false, 1, 700);
        this.hitCounter = 0;
        this.playerNumber = playerNumber;
        relativePlayerNumber = playerNumber;
        this.xboxControlled = xboxControlled;
        this.iceSpeed = new Vector2(0, 0);
        healthBar = new SpriteGameObject(assetName = "Assets/Sprites/Shieldmaiden/HealthBar", layer = 5, id = "healthbar");

        if (playerNumber == 1)
        {
            if (xboxControlled) //opgeslagen controls staan in de txt bestandjes
                xboxControls = GameEnvironment.SettingsHelper.GenerateXboxControls("Assets/Controls/XboxControls/player1Xbox.txt");
            else
                keyboardControls = GameEnvironment.SettingsHelper.GenerateKeyboardControls("Assets/Controls/player1controls.txt");
        }
        else if (playerNumber == 2)
        {
            if (xboxControlled)
                xboxControls = GameEnvironment.SettingsHelper.GenerateXboxControls("Assets/Controls/XboxControls/player2Xbox.txt");
            else
                keyboardControls = GameEnvironment.SettingsHelper.GenerateKeyboardControls("Assets/Controls/player2controls.txt");
        }
        else if (playerNumber == 3)
        {
            if (xboxControlled)
                xboxControls = GameEnvironment.SettingsHelper.GenerateXboxControls("Assets/Controls/XboxControls/player3Xbox.txt");
            else
                playerControlled = false;
        }
        else if (playerNumber == 4)
        {
            if (xboxControlled)
                xboxControls = GameEnvironment.SettingsHelper.GenerateXboxControls("Assets/Controls/XboxControls/player4Xbox.txt");
            else if (this.xboxControlled)
                playerControlled = false;
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
        if (!IsDowned)
        {
            stepSoundTimer.Update(gameTime);
            base.Update(gameTime);
            this.weapon.Update(gameTime);
            UpdateHealthBar();
            if (!playerControlled)
            {
                Vector2 previousPosition = this.position;
                AI.Update(gameTime);
                if (!(previousPosition == this.position) && stepSoundTimer.IsExpired)
                {
                    PlaySFX("walk");
                    stepSoundTimer.Reset();
                }
                PlayAnimationDirection(position - previousPosition);
            }
            
            if (hitCounter >= 0)
            {
                Visible = !Visible;
                hitCounter -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
                Visible = true;
            IsOnIceChecker();
        }
        else
        {
            reviveTimer.Update(gameTime);
            if (reviveTimer.IsExpired)
            {
                this.Reset();
                // when the revivetimer expires, the character dies :( sadly he will lose some of his gold after dying (currently 25% might be higher in later versions)
                this.attributes.Gold = this.attributes.Gold - (this.attributes.Gold / 4);
                reviveTimer.Reset();
                reviveTimer.IsPaused = true;
            }
            reviveTimer.IsPaused = false;
        }
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        healthBar.Draw(gameTime, spriteBatch);
        
        base.Draw(gameTime, spriteBatch);
    }

    public void UpdateHealthBar()
    {

        int maxhealth = baseattributes.HP;
        int health = attributes.HP;
        int displayedhealth = (health / maxhealth) * 60;
        Vector2 healthbarposition = new Vector2(position.X - (Width / 2), position.Y - (Height/2 + 5));
        healthBar.Position = healthbarposition;

        // not sure how to make the healthbar scale down to account for damage taken.
        //Rectangle pppppppp = new Rectangle((int)healthbarposition.X, (int)healthbarposition.Y, displayedhealth, healthBar.Height);

        
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
    public void InteractCollisionChecker()
    {
        GameObjectList charList = GameWorld.Find("playerLIST") as GameObjectList;
        foreach (Character c in charList.Children)
        {
            if(this.CollidesWith(c) && c.IsDowned)
            {

            }
        }

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
            takendamage = 1;
        }
        this.attributes.HP -= takendamage;
        if (this.attributes.HP < 0)
        {
            this.attributes.HP = 0;
        }
    }
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

    public bool XBOXcontrolled
    {
        get { return xboxControlled; }
        set { xboxControlled = value; }
    }
    // returns the facing direction of the character
}