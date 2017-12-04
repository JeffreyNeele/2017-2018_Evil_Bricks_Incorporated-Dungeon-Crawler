using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

//Class will maintain the puzzle conditions and keep track of the puzzle status (cleared or not)
class Puzzle : GameObjectList
{
    bool puzzleCleared = false;
    List<bool> puzzleConditions = new List<bool>();

    public Puzzle()
    {

    }

    public bool puzzleStatus
    {
        get { return puzzleCleared; }
    }
}

