using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

class BasicAttackAbility : Ability
{
    /// <summary>
    ///  Ability attack that does not have a cooldown. Is the primary attack of the character with its weapon
    /// </summary>
    /// <param name="owner">Defines the owner of the ability</param>
    /// <param name="assetName"></param>
    /// <param name="id"></param>
    /// <param name="damage"></param>
    public BasicAttackAbility(Character owner, int damage) 
        : base(owner)
    {
        DamageAA = damage;
        pushVector = new Vector2(0, 0);
        PushFallOff = new Vector2(0, 0);
    }
}

