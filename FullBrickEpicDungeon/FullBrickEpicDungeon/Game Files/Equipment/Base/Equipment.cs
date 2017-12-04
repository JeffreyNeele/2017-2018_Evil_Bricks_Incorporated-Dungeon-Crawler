// Class for defining equipment
abstract class Equipment : SpriteGameObject
{
    protected ClassType classType;
    private int movementspeedincrease, armour, goldworth; 

    protected Equipment(ClassType classType, string assetName, string id, int layer = 0) : base(assetName, layer, id)
    {
        this.classType = classType;
    }

    public int MovementSpeedIncrease
    {
        get { return movementspeedincrease; }
        protected set { movementspeedincrease = value; }
    }

    public int Armour
    {
        get { return armour; }
        protected set { armour = value; }
    }

    public int GoldWorth
    {
        get { return goldworth; }
        protected set { goldworth = value; }
    }

    public ClassType Type
    {
        get { return classType; }
    }
}
