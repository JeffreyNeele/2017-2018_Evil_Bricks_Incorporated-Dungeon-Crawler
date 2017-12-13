
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml.Linq;

class TestState : GameObjectList
{
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

        Tile[,] tiles = new Tile[mapWidth, mapHeight];
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                // change default values later such as id
                Tile newtile = new Tile(TileType.Wall, "");
                newtile.pos
                tiles[x, y]
                   // new Vector2(x * 16, y * 16)
                   // new Rectangle((int)sourcePos[intIDs[x, y] - 1].X, (int)sourcePos[intIDs[x, y] - 1].Y, 16, 16)

            }
        }
        int id;
        Tile newtile;
        switch (id)
        {
            case 1: newtile(TileType.Wall, assetname)
        }
        // optional statements for all tiles
        tilesgrid.Add(newtile)
        return tiles;
    }
}
