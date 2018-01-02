using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

class BasicAttackAbility : Ability
{
    bool knockback;
    public BasicAttackAbility(Character owner, ClassType classType, Weapon weapon, string assetName, string id, int damage, bool knockBack) 
        : base(owner, classType)
    {
        //weapon.LoadAnimation(assetName, id, false);
        damageAA = damage;
        this.knockBack = knockBack;
        pushVector = new Vector2(0, 0);
    }

    public override void Use(Weapon weapon, string id)
    {
        this.monstersHitList = new List<Monster>();
        base.Use(weapon, id);
    }

    public override void attackHit(Monster monster)
    {
        if(!monsterHit.Contains(monster) && monster.Attributes.HP > 0)
        {
            monster.TakeDamage(damageAA);
            if (this.knockBack)
                if (Owner.Mirror)
                    monster.Position += pushBackVector;
                else
                    monster.Position -= pushBackVector;
            monsterAdd(monster);
        }
    }

    public bool knockBack
    {
        get { return knockback; }
        set { knockback = value; }
    }
}

