
using Microsoft.Xna.Framework;

// This is ka timed ability that maintains a timer.
abstract class TimedAbility : Ability
{
    Timer abilityTimer;
    protected TimedAbility(int targetTime)
    {
        abilityTimer = new Timer(targetTime);
    }

    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    public bool isOnCooldown
    {
        get { return abilityTimer.IsExpired(); }
    }
    public int TimeLeft
    {
        get { return (int)(abilityTimer.MaxTime - abilityTimer.SecondsElapsed); }
    }
}