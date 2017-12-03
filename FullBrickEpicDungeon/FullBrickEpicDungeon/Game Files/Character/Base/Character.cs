using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;

abstract partial class Character : AnimatedGameObject
{
    protected BaseAttributes attributes;
    protected Weapon weapon;
    protected List<Equipment> inventory;
    protected Character(Weapon weapon, string type)
    {
        this.weapon = weapon;
        inventory = new List<Equipment>();
        attributes = new BaseAttributes(type);
    }

    public override void Update(GameTime gameTime)
    {
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
        foreach(InteractiveObject intObj in objectList.Children)
        {
            if (intObj.CollidesWith(this))
            {
                intObj.IsInteracting = true;
                intObj.TargetCharacter = this;
            }
        }
    }
    public void ChangeWeapon(Weapon weapon)
    {
        this.weapon = weapon;
        // Drop the weapon on the floor after it changes
    }
 
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

    public bool OwnsItem(Equipment item)
    {
        return inventory.Contains(item);
    }

    public void TakeDamage(int damage)
    {
        this.attributes.HP -= this.attributes.HP - (damage - (int)(0.3F * this.attributes.Armour));
    }

    public BaseAttributes Attributes
    {
        get { return attributes; }
        set { attributes = value; }
    }

}