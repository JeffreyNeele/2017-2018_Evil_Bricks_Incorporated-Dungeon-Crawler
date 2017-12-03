using System;
using System.Collections.Generic;

partial class Level
{
    protected GameObjectList playerList, monsterList, objectList;

    public Level()
    {
        playerList = new GameObjectList(0, "playerLIST");
        monsterList = new GameObjectList(1, "monsterLIST");
        objectList = new GameObjectList(2, "objectLIST");
    }

}
