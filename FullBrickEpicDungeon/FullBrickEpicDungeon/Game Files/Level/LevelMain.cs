using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

partial class Level : GameObjectList
{
    // Lists for items NOTE TO LEVEL PROGRAMMERS: do not remove these and make sure they are initialized before the monsters, players, objects etc.
    protected int levelIndex;
    protected GameObjectList playerList, monsterList, objectList, projectileList;
    protected GameObjectGrid levelTileField;
    public Level(int levelIndex)
    {
        this.levelIndex = levelIndex;
        playerList = new GameObjectList(5, "playerLIST");
        monsterList = new GameObjectList(3, "monsterLIST");
        objectList = new GameObjectList(4, "objectLIST");
        projectileList = new GameObjectList(2, "projectileLIST");

        Add(playerList);
        Add(monsterList);
        Add(objectList);
        Add(projectileList);
        // dit moet waarschijnlijk ergens anders gezet worden.

        LoadFromFile();
    }

}
