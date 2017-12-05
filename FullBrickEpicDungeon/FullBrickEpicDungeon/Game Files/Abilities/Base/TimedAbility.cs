
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

    public override void Use()
    {
        this.abilityTimer.Reset();
        base.Use();
    }
    public bool isOnCooldown
    {
        get { return !(abilityTimer.IsExpired); }
        set { abilityTimer.IsExpired = !value; }
    }
    public int TimeLeftUntilUse
    {
        get { return (int)(abilityTimer.MaxTime - abilityTimer.SecondsElapsed); }
    }
}