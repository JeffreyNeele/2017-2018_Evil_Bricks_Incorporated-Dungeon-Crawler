using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

class Shieldmaiden : Character
{
    public Shieldmaiden() : base(ClassType.ShieldMaiden, "Sprites/Lightbringer/lightbringer_default", "Lightbringer")
    {
        // Loads the idle animation
        LoadAnimation("Sprites/Lightbringer/lightbringer_idle@3", "lightbringer_idle", true, 0.33F);
        // sets this characters base attributes, might be set in level later but for now it is in this constructors example.
        this.attributes.HP = 200;
        this.attributes.Armour = 50;
        this.attributes.Attack = 10;
        this.attributes.Gold = 0;
    }

    public override void Update(GameTime gameTime)
    {
        PlayAnimation("lightbringer_idle");
    }
}

