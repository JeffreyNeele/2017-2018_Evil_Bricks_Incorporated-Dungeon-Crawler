using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

partial class Level : GameObjectList
{
    GameObjectGrid tileField;
    public void LoadFromFile(string path)
    {
        List<string> fileLines = new List<string>();
        path = "Content/" + path;
        StreamReader fileReader = new StreamReader(path);
        string line = fileReader.ReadLine();
        while (line != null)
        {
            fileLines.Add(line);
            line = fileReader.ReadLine();
        }
        int previousline;
        previousline = 8;

        List<string> tileLines = new List<string>();
        for (int i = previousline + 1; i < fileLines.Count; i++)
        {
            if(fileLines[i] == "POSITION")
            {
                LoadTiles(tileLines);
                tileLines.Clear();
                break;
            }
            tileLines.Add(fileLines[i]);
        }
    }
    protected void LoadTiles(List<string> tileStringList)
    {
        // TODO: go through the document and find the things you want to find height, width etc.

        tileField = new GameObjectGrid(10, 19, 0); // TODO: add the height and width to initialize the grid here
        tileField.CellWidth = 100; // TO DO set correct cell height and width, should either be hardcoded or be received from the file
        tileField.CellHeight = 100;

        // TODO: add a method that generates an array with all IDs from the file given
        int[,] IDlist;
        
        

        // TODO: add a method that goes through all the IDs in the IDlist and associates a correct value with a switch statement
        // Add the tile to the tileField at x,y in the double for loop
        for (int x = 0; x < tileField.Columns; x++)
        {
            for (int y = 0; y < tileField.Rows; y++)
            {
                
                Tile newtile;
                switch (IDlist[x, y])
                {
                    case 1:
                        newtile = new Tile(TileType.BasicTile, "Assets/Sprites/Tiles/BasicTile1"); // add the correct id here later this is just an example
                        break;
                    case 2:
                        newtile = new Tile(TileType.Brick, "Assets/Sprites/Tiles/TileBrick");
                        break;
                    case 3:
                        newtile = new Tile(TileType.Ice, "Assets/Sprites/Tiles/TileIce1");
                        break;
                    case 4:
                        newtile = new Tile(TileType.RockIce, "Assets/Sprites/Tiles/TileRockIce");
                        break;
                    case 5:
                        newtile = new Tile(TileType.Water, "Assets/Sprites/Tiles/TileWater1");
                        break;
                    default: throw new NullReferenceException("the given ID " + IDlist[x, y] + " was not found in the preprogrammed IDs");
                }
                tileField.Add(newtile);
                
            }
        }

        // PS: remember to add new Tile types to the enum list (Enumerators class) and to make a new class from them in the Tile.cs file
    }
}
