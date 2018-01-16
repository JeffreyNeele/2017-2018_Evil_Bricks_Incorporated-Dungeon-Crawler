// Class for defining equipment
using Microsoft.Xna.Framework;

abstract class Equipment : SpriteGameObject
{
    private int movementspeedincrease, armour, goldworth; 

    /// <summary>
    /// Class that defines an Equipment item
    /// </summary>
    /// <param name="assetName">asset name</param>
    /// <param name="id">id for this piece of equipment</param>
    /// <param name="layer">layer this equipment should be drawn on</param>
    protected Equipment(string assetName, string id, int layer = 0) : base(assetName, layer, id)
    {

    }

    public override void Update(GameTime gameTime)
    {

    }

    /// <summary>
    /// Properties for the equipment
    /// </summary>
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
