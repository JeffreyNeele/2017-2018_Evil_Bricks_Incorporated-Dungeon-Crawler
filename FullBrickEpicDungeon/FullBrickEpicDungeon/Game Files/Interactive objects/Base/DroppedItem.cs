using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

class DroppedItem : InteractiveObject
{
    GameObject item;
    public DroppedItem(SpriteGameObject item, string id, int layer = 2) : base("", id, 0, layer)
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