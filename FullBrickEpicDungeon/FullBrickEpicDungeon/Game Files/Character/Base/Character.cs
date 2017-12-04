using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

abstract partial class Character : AnimatedGameObject
{
    //baseattributes contains the standard base stats and should not be changed, the values in attributes may be changes are used during the remainder of the level
    protected BaseAttributes attributes, baseattributes;
    protected Weapon weapon;
    protected List<Equipment> inventory;
    protected Timer reviveTimer;
    string type;
    bool playerControlled;
    Vector2 startPosition;
    protected Character(string type, string id = "") : base(0, id)
    {
        baseattributes = new BaseAttributes(type + " base");
        inventory = new List<Equipment>();
        attributes = new BaseAttributes(type);
        reviveTimer = new Timer(10);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        CollisionChecker();
        if (IsDowned)
        {
            reviveTimer.IsPaused = false;
            if (reviveTimer.IsExpired)
            {
                this.Die();
            }
        }
    }

    //Method for character input (both xbox controller and keyboard), for now dummy keys for 1 controller are inserted, but the idea should be clear
    //TO DO: a way to distinguish characters / players from each other.
    public override void HandleInput(InputHelper inputHelper)
    {
        if(!IsDowned)
        {
            base.HandleInput(inputHelper);

            //Input keys for basic AA and abilities
            if (inputHelper.KeyPressed(Keys.Q))
                this.weapon.Attack();
            if (inputHelper.KeyPressed(Keys.E))
                this.weapon.UseMainAbility();
            if (inputHelper.KeyPressed(Keys.R))
                this.weapon.UseSpecialAbility();

            //Input keys for character movement, nog te bepalen of movementspeed vector2 of int is
            /*
            if (inputHelper.KeyPressed(Keys.W))

            if (inputHelper.KeyPressed(Keys.S))

            if (inputHelper.KeyPressed(Keys.A))

            if (inputHelper.KeyPressed(Keys.D))
            */
        }

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

    // Checks if the Character collides with monsters, objects or tiles
    public void CollisionChecker()
    {
        GameObjectList monsterList = GameWorld.Find("monsterLIST") as GameObjectList;
        GameObjectList objectList = GameWorld.Find("objectLIST") as GameObjectList;
        // TODO: Add Tilefield collision with walls puzzles etc, (not doable atm as it isn't programmed as of writing this)
        foreach(Monster monsterobj in monsterList.Children)
        {
            if (monsterobj.CollidesWith(this))
            {
                this.TakeDamage(monsterobj.Attributes.Attack);
            }
        }

        // If a character collides with an interactive object, set the target character to this instance and tell the interactive object that it is currently interacting
        foreach(InteractiveObject intObj in objectList.Children)
        {
            if (intObj.CollidesWith(this))
            {
                intObj.IsInteracting = true;
                intObj.TargetCharacter = this;
            }
        }
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
                throw new ArgumentOutOfRangeException("No such item was found in " + this.attributes.Type + "'s inventory!");
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
        this.attributes.HP -= (damage - (int)(0.3F * this.attributes.Armour));
        if (this.attributes.HP < 0)
        {
            this.attributes.HP = 0;
        }
    }

    //Will need additional information from subclasses
    //Method that respawns a character when the reviveTimer is expired
    public void Die()
    {
        
        this.position = StartPosition;
    }

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

    // returns the weapon of the character
    public Weapon CurrentWeapon
    {
        get { return weapon; }
    }
    // returns the attributes of the character
    public BaseAttributes Attributes
    {
        get { return attributes; }
        set { attributes = value; }
    }

}