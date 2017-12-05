
using Microsoft.Xna.Framework;

// This is ka timed ability that maintains a timer.
abstract class TimedAbility : Ability
{
    Timer abilityTimer;
    protected TimedAbility(Character owner, ClassType type, int targetTime) : base(owner, type)
    {
        abilityTimer = new Timer(targetTime);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    public bool isOnCooldown
    {
        get { return !(abilityTimer.IsExpired); }
    }
    public int TimeLeftUntilUse
    {
        get { return (int)(abilityTimer.MaxTime - abilityTimer.SecondsElapsed); }
    }
}