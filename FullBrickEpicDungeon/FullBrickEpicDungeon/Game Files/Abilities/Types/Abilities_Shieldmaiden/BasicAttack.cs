using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

class BasicAttackAbility : Ability
{

    public BasicAttackAbility(Character owner, ClassType classType, Weapon weapon, string assetName, string id, int damage) 
        : base(owner, classType)
    {
        //weapon.LoadAnimation(assetName, id, false);
        damageAA = damage;
        pushVector = new Vector2(0, 0);
    }

    public override void Use(Weapon weapon, string id)
    {
        base.Use(weapon, id);
    }

    public override void attackHit(Monster monster, GameObjectGrid field)
    {
        fieldGrid = field;
        if(!monsterHit.Contains(monster) && monster.Attributes.HP > 0)
        {
            monster.TakeDamage(damageAA);
            monsterAdd(monster, Owner.Mirror);
        }
    }
}

