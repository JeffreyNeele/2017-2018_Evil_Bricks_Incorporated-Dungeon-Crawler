using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

class BasicAttackAbility : Ability
{

    public BasicAttackAbility(Character owner, Weapon weapon, string assetName, string id, int damage) 
        : base(owner)
    {
        //weapon.LoadAnimation(assetName, id, false);
        DamageAA = damage;
        pushVector = new Vector2(0, 0);
        PushFallOff = new Vector2(0, 0);
    }

    public override void Use(Weapon weapon, string id)
    {
        base.Use(weapon, id);
    }

    public override void AttackHit(Monster monster, GameObjectGrid field)
    {
        fieldGrid = field;
        if(!MonsterHit.Contains(monster) && monster.Attributes.HP > 0)
        {
            monster.TakeDamage(DamageAA);
            MonsterAdd(monster, Owner.Mirror);
        }
    }
}

