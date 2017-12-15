using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

partial class Level : GameObjectList
{
    // Lists for items NOTE TO LEVEL PROGRAMMERS: do not remove these and make sure they are initialized before the monsters, players, objects etc.
    protected GameObjectList playerList, monsterList, objectList, projectileList;
    protected GameObjectGrid levelTileField;
    public Level(/* int levelindex*/)
    {
        playerList = new GameObjectList(0, "playerLIST");
        monsterList = new GameObjectList(1, "monsterLIST");
        objectList = new GameObjectList(2, "objectLIST");
        projectileList = new GameObjectList(1, "projectileLIST");

        Add(playerList);
        Add(monsterList);
        Add(objectList);
        Add(projectileList);

        // dit moet waarschijnlijk ergens anders gezet worden.
    }

}
