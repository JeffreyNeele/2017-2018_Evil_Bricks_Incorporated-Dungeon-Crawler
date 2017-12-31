using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

class Talismaniac : Character
{
    public Talismaniac(int playerNumber) : base(playerNumber, ClassType.TalisManiac, "Sprites/Lightbringer/lightbringer_default", "Lightbringer")
    {
        // Loads the idle animation
        LoadAnimation("Sprites/Lightbringer/lightbringer_idle@3", "lightbringer_idle", true, 0.33F);
        // sets this characters base attributes, might be set in level later but for now it is in this constructors example.
        this.attributes.HP = 100;
        this.attributes.Armour = 25;
        this.attributes.Attack = 20;
        this.attributes.Gold = 0;
    }

    public override void Update(GameTime gameTime)
    {
        PlayAnimation("lightbringer_idle");
    }
}

