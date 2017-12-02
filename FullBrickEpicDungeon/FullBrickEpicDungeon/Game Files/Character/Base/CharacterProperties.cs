using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

abstract partial class Character : AnimatedGameObject
{
    // Main attributes of the character
    public int HP
    {
        get { return hitpoints; }
        set { hitpoints = value; }
    }
    public int Armour
    {
        get { return armour; }
        set { armour = value; }
    }
    public int Gold
    {
        get { return gold; }
        set { gold = value; }
    }
}
