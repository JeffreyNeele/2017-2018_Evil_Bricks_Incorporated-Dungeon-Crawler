using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

partial class Level : GameObjectList
{
    // Lists for items NOTE TO LEVEL PROGRAMMERS: do not remove these and make sure they are initialized before the monsters, players, objects etc.
    protected int levelIndex, numberOfPlayers;
    protected GameObjectList playerList, monsterList, objectList, icetileList, projectileList;
    protected GameObjectGrid levelTileField;
    public Level(int levelIndex) : base(1)
    {
        numberOfPlayers = FullBrickEpicDungeon.DungeonCrawler.numberOfPlayers;
        this.levelIndex = levelIndex;
        playerList = new GameObjectList(5, "playerLIST");
        monsterList = new GameObjectList(5, "monsterLIST");
        objectList = new GameObjectList(4, "objectLIST");
        icetileList = new GameObjectList(3, "icetileLIST");
        projectileList = new GameObjectList(2, "projectileLIST");

        Add(playerList);
        Add(monsterList);
        Add(objectList);
        Add(projectileList);

        LoadFromFile();
    }

    public GameObjectGrid TileField
    {
        get { return levelTileField; }
    }

}
