using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

/// <summary>
/// Class that specifies the BossPenguin
/// </summary>
class BossPenguin : AImonster
{
    float movementSpeed;

    /// <summary>
    /// Make a new BossPenguin monster object
    /// </summary>
    /// <param name="currentLevel">current level the penguin is in</param>
    public BossPenguin(Level level) : base(0, level, "BossPenguin", 2000)
    {
        //Attributes
        this.baseattributes.HP = 850;
        this.baseattributes.Armour = 0;
        this.baseattributes.Attack = 0;
        this.baseattributes.Gold = 500;
        attributes.HP = baseattributes.HP;
        attributes.Armour = baseattributes.Armour;
        attributes.Attack = baseattributes.Attack;
        attributes.Gold = baseattributes.Gold;

        //Movementspeed of the BossPenguin
        movementSpeed = 0f;
        AI.RandomTargeting = true;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }
}

