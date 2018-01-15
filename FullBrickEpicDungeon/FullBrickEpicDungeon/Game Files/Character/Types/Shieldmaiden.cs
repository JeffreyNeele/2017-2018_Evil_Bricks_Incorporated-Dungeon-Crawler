using Microsoft.Xna.Framework;
using System;

class Shieldmaiden : Character
{
    public Shieldmaiden(int playerNumber, Level currentLevel, bool xboxcontrolled = false) : base(playerNumber, currentLevel, xboxcontrolled, ClassType.ShieldMaiden, "Shieldmaiden")
    {
        // Loads the idle animation
        // sets this characters base attributes, might be set in level later but for now it is in this constructors example.
        this.baseattributes.HP = 200;
        this.baseattributes.Armour = 50;
        this.baseattributes.Attack = 10;
        this.baseattributes.Gold = 0;
        attributes = baseattributes;
        weapon = new SwordAndShield(this);
        //Healthbar healthbar = new Healthbar(content, this.position);
        string playerColor;
        switch (playerNumber)
        {
            case 1: playerColor = "default";
                break;
            case 2: playerColor = "orange";
                break;
            case 3: playerColor = "green";
                break;
            case 4: playerColor = "blue";
                break;
            default:
                throw new IndexOutOfRangeException("playerNumber was not between 1 and 4, the given number was: " + playerNumber);
        }

        LoadAnimation("Assets/Sprites/Shieldmaiden/shieldmaiden_" + playerColor, "idle", false);
        LoadAnimation("Assets/Sprites/Shieldmaiden/shieldmaiden_walk_back_" + playerColor + "@4", "backcycle", true, 0.2F);
        LoadAnimation("Assets/Sprites/Shieldmaiden/shieldmaiden_walk_front_" + playerColor + "@4", "frontcycle", true, 0.2F);
        LoadAnimation("Assets/Sprites/Shieldmaiden/shieldmaiden_walk_left_" + playerColor + "@4", "leftcycle", true, 0.2F);
        LoadAnimation("Assets/Sprites/Shieldmaiden/shieldmaiden_walk_right_" + playerColor + "@4", "rightcycle", true, 0.2F);
        PlayAnimation("idle");

        characterSFX.Add("attack_hit", "Assets/SFX/Shieldmaiden/sword_hit");
        characterSFX.Add("attack_miss", "Assets/SFX/Shieldmaiden/sword_miss");
        characterSFX.Add("basic_ability", "Assets/SFX/Shieldmaiden/hit_shieldbash");
        characterSFX.Add("walk", "Assets/SFX/Shieldmaiden/walk");
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }
}

