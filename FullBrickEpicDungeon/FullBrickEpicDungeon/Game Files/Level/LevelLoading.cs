using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

partial class Level : GameObjectList
{
    GameObjectGrid tileField;
    protected void LoadTiles()
    {
        // TODO: go through the document and find the things you want to find height, width etc.

        tileField = new GameObjectGrid(); // TODO: add the height and width to initialize the grid here
        tileField.CellWidth = 0; // TO DO set correct cell height and width, should either be hardcoded or be received from the file
        tileField.CellHeight = 0;

        // TODO: add a method that generates an array with all IDs  from the file given
        int[,] IDlist;

        // TODO: add a method that goes through all the IDs in the IDlist and associates a correct value with a switch statement
        // Add the tile to the tileField at x,y in the double for loop
        for(int x = 0; x < tileField.Columns; x++)
        {
            for(int y = 0; y < tileField.Rows; y++)
            {
                Tile newtile;
                switch (IDlist[x, y])
                {
                    case 1: newtile = new Tile(TileType.Wall, "" // add the correct id here later this is just an example
                }
            }
        }

        // PS: remember to add new Tile types to the enum list (Enumerators class) and to make a new class from them in the Tile.cs file
    }
}
