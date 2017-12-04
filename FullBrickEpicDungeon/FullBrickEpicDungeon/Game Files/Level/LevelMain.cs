using System;
using System.Collections.Generic;

partial class Level
{
    // Lists for items
    protected GameObjectList playerList, monsterList, objectList, projectileList;

    public Level()
    {
        playerList = new GameObjectList(0, "playerLIST");
        monsterList = new GameObjectList(1, "monsterLIST");
        objectList = new GameObjectList(2, "objectLIST");
        projectileList = new GameObjectList(1, "projectileLIST");
    }

}
