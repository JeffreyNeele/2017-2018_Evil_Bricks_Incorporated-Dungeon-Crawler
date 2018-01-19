using System;
using Microsoft.Xna.Framework;

// This is ka timed ability that maintains a timer.
/// <summary>
/// Class that defines the attributes of abilities with a cooldown
/// </summary>
abstract class TimedAbility : Ability
{
    protected float targetTime;
    Timer abilityTimer;
    /// <summary>
    /// </summary>
    /// <param name="owner">Defines the owner of the ability</param>
    /// <param name="targetTime">The cooldown time of the ability</param>
    protected TimedAbility(Character owner, float targetTime) : base(owner)
    {
        this.targetTime = targetTime;
        this.abilityTimer = new Timer(targetTime);
        abilityTimer.SecondsElapsed = targetTime;
        this.IsOnCooldown = false;
    }

    // Resets the ability and makes it instantly usable again
    public override void Reset()
    {
        this.IsOnCooldown = false;
        base.Reset();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        abilityTimer.Update(gameTime);
    }

    // Uses the ability (resets the timer to 0)
    public override void Use()
    {
        this.abilityTimer.Reset();
        base.Use();
    }
    // Sets the IsExpired value in ability so it is either in cooldown or not
    public bool IsOnCooldown
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