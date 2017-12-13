using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

class Lightbringer : Character
{
    public Lightbringer() : base(ClassType.Lightbringer, "Sprites/Lightbringer/lightbringer_default", "Lightbringer")
    {
        // Loads the idle animation
        LoadAnimation("Sprites/Lightbringer/lightbringer_idle@3", "lightbringer_idle", true, 0.33F);
        // sets this characters base attributes, might be set in level later but for now it is in this constructors example.
        this.attributes.HP = 0;
        this.attributes.Armour = 0;
        this.attributes.Attack = 0;
        this.attributes.Gold = 0;
    }
}

