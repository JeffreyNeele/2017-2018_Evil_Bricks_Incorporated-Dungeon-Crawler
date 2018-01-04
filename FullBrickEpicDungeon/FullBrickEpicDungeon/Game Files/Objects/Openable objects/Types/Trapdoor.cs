using Microsoft.Xna.Framework;

class Trapdoor : OpenableObject
{
    //naar volgende level moet nog geimplementeerd worden. Momenteel wordt hij alleen getekend.
    public Trapdoor(TileType door, string assetname, string id, int sheetIndex) : base(door, assetname, id, sheetIndex)
    {
    }
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }
}