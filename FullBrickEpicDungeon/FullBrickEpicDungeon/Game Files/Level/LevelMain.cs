
partial class Level : GameObjectList
{
    // ints for levelindex and the current amount of players in the level.
    protected int levelIndex, numberOfPlayers;
    // Lists for different objects
    protected GameObjectList playerList, monsterList, objectList, projectileList;
    // var for the leveltilefield
    protected GameObjectGrid levelTileField;
    /// <summary>
    /// Main level class
    /// </summary>
    /// <param name="levelIndex">Relative level index</param>
    public Level(int levelIndex) : base(1)
    {
        numberOfPlayers = CharacterSelection.NumberOfPlayers;
        this.levelIndex = levelIndex;
        // assign the lists and add them
        playerList = new GameObjectList(5, "playerLIST");
        monsterList = new GameObjectList(5, "monsterLIST");
        objectList = new GameObjectList(4, "objectLIST");
        projectileList = new GameObjectList(2, "projectileLIST");
        Add(playerList);
        Add(monsterList);
        Add(objectList);
        Add(projectileList);

        // load the level
        LoadFromFile();
    }

    public GameObjectGrid TileField
    {
        get { return levelTileField; }
    }

}


