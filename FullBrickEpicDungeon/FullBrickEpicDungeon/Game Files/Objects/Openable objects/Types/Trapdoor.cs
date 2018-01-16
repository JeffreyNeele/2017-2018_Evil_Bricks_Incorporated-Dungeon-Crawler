using Microsoft.Xna.Framework;

class Trapdoor : OpenableObject
{
    /// <summary>
    /// Class for a Trap Door.
    /// </summary>
    /// <param name="assetname"></param>
    /// <param name="id"></param>
    /// <param name="sheetIndex"></param>
    public Trapdoor(string assetname, string id, int sheetIndex) : base(TileType.Trapdoor, assetname, id, sheetIndex)
    {
    }
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }
}