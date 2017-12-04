using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

class DroppedItem : InteractiveObject
{
    GameObject item;
    public DroppedItem(GameObject item, string assetName, string id, int sheetIndex, int layer = 2) : base(assetName, id, sheetIndex, layer)
    {
        this.item = item;
    }

    public override void Update(GameTime gameTime)
    {
        if (interacting)
        {
            this.PickUp(TargetCharacter);
        }
        base.Update(gameTime);
    }
    public void PickUp(Character character)
    {
        if(item is Equipment)
        {
            Equipment gear = item as Equipment;
            character.ChangeItems(gear);
        }

        if(item is Weapon)
        {
            Weapon weapon = item as Weapon;
            character.ChangeWeapon(weapon);
        }
    }

}