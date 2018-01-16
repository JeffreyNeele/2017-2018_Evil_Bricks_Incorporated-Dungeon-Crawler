using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

/// <summary>
/// The shieldBash ability is a knockback ability and shall use the push methodes defined in the super class Ability.
/// Is the main ability of the shieldMaiden
/// </summary>
class ShieldBashAbility : TimedAbility
{
    /// <summary>
    /// </summary>
    /// <param name="owner">Defines the owner of the ability</param>
    /// <param name="damage">Defines the damage of the ability</param>
    /// <param name="targetTime">Defines the cooldown of the ability</param>
    public ShieldBashAbility(Character owner, int damage, float targetTime)
        : base(owner, targetTime)
    {
        pushVector = new Vector2(60, 0);
        PushFallOff = new Vector2(2, 0);
        PushTimeCount = 16;
        DamageAA = damage;
    }

    public override void AttackHit(Monster monster, GameObjectGrid field)
    {
        fieldGrid = field;
        if (!MonsterHit.Contains(monster) && monster.Attributes.HP > 0 && MonsterHit.Count == 0)
        {
            monster.TakeDamage(DamageAA);
            MonsterAdd(monster, Owner.Mirror);
        }
    }
}
