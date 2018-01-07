using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

// NOTE: the gold attribute for the monster is seen as the amount of gold the player gets when it is defeated.
abstract partial class Monster : AnimatedGameObject
{
    protected BaseAttributes attributes, baseattributes;
    protected BaseAI baseAI;
    protected Vector2 startPosition;
    protected List<Character> playersHit;
    protected float hitCounter;
    public Monster(string id, Level currentLevel) : base(0, id)
    {
        attributes = new BaseAttributes();
        baseattributes = new BaseAttributes();
        hitCounter = 0;
    }


    public override void Update(GameTime gameTime)
    {
        if (!IsDead)
        {
            base.Update(gameTime);
            if (hitCounter >= 0)
            {
                Visible = !Visible;
                hitCounter -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
                Visible = true;
        }
        else
        {
            if (this.color.A - 10 < 0)
            {
                GameObjectList monsterList = GameWorld.Find("monsterLIST") as GameObjectList;
                monsterList.Remove(this);
            }
                
            else
                this.color.A -= 10;
        }
    }

    public override void Reset()
    {
        this.position = startPosition;
        base.Reset();
    }

    //Takes damage, HP can never be below 0 because of health bars
    public void TakeDamage(int damage)
    {
        int takendamage = (damage - (int)(0.3F * this.attributes.Armour));
        if (takendamage < 5)
        {
            takendamage = 5;
        }
        this.attributes.HP -= takendamage;
        if (this.attributes.HP <= 0)
        {
            this.attributes.HP = 0;
        }
        else
            hitCounter = 0.5f;
    }

    //Method for the attack of the monster
    public virtual void Attack(Character player)
    {
        if (!playersHit.Contains(player))
        {
            playersHit.Add(player);
            player.TakeDamage(baseattributes.Attack);
        }
    }

    // returns the base attributes of a monster
    public BaseAttributes Attributes
    {
        get { return attributes; }
        set { attributes = value; }
    }

    public Vector2 StartPosition
    {
        get { return startPosition; }
        set { startPosition = value; }
    }

    public bool IsDead
    {
        get { return this.attributes.HP == 0; }
    }
}