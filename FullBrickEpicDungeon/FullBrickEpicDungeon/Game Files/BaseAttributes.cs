// Contains all base attributes of equipment, monsters or character
// this class is initialized in one of those classes themselves.
using Microsoft.Xna.Framework;

class BaseAttributes
{
    public BaseAttributes()
    {

    }
    private int hitpoints, armour, attack, gold;
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

    public int Attack
    {
        get { return attack; }
        set { attack = value; }
    }
}