
class BaseAttributes
{
    public BaseAttributes(string id)
    {
        type = id;
    }
    private int hitpoints, armour, attack, gold, movementspeed;
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
    public int MovementSpeed
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