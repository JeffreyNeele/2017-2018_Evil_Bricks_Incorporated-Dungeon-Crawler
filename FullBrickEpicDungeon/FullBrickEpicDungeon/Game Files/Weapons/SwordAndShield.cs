using Microsoft.Xna.Framework;

/// <summary>
/// Main and starting weapon of the ShieldMaiden
/// </summary>
class SwordAndShield : Weapon
{
    /// <summary>
    /// </summary>
    /// <param name="owner">Character that owns this weapon</param>
    public SwordAndShield(Character owner) : base(owner, "Swordandshield", "putassetnamehere")
    {
        // properties for attack and amount of gold the weapon costs
        AttackDamage = 5;
        GoldWorth = 0;

        LoadAnimation("Assets/Sprites/Weapons/SwordUp", "attack_up", false, 0.2f);
        LoadAnimation("Assets/Sprites/Weapons/SwordDown", "attack_down", false, 0.2f);
        LoadAnimation("Assets/Sprites/Weapons/SwordLeft", "attack_left", false, 0.2f);
        LoadAnimation("Assets/Sprites/Weapons/SwordRight", "attack_right", false, 0.2f);

        // IDs for the abilities this weapon has
        idBaseAA = "SwordAndShieldAA";
        idMainAbility = "SwordAndShieldMainAbility";
        idSpecialAbility = "SwordAndShieldSpecialAbility";

        //Basic attack of the weapon
        BasicAttack = new BasicAttackAbility(owner, AttackDamage);
        BasicAttack.PushBackVector = new Vector2(30, 0);
        BasicAttack.PushFallOff = new Vector2(2, 0);
        BasicAttack.PushTimeCount = 8;

        //Basic ability of the weapon: ShieldBash
        mainAbility = new ShieldBashAbility(owner, AttackDamage * 4, 8);
        PlayAnimation("attack_up");
    }

    //Method that checks collision for the mainAbility with the monsters
    public override void AnimationMainCheck()
    {
        bool hit = false;
        foreach (Monster m in monsterObjectList.Children)
        {
            if (mainAbility.MonsterHit.Count < 1 && m.BoundingBox.Intersects(this.BoundingBox))
            {
                mainAbility.AttackHit(m, fieldList);
                hit = true;
            }
        }
        if (hit)
            prevAttackHit = true;
        else
            prevAttackHit = false;
    }
}
