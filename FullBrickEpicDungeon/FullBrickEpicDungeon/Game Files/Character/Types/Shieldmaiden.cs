using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

class Shieldmaiden : Character
{
    public Shieldmaiden() : base(ClassType.ShieldMaiden, "Assets/Sprites/Shieldmaiden/shieldmaiden_default", "Shieldmaiden")
    {
        // Loads the idle animation
        // sets this characters base attributes, might be set in level later but for now it is in this constructors example.
        this.baseattributes.HP = 200;
        this.baseattributes.Armour = 50;
        this.baseattributes.Attack = 10;
        this.baseattributes.Gold = 0;
        attributes = baseattributes;
        LoadAnimation("Assets/Sprites/Shieldmaiden/shieldmaiden_default", "test", true);
        //LoadAnimation();      Gereserveerd voor base AA animation van de shieldMaiden
        weapon = new SwordAndShield(this);
        PlayAnimation("test");
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }
}

