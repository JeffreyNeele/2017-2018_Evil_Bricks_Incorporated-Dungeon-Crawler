using System.Collections.Generic;
using Microsoft.Xna.Framework;

abstract partial class Monster : AnimatedGameObject
{
    protected BaseAttributes attributes, baseattributes;
    protected Level currentLevel;
    protected Vector2 startPosition;
    // List for checking what players the monster last hit
    protected List<Character> playersHit;
    // semi timer that checks if the monster should be flashing from taking damage
    protected float hitCounter;
    // the ID of the current animation
    protected string idAnimation;
    /// <summary>
    /// Class that defines a simple monster that has no AI
    /// </summary>
    /// <param name="id">the ID of the monster</param>
    /// <param name="currentLevel">current level the monster is in</param>
    public Monster(string id, Level currentLevel) : base(0, id)
    {
        this.currentLevel = currentLevel;
        playersHit = new List<Character>();
        attributes = new BaseAttributes();
        baseattributes = new BaseAttributes();
        hitCounter = 0;
        idAnimation = "idle";
    }

    /// <summary>
    /// Updates the monster
    /// </summary>
    /// <param name="gameTime">current game time</param>
    public override void Update(GameTime gameTime)
    {
        if (!IsDead)
        {
            base.Update(gameTime);
            // If the monster was hit, display a visual effect
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
            // If the monster is dead, make it slowly fade and then remove it
            if (this.color.A - 10 < 0)
            {
                GameObjectList monsterList = currentLevel.GameWorld.Find("monsterLIST") as GameObjectList;
                monsterList.Remove(this);
            }
            else
                this.color.A -= 10;
        }
        
        // Check for attack collisions
        if(idAnimation == "attack")
        {
            if (!CurrentAnimation.AnimationEnded)
                AnimationCheck();
            else
            {
                PlayAnimation("idle");
                idAnimation = "idle";
            }    
        }
    }

    /// <summary>
    /// Resets the Monster
    /// </summary>
    public override void Reset()
    {
        this.position = startPosition;
        base.Reset();
    }

    /// <summary>
    /// Makes the monster take damage
    /// </summary>
    /// <param name="damage">amount of damage received</param>
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

    /// <summary>
    /// Attacks a player
    /// </summary>
    public virtual void Attack()
    {
        playersHit.Clear();
        PlayAnimation("attack");
        idAnimation = "attack";
    }

    /// <summary>
    /// If an attack hit a player, add this player to the recently damaged players and force it to take damage
    /// </summary>
    /// <param name="player">the player that was hit</param>
    protected virtual void AttackHit(Character player)
    {
        if (!playersHit.Contains(player))
        {
            playersHit.Add(player);
            player.TakeDamage(attributes.Attack);
        }
    }
    
    /// <summary>
    /// Checks if the monster (if it is currently playing the attack animation) hit a player
    /// </summary>
    protected virtual void AnimationCheck()
    {
        GameObjectList players = currentLevel.GameWorld.Find("playerLIST") as GameObjectList;

        foreach (Character player in players.Children)
            if (BoundingBox.Intersects(player.BoundingBox))
                AttackHit(player);
    }

    /// <summary>
    /// Monster properties
    /// </summary>
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