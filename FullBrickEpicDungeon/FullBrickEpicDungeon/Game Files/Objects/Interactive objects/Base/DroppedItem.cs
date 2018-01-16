using Microsoft.Xna.Framework;

/// <summary>
/// Class for defining a dropped item that is on the ground, takes the item itself as a property.
/// </summary>
class DroppedItem : InteractiveObject
{
    GameObject item;
    public DroppedItem(SpriteGameObject item, string id, int layer = 2) : base("", id, 0, layer)
    {
        this.item = item;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    // Method for picking this item up
    protected override void Interact(Character character)
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