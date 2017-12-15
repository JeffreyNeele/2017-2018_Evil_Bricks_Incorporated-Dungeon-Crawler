
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml.Linq;

class TestState : GameObjectList
{
    /*
    public Tile[,] getTileset()
    {
        XDocument xDoc = XDocument.Load("Content/Level/TileMapTest2.tmx");
        int mapWidth = int.Parse(xDoc.Root.Attribute("width").Value);
        int mapHeight = int.Parse(xDoc.Root.Attribute("height").Value);
        // somehow make an ID array (of ints)
        // then you make a for loop for both x and y, for all IDs, make a correct tile in the case statement
        // and set the position after the switch statement, after which you add the state

        int tilecount = 10; //int.Parse(xDoc.Root.Element("Tilemaptest").Attribute("tilecount").Value);
        int columns = 19; //int.Parse(xDoc.Root.Element("Tilemaptest").Attribute("columns").Value);

        string IDArray = xDoc.Root.Element("layer").Element("data").Value;
        string[] splitArray = IDArray.Split(',');

        int[,] intIDs = new int[mapWidth, mapHeight];

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                intIDs[x, y] = int.Parse(splitArray[x + y * mapWidth]);
            }
        }

        int key = 0;
        Vector2[] sourcePos = new Vector2[tilecount];
        for (int x = 0; x < tilecount / columns; x++)
        {
            for (int y = 0; y < columns; y++)
            {
                sourcePos[key] = new Vector2(y * 16, x * 16);
                key++;
            }

        }
    }
    */
    Level gay;
    public TestState()
    {
        gay = new Level();
        gay.LoadFromFile("Assets/Levels/Demolevelfile.txt");
        Add(gay);
    }

    public override void Update(GameTime gameTime)
    {
        animationTester.Update(gameTime);
        base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        animationTester.Draw(gameTime, spriteBatch);
        base.Draw(gameTime, spriteBatch);
    }
}
