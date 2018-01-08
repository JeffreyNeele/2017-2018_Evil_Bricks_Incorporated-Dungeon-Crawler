using Microsoft.Xna.Framework;

class Trapdoor : OpenableObject
{
    GameObjectList allPlayers;
    //naar volgende level moet nog geimplementeerd worden. Momenteel wordt hij alleen getekend.
    public Trapdoor(TileType door, string assetname, string id, int sheetIndex, Level level) : base(door, assetname, id, sheetIndex)
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
        allPlayers = GameWorld.Find("playerLIST") as GameObjectList;
        int onTrapdoorCounter = 0;
        foreach (Character player in allPlayers.Children)
        {
            Rectangle quarterBoundingBox = new Rectangle((int)player.BoundingBox.X, (int)(player.BoundingBox.Y + 0.75 * Height), player.Width, (int)(player.Height / 4));
            if (this.BoundingBox.Intersects(quarterBoundingBox))
                onTrapdoorCounter++;
        }

        if(onTrapdoorCounter == 2)
        {
            return true;
        }

        return false;

    }
}