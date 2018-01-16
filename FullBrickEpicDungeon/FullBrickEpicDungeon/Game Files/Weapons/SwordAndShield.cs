using Microsoft.Xna.Framework;
class SwordAndShield : Weapon
{
    /// <summary>
    /// Main weapon for the ShieldMaiden
    /// </summary>
    /// <param name="owner">Character that owns this weapon</param>
    public SwordAndShield(Character owner) : base(owner, "Swordandshield", "putassetnamehere")
    {
        // properties for attack and amount of gold the weapon costs
        AttackDamage = 10;
        GoldWorth = 0;

        LoadAnimation("Assets/Sprites/Weapons/SwordUp", "attack_up", false);
        LoadAnimation("Assets/Sprites/Weapons/SwordDown", "attack_down", false);
        LoadAnimation("Assets/Sprites/Weapons/SwordLeft", "attack_left", false);
        LoadAnimation("Assets/Sprites/Weapons/SwordRight", "attack_right", false);

        // IDs for the abilities this weapon has
        idBaseAA = "SwordAndShieldAA";
        idMainAbility = "SwordAndShieldMainAbility";
        idSpecialAbility = "SwordAndShieldSpecialAbility";

        // Basic attack of the weapon
        BasicAttack = new BasicAttackAbility(owner, this, "assetName", idBaseAA, AttackDamage)
        {
            PushBackVector = new Vector2(30, 0),
            PushFallOff = new Vector2(2, 0),
            PushTimeCount = 8
        };
        //Basic ability of the weapon: ShieldBash
        mainAbility = new ShieldBashAbility(owner, this, "assetName", idMainAbility, 8);

        AttackDirection = "Up";
        PlayAnimation("attack_up");
    }

    /// <summary>
    /// Method for the basic attack of the weapon
    /// </summary>
    /// <param name="monsterList">List of monsters in the current level</param>
    /// <param name="field">Gridfield of the level</param>
    public override void Attack(GameObjectList monsterList, GameObjectGrid field)
    {
        base.Attack(monsterList, field);
        monsterObjectList = monsterList;
        fieldList = field;
        AnimationAttackCheck();
        BasicAttack.Use(this, idBaseAA);
        idAnimation = idBaseAA;
    }


    /// <summary>
    /// Method for using the main ability of this weapon
    /// </summary>
    /// <param name="monsterList">List of monsters in the current level</param>
    /// <param name="field">Gridfield of the level</param>
    public override void UseMainAbility(GameObjectList monsterList, GameObjectGrid field)
    {
        if(!mainAbility.IsOnCooldown)
        {
            this.mainAbility.Use(this, idMainAbility);
            this.idAnimation = idMainAbility;
            monsterObjectList = monsterList;
            fieldList = field;
            AnimationMainCheck();
        }
    }

    /// <summary>
    /// Main Ability collisiopn checker
    /// </summary>
    public void AnimationMainCheck()
    {
        foreach (Monster m in monsterObjectList.Children)
        {
            if (mainAbility.MonsterHit.Count < 1 && m.CollidesWith(Owner))
                mainAbility.AttackHit(m, fieldList);
        }
    }

    /// <summary>
    /// Updates the Shield and sword weapon
    /// </summary>
    /// <param name="gameTime">current gameTime</param>
    public override void Update(GameTime gameTime)
    {
        BasicAttack.Update(gameTime);
        mainAbility.Update(gameTime);

        base.Update(gameTime);
    }

}
