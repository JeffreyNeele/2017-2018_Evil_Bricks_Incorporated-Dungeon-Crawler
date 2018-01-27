using Microsoft.Xna.Framework;

/// <summary>
/// Class for a Trap Door.
/// </summary>
class Trapdoor : OpenableObject
{
    GameObjectList allPlayers;
    /// <param name="assetname">Path to be able to load in the sprite</param>
    /// <param name="id">defined id to be able to find the door</param>
    /// <param name="sheetIndex">Defines which picture of the animation will be shown</param>
    public Trapdoor(TileType door, string assetname, string id, int sheetIndex, Level level) : base(door, assetname, level, id, sheetIndex)
    {

    }
    public override void Update(GameTime gameTime)
    {
        if (DetectAllPlayers() && this.Sprite.SheetIndex == 1)
            GameEnvironment.GameStateManager.SwitchTo("levelFinishedState");
        base.Update(gameTime);
    }

    public bool DetectAllPlayers()
    {
        allPlayers = currentlevel.GameWorld.Find("playerLIST") as GameObjectList;
        int onTrapdoorCounter = 0;
        foreach (Character player in allPlayers.Children)
        {
            if (this.CollidesWith(player))
                onTrapdoorCounter++;
        }

        if (onTrapdoorCounter >= CharacterSelection.NumberOfPlayers)
        {
            return true;
        }

        return false;

    }
}