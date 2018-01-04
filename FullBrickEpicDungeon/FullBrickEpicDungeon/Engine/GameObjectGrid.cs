﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class GameObjectGrid : GameObject
{
    protected GameObject[,] grid;
    protected int cellWidth, cellHeight;
    protected GameObjectList debugText;
    public GameObjectGrid(int rows, int columns, int layer = 0, string id = "")
        : base(layer, id)
    {
        debugText = new GameObjectList(99);
        grid = new GameObject[columns, rows];
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                grid[x, y] = null;
            }
        }
        MakeDebugNumbers();
    }

    public void Add(GameObject obj, int x, int y)
    {
        grid[x, y] = obj;
        obj.Parent = this;
        obj.Position = new Vector2(x * cellWidth, y * cellHeight);
    }

    public GameObject Get(int x, int y)
    {
        if (x >= 0 && x < grid.GetLength(0) && y >= 0 && y < grid.GetLength(1))
        {
            return grid[x, y];
        }
        else
        {
            return null;
        }
    }

    public GameObject[,] Objects
    {
        get
        {
            return grid;
        }
    }

    public Vector2 GetAnchorPosition(GameObject s)
    {
        for (int x = 0; x < Columns; x++)
        {
            for (int y = 0; y < Rows; y++)
            {
                if (grid[x, y] == s)
                {
                    return new Vector2(x * cellWidth, y * cellHeight);
                }
            }
        }
        return Vector2.Zero;
    }

    public int Rows
    {
        get { return grid.GetLength(1); }
    }

    public int Columns
    {
        get { return grid.GetLength(0); }
    }

    public int CellWidth
    {
        get { return cellWidth; }
        set { cellWidth = value; }
    }

    public int CellHeight
    {
        get { return cellHeight; }
        set { cellHeight = value; }
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        foreach (GameObject obj in grid)
        {
            obj.HandleInput(inputHelper);
        }
    }

    public override void Update(GameTime gameTime)
    {
        foreach (GameObject obj in grid)
        {
            obj.Update(gameTime);
        }
        debugText.Update(gameTime);
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        foreach (GameObject obj in grid)
        {
            obj.Draw(gameTime, spriteBatch);
        }
        debugText.Draw(gameTime, spriteBatch);
    }

    public override void Reset()
    {
        base.Reset();
        foreach (GameObject obj in grid)
        {
            obj.Reset();
        }
    }

    public void MakeDebugNumbers()
    {
        for (int x = 0; x < Columns; x++)
        {
            for (int y = 0; y < Rows; y++)
            {
                TextGameObject numberViewer = new TextGameObject("Assets/Fonts/ConversationFont", 99);
                numberViewer.Text = x.ToString() + " " + y.ToString();
                numberViewer.Color = Color.Red;
                numberViewer.Position = new Vector2(x * 100, y * 100);
                debugText.Add(numberViewer);
            }
        }
    }
}
