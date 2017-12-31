using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

abstract class Character : AnimatedGameObject
{
    //baseattributes contains the standard base stats and should not be changed, the values in attributes may be changes are used during the remainder of the level
    protected ClassType classType;
    protected BaseAttributes attributes, baseattributes;
    protected Weapon weapon;
    protected List<Equipment> inventory;
    protected Timer reviveTimer;
    protected Vector2 startPosition, movementSpeed;
    protected Dictionary<Keys, Keys> keyboardControls;
    protected bool keyboardControlled;
    protected int playerNumber;
    protected Character(int playerNumber, ClassType classType, string baseAsset, string id = "", bool keyboardControlled = true) : base(0, id)
    {
        this.keyboardControlled = keyboardControlled;
        this.playerNumber = playerNumber;
        this.classType = classType;
        baseattributes = new BaseAttributes();
        inventory = new List<Equipment>();
        attributes = new BaseAttributes();
        reviveTimer = new Timer(10);
        this.velocity = Vector2.Zero;
        this.movementSpeed = new Vector2(3, 3);

        if (this.keyboardControlled)
        {
            if (playerNumber == 1)
            {
                keyboardControls = GameEnvironment.SettingsHelper.GenerateKeyboardControls("Assets/KeyboardControls/player1controls.txt");
            }
            else if (playerNumber == 2)
            {
                keyboardControls = GameEnvironment.SettingsHelper.GenerateKeyboardControls("Assets/KeyboardControls/player2controls.txt");
            }
        }

    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        MonsterCollisionChecker();
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
    }


    //Method for character input (both xbox controller and keyboard), for now dummy keys for 1 controller are inserted, but the idea should be clear
    //TO DO: a way to distinguish characters / players from each other.
    public override void HandleInput(InputHelper inputHelper)
    {
        if (!IsDowned)
        {
            Vector2 previousPosition = this.position;
            //Input keys for basic AA and abilities
            if (inputHelper.KeyPressed(keyboardControls[Keys.Q]))
                this.weapon.Attack(GameWorld.Find("monsterLIST") as GameObjectList);
            if (inputHelper.KeyPressed(keyboardControls[Keys.R]))
                this.weapon.UseMainAbility(GameWorld.Find("monsterLIST") as GameObjectList);
            if (inputHelper.KeyPressed(keyboardControls[Keys.T]))
                this.weapon.UseSpecialAbility(GameWorld.Find("monsterLIST") as GameObjectList);

            if (inputHelper.IsKeyDown(keyboardControls[Keys.W]) || inputHelper.IsKeyDown(keyboardControls[Keys.S]))
            {

                if (inputHelper.IsKeyDown(keyboardControls[Keys.W]))
                {
                    if (inputHelper.IsKeyDown(keyboardControls[Keys.A]))
                    {
                        this.position += MovementVector(this.movementSpeed, 225);
                        this.PlayAnimation("leftcycle");
                    }
                    else if (inputHelper.IsKeyDown(keyboardControls[Keys.D]))
                    {
                        this.position += MovementVector(this.movementSpeed, 315);
                        this.PlayAnimation("rightcycle");
                    }
                    else
                    {
                        this.position += MovementVector(this.movementSpeed, 270);
                        this.PlayAnimation("backcycle");
                    }

                }

                else if (inputHelper.IsKeyDown(keyboardControls[Keys.S]))
                {
                    if (inputHelper.IsKeyDown(keyboardControls[Keys.A]))
                    {
                        this.position += MovementVector(this.movementSpeed, 135);
                        this.PlayAnimation("leftcycle");
                    }
                    else if (inputHelper.IsKeyDown(keyboardControls[Keys.D]))
                    {
                        this.position += MovementVector(this.movementSpeed, 45);
                        this.PlayAnimation("rightcycle");
                    }
                    else
                    {
                        this.position += MovementVector(this.movementSpeed, 90);
                        this.PlayAnimation("frontcycle");
                    }
                }
            }

            else if (inputHelper.IsKeyDown(keyboardControls[Keys.A]))
            {
                this.position += MovementVector(this.movementSpeed, 180);
                this.PlayAnimation("leftcycle");
            }

            else if (inputHelper.IsKeyDown(keyboardControls[Keys.D]))
            {
                this.position += MovementVector(this.movementSpeed, 0);
                this.PlayAnimation("rightcycle");
            }

            else
            {
                PlayAnimation("idle");
            }

            if (inputHelper.IsKeyDown(keyboardControls[Keys.E])) //Interact key
            {
                ObjectCollisionChecker();
            }

            if (!SolidCollisionChecker())
            {
                this.position = previousPosition;
            }
            base.HandleInput(inputHelper);
        }
    }
    public override void Reset()
    {
        this.attributes.HP = this.baseattributes.HP;
        this.position = StartPosition;
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

    // Checks if the Character collides with monsters
    public void MonsterCollisionChecker()
    {
        GameObjectList monsterList = GameWorld.Find("monsterLIST") as GameObjectList;
        // TODO: Add Tilefield collision with walls puzzles etc, (not doable atm as it isn't programmed as of writing this)
        foreach (Monster monsterobj in monsterList.Children)
        {
            if (monsterobj.CollidesWith(this))
            {
                this.TakeDamage(monsterobj.Attributes.Attack);
            }
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
            if (tile.isSolid && quarterBoundingBox.Intersects(tile.BoundingBox))
            {
                return false;
            }
        }
        return true;
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
        if(item == null)
        {
            return;
        }

        if (remove)
        {
            try
            {
                inventory.Remove(item);
            }
            catch
            {
                throw new ArgumentOutOfRangeException("No such item was found in " + this.classType + "'s inventory!");
            }
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
        foreach(Equipment item in inventory)
        {
            totalitemdefense += item.Armour;
        }
        this.attributes.HP -= (damage - (int)(0.3F * (this.attributes.Armour + totalitemdefense)));
        if (this.attributes.HP < 0)
        {
            this.attributes.HP = 0;
        }
    }

    // Calculates the new movementVector for a character (movementVector outcome may differ between xbox controllers and keyboard controllers)
    public Vector2 MovementVector(Vector2 movementSpeed, float angle)
    {
        float adjacent = movementSpeed.X;
        float opposite = movementSpeed.Y;

        float hypotenuse = (float)Math.Sqrt(adjacent * adjacent + opposite * opposite);
        adjacent = (float)Math.Cos(angle * (Math.PI / 180)) * hypotenuse;
        opposite = (float)Math.Sin(angle * (Math.PI / 180)) * hypotenuse;

        return new Vector2(adjacent, opposite);
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

    public Dictionary<Keys, Keys> KeyboardControlScheme
    {
        get { return keyboardControls; }
        set { keyboardControls = value; }
    }
    public ClassType Type
    {
        get { return classType; }
    }

    public int PlayerNumber
    {
        get { return playerNumber; }
    }
    // returns the facing direction of the character
}