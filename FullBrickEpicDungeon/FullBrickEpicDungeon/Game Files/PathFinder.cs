using System;
using System.Collections.Generic;
using RoyT.AStar;
using Microsoft.Xna.Framework;

class PathFinder
{
    GameObjectGrid levelGrid;
    public PathFinder(GameObjectGrid levelGrid)
    {
        this.levelGrid = levelGrid;
    }
    public Point[] FindPath(Vector2 endPosition, Vector2 startPosition)
    {
        Grid pathGrid = new Grid(levelGrid.Columns, levelGrid.CellWidth);
        for (int x = 0; x < levelGrid.Columns; x++)
        {
            for (int y = 0; y < levelGrid.Rows; y++)
            {
                Tile currenttile = levelGrid.Objects[x, y] as Tile;
                if (currenttile.isSolid)
                {
                    pathGrid.BlockCell(new Position(x, y));
                }
                if (currenttile.Type == TileType.Water)
                {
                    pathGrid.SetCellCost(new Position(x, y), 10);
                }
            }
        }
        Position[] pathArray = pathGrid.GetSmoothPath(new Position((int)startPosition.X, (int)startPosition.Y), new Position((int)endPosition.X, (int)endPosition.Y));
        Point[] pointArray = new Point[pathArray.Length];
        for (int i = 0; i < pathArray.Length; i++)
        {
            pointArray[i] = new Point(pathArray[i].X, pathArray[i].Y);
        }
        return pointArray;
    }
}
