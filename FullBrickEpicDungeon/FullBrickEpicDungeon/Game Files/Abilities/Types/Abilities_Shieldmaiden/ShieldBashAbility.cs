using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

class ShieldBashAbility : TimedAbility
{
    public ShieldBashAbility(Character owner, ClassType classType, Weapon weapon, string assetName, string id, float targetTime)
        : base(owner, classType, targetTime)
    {
        //weapon.LoadAnimation(assetName, id, false);
        pushVector = new Vector2(100, 0);
        this.damageAA = 40;
    }

    public override void Use(Weapon weapon, string idAbility)
    {
        this.monstersHitList = new List<Monster>();
        base.Use(weapon, idAbility);
    }

    public override void attackHit(Monster monster)
    {
        if (!monsterHit.Contains(monster) && monster.Attributes.HP > 0 && monsterHit.Count == 0)
        {
            monster.TakeDamage(damageAA);
            if (Owner.Mirror)
                monster.Position += pushBackVector;
            else
                monster.Position -= pushBackVector;
            monsterAdd(monster);
        }
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }
}
