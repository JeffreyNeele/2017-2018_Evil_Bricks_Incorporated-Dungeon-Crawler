using System.Collections.Generic;
using System;

abstract partial class Character
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
           inventory.Remove(item);
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

    public void UseAbility()
    {

    }

    public void UseSpecial()
    {

    }

    public void PassiveAbility()
    {

    }



}