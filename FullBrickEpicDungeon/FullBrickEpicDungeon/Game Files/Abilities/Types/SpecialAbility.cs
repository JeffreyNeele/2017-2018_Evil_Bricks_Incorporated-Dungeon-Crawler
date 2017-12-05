using Microsoft.Xna.Framework;

// Abillity that maintains a cost and a treshold, specialmeter will be increased by monsters in the weapon class
class SpecialAbility : Ability
{
    protected int specialMeter, treshold, cost;
    protected SpecialAbility(Character owner, ClassType type, int treshold = 100, int cost = 100) : base(owner, type)
    {
        this.treshold = treshold;
        this.cost = cost;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }
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
