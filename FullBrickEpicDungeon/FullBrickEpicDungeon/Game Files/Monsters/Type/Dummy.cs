using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

class Dummy : Monster
{
    Vector2 speed;
    public Dummy(Vector2 movementSpeed, Vector2 dummyPosition, string assetName, Level level, string type = "dummy")
        : base(movementSpeed, assetName, type, level)
    {
        this.position = dummyPosition;
        speed = movementSpeed;
        this.baseattributes.HP = 50;
        this.baseattributes.Armour = 0;
        this.baseattributes.Gold = 0;
        this.baseattributes.Attack = 0;
        attributes = baseattributes;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }
}

