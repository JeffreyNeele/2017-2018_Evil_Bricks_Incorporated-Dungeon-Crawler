using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

partial class Level : GameObjectList
{
    protected int levelIndex, numberOfPlayers;
    protected GameObjectList playerList, monsterList, objectList, projectileList;
    protected GameObjectGrid levelTileField;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="levelIndex"></param>
    public Level(int levelIndex) : base(1)
    {
        numberOfPlayers = FullBrickEpicDungeon.DungeonCrawler.numberOfPlayers;
        this.levelIndex = levelIndex;
        playerList = new GameObjectList(5, "playerLIST");
        monsterList = new GameObjectList(5, "monsterLIST");
        objectList = new GameObjectList(4, "objectLIST");
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
