using Microsoft.Xna.Framework;

class SpecialAbility : Ability
{
    protected int specialMeter, treshold, cost;
    protected SpecialAbility(string type, int treshold = 100, int cost = 100) : base(type)
    {
        this.treshold = treshold;
        this.cost = cost;
    }

    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }
    protected virtual void UseSpecial()
    {
        specialMeter -= cost;
    }

    public bool Usable
    {
        get { return specialMeter >= cost; }
    }
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
