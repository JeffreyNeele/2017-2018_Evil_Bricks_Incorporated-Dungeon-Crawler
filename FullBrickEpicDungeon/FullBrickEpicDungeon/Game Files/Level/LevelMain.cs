using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

partial class Level : GameObjectList
{
    // Lists for items NOTE TO LEVEL PROGRAMMERS: do not remove these and make sure they are initialized before the monsters, players, objects etc.
    protected int levelIndex;
    protected GameObjectList playerList, monsterList, objectList, projectileList, AItracker;
    protected GameObjectGrid levelTileField;
    public Level(int levelIndex) : base(1, "nigger")
    {
        this.levelIndex = levelIndex;
        playerList = new GameObjectList(5, "playerLIST");
        monsterList = new GameObjectList(5, "monsterLIST");
        objectList = new GameObjectList(4, "objectLIST");
        projectileList = new GameObjectList(2, "projectileLIST");
        AItracker = new GameObjectList(0, "AItracker");

        Add(playerList);
        Add(monsterList);
        Add(objectList);
        Add(projectileList);
        Add(AItracker);

        LoadFromFile();
    }

    public GameObjectGrid TileField
    {
        get { return levelTileField; }
    }

}
