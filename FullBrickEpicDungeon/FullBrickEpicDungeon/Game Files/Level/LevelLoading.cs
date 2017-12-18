using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
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
        int previousline = 0;
        // a list for the currently stored lines in a for loop
        List<string> storedLines = new List<string>();
        // list that loads the level information
        for (int i = previousline + 1; i < fileLines.Count; i++)
        {
            // if we reach TILES this means that the qualification for the next for loop has been reached and we dont need any more information for this loop
            if (fileLines[i] == "TILES")
            {
                // Loads the level information
                LevelInformationLoader(storedLines);
                // clear the stored lines for the next for loop in our list
                storedLines.Clear();
                // set the previousline to what this for loop ended on so that the next for loop doesn't receive information it doesn't need
                previousline = i;
                break;
            }
            storedLines.Add(fileLines[i]);
        }

        // Load the tile field from the file
        for (int i = previousline + 1; i < fileLines.Count; i++)
        {
            if(fileLines[i] == "POSITION")
            {
                // set the levels tileField to the LoadTiles method returned tileField
                levelTileField = LoadTiles(storedLines);
                Add(levelTileField);
                storedLines.Clear();
                previousline = i;
                break;
            }
            storedLines.Add(fileLines[i]);
        }

        // Loads position and characters etc.
        for (int i = previousline + 1; i < fileLines.Count; i++)
        {
            if (fileLines[i] == "ENDOFFILE")
            {
                LevelPositionLoader(storedLines);
                storedLines.Clear();
                break;
            }
            storedLines.Add(fileLines[i]);
        }

    }

    //Loads level information
    protected void LevelInformationLoader(List<string> informationStringList)
    {
        this.id = "LEVEL_" + informationStringList[0];
    }

    // Loads Characters at their appropiate position, as well as interactive objects
    protected void LevelPositionLoader(List<string> positionStringList)
    {
        int handlecounter = 0;
        for (int i = 0; i < positionStringList.Count; i++)
        {
            // Split the current line
            string[] splitArray = positionStringList[i].Split(' ');
            if(splitArray[0] == "SHIELDMAIDEN")
            {
                Shieldmaiden shieldmaiden = new Shieldmaiden();
                shieldmaiden.StartPosition = new Vector2(float.Parse(splitArray[1]), float.Parse(splitArray[2]));
                shieldmaiden.CurrentWeapon = new SwordAndShield(shieldmaiden);
                shieldmaiden.Reset();
                playerList.Add(shieldmaiden);
            }
            if (splitArray[0] == "HANDLE")
            {
                Handle handle = new Handle("Assets/Sprites/InteractiveObjects/handle1", "Handle", handlecounter);
                handle.Position = new Vector2(float.Parse(splitArray[1]), float.Parse(splitArray[2]));
                objectList.Add(handle);
            }

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
        GameObjectGrid tileField = new GameObjectGrid(tileStringList.Count, tileStringList[0].Length / 2, 3, "TileField"); 

        //values for the cell width and height, these are predetermined in the Tiled Map Editor, so are constants.
        tileField.CellWidth = 100; 
        tileField.CellHeight = 100;

        // Go through all given lines in the file, it is assumed here that it is in the correct form. 
        int[,] IDlist = new int[tileField.Columns, tileField.Rows];

        for(int y = 0; y < tileField.Rows; y++)
        {
            string[] lineArray = tileStringList[y].Split(',');
            for(int x = 0; x < tileField.Columns; x++)
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
