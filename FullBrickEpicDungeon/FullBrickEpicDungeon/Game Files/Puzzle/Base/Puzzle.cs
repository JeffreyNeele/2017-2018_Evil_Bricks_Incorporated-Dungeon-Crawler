using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Klasse houdt de conditie van de puzzel bij en kijkt of de puzzel is afgerond
class Puzzle : GameObjectList
{
    bool puzzleCleared = false;

    public Puzzle()
    {

    }

    public bool puzzleStatus
    {
        get { return puzzleCleared; }
    }
}

