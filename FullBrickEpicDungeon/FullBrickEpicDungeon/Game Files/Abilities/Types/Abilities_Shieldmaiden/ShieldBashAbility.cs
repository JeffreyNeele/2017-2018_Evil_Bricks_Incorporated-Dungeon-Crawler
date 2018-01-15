using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

class ShieldBashAbility : TimedAbility
{
    public ShieldBashAbility(Character owner, Weapon weapon, string assetName, string id, float targetTime)
        : base(owner, targetTime)
    {
        //weapon.LoadAnimation(assetName, id, false);
        pushVector = new Vector2(60, 0);
        PushFallOff = new Vector2(2, 0);
        PushTimeCount = 16;
        DamageAA = 40;
    }

    public override void Use(Weapon weapon, string idAbility)
    {
        base.Use(weapon, idAbility);
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

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }
}
