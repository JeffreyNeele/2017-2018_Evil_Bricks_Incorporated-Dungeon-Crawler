

using Microsoft.Xna.Framework;

class Door : OpenableObject
{
    //naar volgende level moet nog geimplementeerd worden. Momenteel wordt hij alleen getekend.
    public Door(TileType door, string assetname, string id, int sheetIndex) : base(door, assetname, id, sheetIndex)
    {
    }
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }
}