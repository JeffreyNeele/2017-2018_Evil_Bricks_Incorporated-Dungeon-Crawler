using Microsoft.Xna.Framework;

/// <summary>
/// Class for a Trap Door.
/// </summary>
class Trapdoor : OpenableObject
{

    /// <param name="assetname">Path to be able to load in the sprite</param>
    /// <param name="id">defined id to be able to find the door</param>
    /// <param name="sheetIndex">Defines which picture of the animation will be shown</param>
    public Trapdoor(string assetname, string id, int sheetIndex) : base(TileType.Trapdoor, assetname, id, sheetIndex)
    {
    }
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }
}