// Contains all base attributes of equipment, monsters or character
// this class is initialized in one of those classes themselves.
using Microsoft.Xna.Framework;

class BaseAttributes
{
    public BaseAttributes(string id)
    {
        type = id;
    }
    private int hitpoints, armour, attack, gold;
    private Vector2 movementspeed;
    private string type;
    public int HP
    {
        get { return hitpoints; }
        set { hitpoints = value; }
    }
    public int Armour
    {
        get { return armour; }
        set { armour = value; }
    }
    public int Gold
    {
        get { return gold; }
        set { gold = value; }
    }
    public Vector2 MovementSpeed
    {
        get { return movementspeed; }
        set { movementspeed = value; }
    }
    public int Attack
    {
        get { return attack; }
        set { attack = value; }
    }
    public string Type
    {
        get { return type; }
    }
}