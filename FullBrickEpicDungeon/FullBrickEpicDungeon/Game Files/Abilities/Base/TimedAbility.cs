
using Microsoft.Xna.Framework;

// This is ka timed ability that maintains a timer.
abstract class TimedAbility : Ability
{
    protected int targetTime;
    Timer abilityTimer;
    protected TimedAbility(Character owner, ClassType type, int targetTime) : base(owner, type)
    {
        this.targetTime = targetTime;
        abilityTimer = new Timer(targetTime);
        this.isOnCooldown = false;
    }

    // Resets the ability and makes it instantly usable again
    public override void Reset()
    {
        this.isOnCooldown = false;
        base.Reset();
    }
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    // Uses the ability (resets the timer to 0)
    public override void Use()
    {
        this.abilityTimer.Reset();
        base.Use();
    }
    // Sets the IsExpired value in ability so it is either in cooldown or not
    public bool isOnCooldown
    {
        get { return !(abilityTimer.IsExpired); }
        set { abilityTimer.IsExpired = !value; }
    }
    // returns time left until use (usefful for the hud programmers ;) )
    public int TimeLeftUntilUse
    {
        get { return (int)(abilityTimer.MaxTime - abilityTimer.SecondsElapsed); }
    }
}