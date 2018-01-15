// Class for defining equipment
using Microsoft.Xna.Framework;

abstract class Equipment : SpriteGameObject
{
    private int movementspeedincrease, armour, goldworth; 

    protected Equipment(string assetName, string id, int layer = 0) : base(assetName, layer, id)
    {

    }

    public override void Update(GameTime gameTime)
    {

    }
    // Equipment has a movementspeedincrease, armour and goldworth value, furthermore it has a string but that's about it. Equipment names will derive from the id
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

}
