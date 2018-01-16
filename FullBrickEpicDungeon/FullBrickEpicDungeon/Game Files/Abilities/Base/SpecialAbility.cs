using Microsoft.Xna.Framework;

/// <summary>
/// Abillity that maintains a cost and a treshold, specialmeter will be increased by monsters in the weapon class
/// </summary>
class SpecialAbility : Ability
{
    protected int specialMeter, treshold, cost;

    /// <summary>
    /// </summary>
    /// <param name="owner">Defines the owner of the ability</param>
    /// <param name="treshold">Defines the max amount of special meter points a player may hold at any given time</param>
    /// <param name="cost">Defines the amount of special meter points needed to use the special ability</param>
    protected SpecialAbility(Character owner, int treshold = 100, int cost = 100) : base(owner)
    {
        this.treshold = treshold;
        this.cost = cost;
    }
    
    //Will include more when a special ability will be implemented
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    /// <summary>
    /// Readies the appropriate attribute before using the special ability
    /// </summary>
    protected virtual void UseSpecial()
    {
        if (specialMeter >= cost)
        {
            specialMeter -= cost;
        }
        else
        {
            // TODO: add a not usable sound effect
            return;
        }
    }

    public override void AttackHit(Monster monster, GameObjectGrid field)
    {

    }

    public bool Usable
    {
        get { return specialMeter >= cost; }
    }
    // Makes it so that the specialmeter can be set but can never go above the treshold value
    public int SpecialMeter
    {
        get { return specialMeter; }
        set
        {
            if (value <= treshold)
                specialMeter = value;
            else
            {
                specialMeter = treshold;
            }
        }
    }
    public int SpecialCost
    {
        get { return cost; }
    }
}
