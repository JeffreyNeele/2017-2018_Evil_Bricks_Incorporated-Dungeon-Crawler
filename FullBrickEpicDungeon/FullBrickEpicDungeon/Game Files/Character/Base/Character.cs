using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;

abstract partial class Character : AnimatedGameObject
{
    protected BaseAttributes attributes;
    protected Weapon weapon;
    protected List<Equipment> inventory;
    string type;
    bool playerControlled;
    protected Character(Weapon weapon, string type, string id = "") : base(0, id)
    {
        this.weapon = weapon;
        inventory = new List<Equipment>();
        attributes = new BaseAttributes(type);
    }

    public override void Update(GameTime gameTime)
    {
        CollisionChecker();
        base.Update(gameTime);
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
    public void ChangeWeapon(Weapon weapon)
    {
        this.weapon = weapon;
        // Drop the weapon on the floor after it changes
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
        this.attributes.HP -= this.attributes.HP - (damage - (int)(0.3F * this.attributes.Armour));
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