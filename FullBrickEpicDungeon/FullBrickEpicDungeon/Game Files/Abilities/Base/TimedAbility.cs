using System;
using Microsoft.Xna.Framework;

// This is ka timed ability that maintains a timer.
abstract class TimedAbility : Ability
{
    protected float targetTime;
    Timer abilityTimer;
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
    public override void Use(Weapon weapon, string idAbility)
    {
        this.abilityTimer.Reset();
        base.Use(weapon, idAbility);
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