using Microsoft.Xna.Framework;

class Trapdoor : OpenableObject
{
    GameObjectList allPlayers;
    //naar volgende level moet nog geimplementeerd worden. Momenteel wordt hij alleen getekend.
    public Trapdoor(TileType door, string assetname, string id, int sheetIndex, Level level) : base(door, assetname, id, sheetIndex)
    {
        allPlayers = GameWorld.Find("playerLIST") as GameObjectList;
    }
    public override void Update(GameTime gameTime)
    {
        //DetectAllPlayers();
        base.Update(gameTime);
    }

    public void DetectAllPlayers()
    {
        int onTrapdoorCounter = 0;
        foreach (Character player in allPlayers.Children)
        {
            Rectangle quarterBoundingBox = new Rectangle((int)this.BoundingBox.X, (int)(this.BoundingBox.Y + 0.75 * Height), this.Width, (int)(this.Height / 4));
            if (this.BoundingBox.Contains(quarterBoundingBox))
                onTrapdoorCounter++;
        }

        if(onTrapdoorCounter == 4)
        {
            //Go to next level
        }
            

    }
}