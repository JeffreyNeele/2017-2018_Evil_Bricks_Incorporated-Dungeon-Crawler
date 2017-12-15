using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

partial class Level : GameObjectList
{
    // Method that glues all other load methods together and checks which parts of the files it should pass to these methods
    public void LoadFromFile(string path)
    {
        // Define a list for all information in the file
        List<string> fileLines = new List<string>();
        // Edit the path so it is set correctly for the stream reader
        path = "Content/" + path;
        StreamReader fileReader = new StreamReader(path);
        string line = fileReader.ReadLine();
        // Reads the file
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
                levelTileField = LoadTiles(tileLines);
                tileLines.Clear();
                break;
            }
            tileLines.Add(fileLines[i]);
        }
    }
    // Method that loads tiles based on an ID list seperated by commas e.g. 3,4,4,5,3,4
    protected GameObjectGrid LoadTiles(List<string> tileStringList)
    {
        // Throw an exception if the given Tile string list is null
        if(tileStringList == null)
        {
            throw new NullReferenceException("The given Tile list was null");
        }

        // Make a new tile field with x being the length of a line (the length) and the amount of lines the y direction
        GameObjectGrid tileField = new GameObjectGrid(tileStringList[0].Length / 2, tileStringList.Count, 0, "TileField"); 

        //values for the cell width and height, these are predetermined in the Tiled Map Editor, so are constants.
        tileField.CellWidth = 100; 
        tileField.CellHeight = 100;

        // Go through all given lines in the file, it is assumed here that it is in the correct form. 
        int[,] IDlist = new int[tileField.Columns, tileField.Rows];

        for(int y = 0; y < tileField.Columns; y++)
        {
            string[] lineArray = tileStringList[y].Split(',');
            for(int x = 0; x < tileField.Rows; x++)
            {
                IDlist[x, y] = int.Parse(lineArray[x]); 
            }
        }

        // Go through each position in the ID list and give the correct tile for each ID, and in the end return the tileField to the invoker of this method
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
                        newtile = new Tile(TileType.RockIce, "Assets/Sprites/Tiles/RockIce");
                        break;
                    case 5:
                        newtile = new Tile(TileType.Water, "Assets/Sprites/Tiles/TileWater1");
                        break;
                    default: throw new NullReferenceException("the given ID " + IDlist[x, y] + " was not found in the preprogrammed IDs");
                }
                tileField.Add(newtile, x, y);
            }
        }
        return tileField;
    }
}
