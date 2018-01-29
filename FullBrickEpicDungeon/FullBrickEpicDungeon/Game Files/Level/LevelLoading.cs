using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.IO;

/// <summary>
/// Class that loads the level
/// </summary>
partial class Level : GameObjectList
{
    /// <summary>
    /// Method that loads everything from a file with the name level + levelindex 
    /// </summary>
    public void LoadFromFile()
    {
        // Define a list for all information in the file
        List<string> fileLines = new List<string>();
        // Edit the path so it is set correctly for the stream reader
        string path = "Content/Assets/Levels/level" + levelIndex + ".txt";
        StreamReader fileReader = new StreamReader(path);
        string line = fileReader.ReadLine();
        // Reads the file
        while (line != null)
        {
            fileLines.Add(line);
            line = fileReader.ReadLine();
        }
        fileReader.Close();
        // a list for the currently stored lines in a for loop
        List<string> storedLines = new List<string>();
        // list that loads the level information

        for(int i = 0; i < fileLines.Count; i++)
        {
            // if we reach TILES this means that the qualification for the next for loop has been reached and we dont need any more information for this loop
            if (fileLines[i] == "TILES")
            {
                // Loads the level information
                LevelInformationLoader(storedLines);
                // clear the stored lines for the next for loop in our list
                storedLines.Clear();
            }

            else if (fileLines[i] == "POSITION")
            {
                // set the levels tileField to the LoadTiles method returned tileField
                levelTileField = LoadTiles(storedLines);
                Add(levelTileField);
                storedLines.Clear();
            }

            //else if (fileLines[i] == "HINT")
            //{
            //    // displays the hint on the top of the screen
            //    HintTextDisplayer(storedLines);
            //    storedLines.Clear();
            //}

            else if (fileLines[i] == "ENDOFFILE")
            {
                LevelPositionLoader(storedLines);
                storedLines.Clear();
            }
            else
                storedLines.Add(fileLines[i]);

        }
    }

/// <summary>
/// Loads the level information
/// </summary>
/// <param name="informationStringList">given string list that corresponds to the level information</param>
protected void LevelInformationLoader(List<string> informationStringList)
    {
        this.id = "LEVEL_" + informationStringList[0];
    }

    /// <summary>
    /// Loads things that aren't tiles at their correct positions
    /// </summary>
    /// <param name="positionStringList">The given list of strings by the main method</param>
    protected void LevelPositionLoader(List<string> positionStringList)
    {
        for (int i = 0; i < positionStringList.Count; i++)
        {
            string[] splitArray = positionStringList[i].Split(' ');
            // All the code blocks below check what object needs to be loaded, loads them in, and then assigns the correct values that are in the file to them
            switch (splitArray[0])
            {
                case "PENGUIN":
                    Monster penguin = new LittlePenguin(this)
                    {
                        StartPosition = new Vector2(float.Parse(splitArray[1]), float.Parse(splitArray[2]))
                    };
                    penguin.Reset();
                    if (splitArray.Length == 4)
                        addHandleSummon(penguin, int.Parse(splitArray[3]));
                    else
                        monsterList.Add(penguin);
                    break;
                case "DUMMY":
                    Monster dummy = new Dummy("Assets/Sprites/Enemies/Dummy", this)
                    {
                        StartPosition = new Vector2(float.Parse(splitArray[1]), float.Parse(splitArray[2]))
                    };
                    dummy.Reset();
                    monsterList.Add(dummy);
                    break;
                case "BUNNY":
                    Bunny bunny = new Bunny(this)
                    {
                        StartPosition = new Vector2(float.Parse(splitArray[1]), float.Parse(splitArray[2]))
                    };
                    bunny.Reset();
                    if (splitArray.Length == 4)
                        addHandleSummon(bunny, int.Parse(splitArray[3]));
                    else
                        monsterList.Add(bunny);
                    break;
                case "BOSSBUNNY":
                    BossBunny bossBunny = new BossBunny(this)
                    {
                        StartPosition = new Vector2(float.Parse(splitArray[1]), float.Parse(splitArray[2]))
                    };
                    bossBunny.Reset();
                    if (splitArray.Length == 4)
                        addHandleSummon(bossBunny, int.Parse(splitArray[3]));
                    else
                        monsterList.Add(bossBunny);
                    break;
                case "BOSSPENGUIN":
                    BossPenguin bossPenguin = new BossPenguin(this)
                    {
                        StartPosition = new Vector2(float.Parse(splitArray[1]), float.Parse(splitArray[2]))
                    };
                    bossPenguin.Reset();
                    if (splitArray.Length == 4)
                        addHandleSummon(bossPenguin, int.Parse(splitArray[3]));
                    else
                        monsterList.Add(bossPenguin);
                    break;
                case "SPIKETRAP":
                    AutomatedObject spikeTrap = new SpikeTrap("Assets/Sprites/InteractiveObjects/SpikeTrap@2", "spikeTrap", 0, this);
                    spikeTrap.Position = new Vector2(float.Parse(splitArray[1]), float.Parse(splitArray[2]));
                    objectList.Add(spikeTrap);
                    break;
                case "HANDLE":
                    Handle handle = new Handle("Assets/Sprites/InteractiveObjects/handles@2", this, "Handle", 0)
                    {
                        Position = new Vector2(float.Parse(splitArray[1]), float.Parse(splitArray[2]))
                    };
                    handle.ObjectNumberConnected = int.Parse(splitArray[3]);
                    if (splitArray.Length == 5)
                        if (int.Parse(splitArray[4]) == 1)
                            handle.ableToSummon = true;
                    objectList.Add(handle);
                    break;
                case "TRAPDOOR":
                    Trapdoor trapdoor = new Trapdoor(TileType.DoorTile, "Assets/Sprites/InteractiveObjects/NextLevelCombined@2", "Trapdoor", 0, this)
                    {
                        Position = new Vector2(float.Parse(splitArray[1]), float.Parse(splitArray[2])),
                        Objectnumber = int.Parse(splitArray[3])
                    };
                    objectList.Add(trapdoor);
                    break;
                case "DOOR":
                    Door doorupperleft = new Door("Assets/Sprites/Tiles/DoorBottomRight@2", this, "Door", 0)
                    {
                        Position = new Vector2(float.Parse(splitArray[1]) + 50, float.Parse(splitArray[2]) + 50),
                        upperLeftPosition = new Vector2(float.Parse(splitArray[1]), float.Parse(splitArray[2])),
                        Objectnumber = int.Parse(splitArray[3])
                    };
                    Door doorupperright = new Door("Assets/Sprites/Tiles/DoorUpperRight@2", this, "Door", 0)
                    {
                        Position = new Vector2(float.Parse(splitArray[1]) + 50, float.Parse(splitArray[2])),
                        upperLeftPosition = new Vector2(float.Parse(splitArray[1]), float.Parse(splitArray[2])),
                        Objectnumber = int.Parse(splitArray[3])
                    };
                    Door doorbottomleft = new Door("Assets/Sprites/Tiles/DoorBottomLeft@2", this, "Door", 0)
                    {
                        Position = new Vector2(float.Parse(splitArray[1]), float.Parse(splitArray[2]) + 50),
                        upperLeftPosition = new Vector2(float.Parse(splitArray[1]), float.Parse(splitArray[2])),
                        Objectnumber = int.Parse(splitArray[3])
                    };
                    Door doorbottomright = new Door("Assets/Sprites/Tiles/DoorUpperleft@2", this, "Door", 0)
                    {
                        Position = new Vector2(float.Parse(splitArray[1]), float.Parse(splitArray[2])),
                        upperLeftPosition = new Vector2(float.Parse(splitArray[1]), float.Parse(splitArray[2])),
                        Objectnumber = int.Parse(splitArray[3])
                    };
                    levelTileField.Add(doorupperleft, (int)doorupperleft.Position.X / 50, (int)doorupperleft.Position.Y / 50);
                    levelTileField.Add(doorupperright, (int)doorupperright.Position.X / 50, (int)doorupperright.Position.Y / 50);
                    levelTileField.Add(doorbottomleft, (int)doorbottomleft.Position.X / 50, (int)doorbottomleft.Position.Y / 50);
                    levelTileField.Add(doorbottomright, (int)doorbottomright.Position.X / 50, (int)doorbottomright.Position.Y / 50);
                    break;
                case "VDOOR":
                    Door vdoorupperleft = new VerticalDoor("Assets/Sprites/Tiles/VerticalDoorUpperLeft@2", this, "Door", 0)
                    {
                        Position = new Vector2(float.Parse(splitArray[1]), float.Parse(splitArray[2])),
                        upperLeftPosition = new Vector2(float.Parse(splitArray[1]), float.Parse(splitArray[2])),
                        Objectnumber = int.Parse(splitArray[3])
                    };
                    Door vdoorupperright = new VerticalDoor("Assets/Sprites/Tiles/VerticalDoorUpperRight@2", this, "Door", 0)
                    {
                        Position = new Vector2(float.Parse(splitArray[1]) + 50, float.Parse(splitArray[2])),
                        upperLeftPosition = new Vector2(float.Parse(splitArray[1]), float.Parse(splitArray[2])),
                        Objectnumber = int.Parse(splitArray[3])
                    };
                    Door vdoormiddleleft = new VerticalDoor("Assets/Sprites/Tiles/VerticalDoorMiddleLeft@2", this, "Door", 0)
                    {
                        Position = new Vector2(float.Parse(splitArray[1]), float.Parse(splitArray[2]) + 50),
                        upperLeftPosition = new Vector2(float.Parse(splitArray[1]), float.Parse(splitArray[2])),
                        Objectnumber = int.Parse(splitArray[3])
                    };
                    Door vdoorbottomleft = new VerticalDoor("Assets/Sprites/Tiles/VerticalDoorBottomLeft@2", this, "Door", 0)
                    {
                        Position = new Vector2(float.Parse(splitArray[1]), float.Parse(splitArray[2]) + 100),
                        upperLeftPosition = new Vector2(float.Parse(splitArray[1]), float.Parse(splitArray[2])),
                        Objectnumber = int.Parse(splitArray[3])
                    };
                    Door vdoormiddleright = new VerticalDoor("Assets/Sprites/Tiles/VerticalDoorMiddleRight@2", this, "Door", 0)
                    {
                        Position = new Vector2(float.Parse(splitArray[1]) + 50, float.Parse(splitArray[2]) + 50),
                        upperLeftPosition = new Vector2(float.Parse(splitArray[1]), float.Parse(splitArray[2])),
                        Objectnumber = int.Parse(splitArray[3])
                    };
                    levelTileField.Add(vdoorupperleft, (int)vdoorupperleft.Position.X / 50, (int)vdoorupperleft.Position.Y / 50);
                    levelTileField.Add(vdoorupperright, (int)vdoorupperright.Position.X / 50, (int)vdoorupperright.Position.Y / 50);
                    levelTileField.Add(vdoorbottomleft, (int)vdoorbottomleft.Position.X / 50, (int)vdoorbottomleft.Position.Y / 50);
                    levelTileField.Add(vdoormiddleright, (int)vdoormiddleright.Position.X / 50, (int)vdoormiddleright.Position.Y / 50);
                    levelTileField.Add(vdoormiddleleft, (int)vdoormiddleleft.Position.X / 50, (int)vdoormiddleleft.Position.Y / 50);
                    break;
                case "SHIELDMAIDEN":
                    ShieldMaidenLoader(splitArray);
                    break;
                case "KEY":
                    KeyLoader(splitArray);
                    break;
                case "LOCK":
                    LockLoader(splitArray);
                    break;
            }
        }
    }

    /// <summary>
    /// Loads the correct color of lock into the level
    /// </summary>
    /// <param name="textArray">array given by the Position loader</param>
    private void LockLoader(string[] textArray)
    {
        Lock lockitem = null;
        switch (textArray[3])
        {
            case "RED":
                lockitem = new Lock("Assets/Sprites/InteractiveObjects/PaladinLock", this, "redlock", 0);
                break;
            case "BLUE":
                lockitem = new Lock("Assets/Sprites/InteractiveObjects/LightbringerLock", this, "bluelock", 0);
                break;
            case "GREEN":
                lockitem = new Lock("Assets/Sprites/InteractiveObjects/RogueLock", this, "greenlock", 0);
                break;
            case "ORANGE":
                lockitem = new Lock("Assets/Sprites/InteractiveObjects/TalismaniacLock", this, "orangelock", 0);
                break;
            case "ALL":
                lockitem = new Lock("Assets/Sprites/InteractiveObjects/AllLock", this, "alllock", 0);
                break;
            default:
                throw new ArgumentException("The given color " + textArray[3] + " was not found in the switch statement!");
        }
        lockitem.Position = new Vector2(float.Parse(textArray[1]), float.Parse(textArray[2]));
        lockitem.Objectnumber = int.Parse(textArray[4]);
        if (lockitem != null)
            objectList.Add(lockitem);
        else
            throw new ArgumentNullException("added lock was null");
    }

    /// <summary>
    /// Loads the correct color of key into the level
    /// </summary>
    /// <param name="textArray">array given by the Position loader</param>
    private void KeyLoader(string[] textArray)
    {
        KeyItem key = null;
        switch (textArray[3])
        {
            case "RED":
                key = new KeyItem("Assets/Sprites/InteractiveObjects/paladinkey1", this, "redkey", 0);
                break;
            case "BLUE":
                key = new KeyItem("Assets/Sprites/InteractiveObjects/lightbringerkey1", this, "bluekey", 0);
                break;
            case "GREEN":
                key = new KeyItem("Assets/Sprites/InteractiveObjects/roguekey1", this, "greenkey", 0);
                break;
            case "ORANGE":
                key = new KeyItem("Assets/Sprites/InteractiveObjects/talismaniackey1", this, "orangekey", 0);
                break;
            case "ALL":
                key = new KeyItem("Assets/Sprites/InteractiveObjects/AllKey1", this, "allkey", 0);
                break;
            default:
                throw new ArgumentException("The given color " + textArray[3] + " was not found in the switch statement!");
        }
        key.Position = new Vector2(float.Parse(textArray[1]) + 10, float.Parse(textArray[2]) + 10);
        key.Objectnumber = int.Parse(textArray[4]);
        if (key != null)
            objectList.Add(key);
        else
            throw new ArgumentNullException("added key was null");
    }

    /// <summary>
    /// Loads the Characters into the level
    /// </summary>
    /// <param name="textArray">array given by the Position loader</param>
    private void ShieldMaidenLoader(string[] textArray)
    {
        int controlsNumber = CharacterSelection.Controls(int.Parse(textArray[3]));
        Shieldmaiden shieldmaiden = new Shieldmaiden(int.Parse(textArray[3]), controlsNumber, this);
        shieldmaiden.StartPosition = new Vector2(float.Parse(textArray[1]), float.Parse(textArray[2]));
        shieldmaiden.CurrentWeapon = new SwordAndShield(shieldmaiden);
        shieldmaiden.Reset();
        playerList.Add(shieldmaiden);
    }

    private void addHandleSummon(Monster monster, int objNumber)
    {
        foreach (var obj in objectList.Children)
            if (obj is Handle)
                if (((Handle)obj).ObjectNumberConnected == objNumber)
                {
                    ((Handle)obj).handleSummon.Add(monster);
                    return;
                }
    }

    /// <summary>
    /// Method that loads all tiles into the tilefield
    /// </summary>
    /// <param name="tileStringList">string list with tile information given from the main method</param>
    /// <returns></returns>
    protected GameObjectGrid LoadTiles(List<string> tileStringList)
    {
        // Throw an exception if the given Tile string list is null
        if(tileStringList == null)
        {
            throw new NullReferenceException("The given Tile list was null");
        }
        string[] lineArray = tileStringList[0].Split(',');
        // Make a new tile field with x being the length of a line (the length) and the amount of lines the y direction
        GameObjectGrid tileField = new GameObjectGrid(tileStringList.Count, lineArray.Length - 1, 3, "TileField")
        {
            //values for the cell width and height, these are predetermined in the Tiled Map Editor, so are constants.
            CellWidth = 50,
            CellHeight = 50
        };

        // Go through all given lines in the file, it is assumed here that it is in the correct form. 
        int[,] IDlist = new int[tileField.Columns, tileField.Rows];

        for(int y = 0; y < tileField.Rows; y++)
        {
            lineArray = tileStringList[y].Split(',');
            for(int x = 0; x < tileField.Columns; x++)
            {
                IDlist[x, y] = int.Parse(lineArray[x]); 
            }
        }

        // Go through each position in the ID list and give the correct tile for each ID, and in the end return the tileField to the invoker of this method
        for (int x = 0; x < tileField.Columns; x++)
        {
            for (int y = 0; y < tileField.Rows; y++)
            {
                
                Tile newtile;
                switch (IDlist[x, y])
                {
                    case 1:
                        newtile = new Tile(TileType.BasicTile, "Assets/Sprites/Tiles/BasicTile1"); // add the correct id here later this is just an example
                        break;
                    case 2:
                        newtile = new Tile(TileType.Brick, "Assets/Sprites/Tiles/TileBrick");
                        break;
                    case 3:
                        newtile = new Tile(TileType.Ice, "Assets/Sprites/Tiles/TileIce1");
                        break;
                    case 4:
                        newtile = new Tile(TileType.RockIce, "Assets/Sprites/Tiles/RockIce");
                        break;
                    case 5:
                        newtile = new Tile(TileType.Water, "Assets/Sprites/Tiles/TileWater1");
                        break;
                    case 6:
                        newtile = new Tile(TileType.CobbleStone, "Assets/Sprites/Tiles/TileCobbleStone1");
                        break;
                    case 7:
                        newtile = new Tile(TileType.Wood, "Assets/Sprites/Tiles/TileWood1");
                        break;
                    case 8:
                        newtile = new Tile(TileType.Grass, "Assets/Sprites/Tiles/TileGrass1");
                        break;
                    case 99:
                        newtile = new Tile(TileType.BasicTile, "Assets/Sprites/Tiles/BasicTile1");
                        break;
                    default: throw new NullReferenceException("the given ID " + IDlist[x, y] + " was not found in the preprogrammed IDs");
                }
              
                tileField.Add(newtile, x, y);
            }
        }
        return tileField;
    }

    private void HintTextDisplayer(List<string> text)
    {
        GameObjectList hintField = new GameObjectList(100);
        Add(hintField);
        string hint = text[1];
        //SpriteGameObject hintFrame = new SpriteGameObject("Overlays/spr_frame_hint", 1);
        hintField.Position = new Vector2((GameEnvironment.Screen.X /*- hintFrame.Width*/) / 2, 10);
        //hintField.Add(hintFrame);
        TextGameObject hintText = new TextGameObject("Assets/Fonts/ConversationFont.spritefont", 100);
        hintText.Text = hint;
        hintText.Position = new Vector2(120, 25);
        hintText.Color = Color.Black;
        hintField.Add(hintText);
    }

}
