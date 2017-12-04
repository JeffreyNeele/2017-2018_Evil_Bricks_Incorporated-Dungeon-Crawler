// Class for defining equipment
abstract class Equipment : SpriteGameObject
{
    protected string classType;
    protected int movementspeed, armour, goldcost; 
    string ID;
    protected Equipment(string classType, string assetName, string id, int layer = 0) : base(assetName, layer, id)
    {
        this.classType = classType;
    }

    public int MovementSpeed
    {
        get { return movementspeed; }
        protected set { movementspeed = value; }
    }

    public int Armour
    {
        get { return armour; }
        protected set { armour = value; }
    }

    public int GoldCost
    {
        get { return goldcost; }
        protected set { goldcost = value; }
    }

    public string ClassType
    {
        get { return classType; }
    }
}
