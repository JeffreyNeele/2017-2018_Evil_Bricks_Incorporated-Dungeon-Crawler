using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

class Dummy : Monster
{
    public Dummy(string assetName, Level currentLevel, string type = "dummy")
        : base(type, currentLevel)
    {
        this.baseattributes.HP = 50;
        this.baseattributes.Armour = 0;
        this.baseattributes.Gold = 0;
        this.baseattributes.Attack = 0;
        attributes.HP = baseattributes.HP;
        attributes.Armour = baseattributes.Armour;
        attributes.Attack = baseattributes.Attack;
        attributes.Gold = baseattributes.Gold;
        LoadAnimation(assetName, "default", false);
        PlayAnimation("default");
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

}

