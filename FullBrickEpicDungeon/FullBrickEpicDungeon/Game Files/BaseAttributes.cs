using Microsoft.Xna.Framework;

/// <summary>
/// Class that defines simple variables that a monster or character have, should be self explanatory
/// </summary>
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