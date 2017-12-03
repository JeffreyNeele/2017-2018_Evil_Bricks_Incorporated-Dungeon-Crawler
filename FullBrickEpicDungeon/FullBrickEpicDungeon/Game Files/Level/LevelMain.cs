using System;
using System.Collections.Generic;

partial class Level
{
    protected GameObjectList playerList, monsterList;

    public Level()
    {
        playerList = new GameObjectList(0, "playerlist");
        monsterList = new GameObjectList(1, "monsterlist");
    }

}
