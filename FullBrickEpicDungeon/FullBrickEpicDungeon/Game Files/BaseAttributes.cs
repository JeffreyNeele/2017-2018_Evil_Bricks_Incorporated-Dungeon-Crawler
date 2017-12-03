
class BaseAttributes
{
    private int hitpoints, armour, gold, movementspeed;
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
    public int MovementSpeed
    {
        get { return movementspeed; }
        set { movementspeed = value; }
    }
}