using System.Collections.Generic;
using System;

abstract partial class Character : AnimatedGameObject
{
    protected int hitpoints, armour, gold;
    Weapon weapon;
    List<Equipment> inventory;
    protected Character(Weapon weapon)
    {
        this.weapon = weapon;
        inventory = new List<Equipment>();
    }

    // Transfers money to another character
    public void TransferMoney(int amount, Character target)
    {
        if (target == this)
        {
            this.gold += amount;
        }
        else
        {
            this.gold -= amount;
            target.gold += amount;
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
                throw new ArgumentOutOfRangeException("No such item was found in this characters inventory!");
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


    public void PassiveAbility()
    {

    }



}