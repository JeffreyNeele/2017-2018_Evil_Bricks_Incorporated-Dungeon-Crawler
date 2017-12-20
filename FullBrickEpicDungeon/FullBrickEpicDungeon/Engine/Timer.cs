using Microsoft.Xna.Framework;

// A class that maintains a timer
class Timer
{
    private float targettime, currentime;
    private bool paused;
    public Timer(float targettime)
    {
        this.targettime = targettime;
        paused = true;
    }

    // Increases the time
    public void Update(GameTime gameTime)
    {
        if (!paused)
        {
            currentime += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        if (IsExpired)
        {
            IsPaused = true;
        }
    }


    //Resets the timer
    public void Reset()
    {
        currentime = 0;
        IsPaused = false;
    }

    // Properties for the timer
    public bool IsPaused
    {
        get { return paused; }
        set { paused = value; }
    }
    public float MaxTime
    {
        get { return targettime; }
    }
    public float SecondsElapsed
    {
        get { return currentime; }
    }
    public bool IsExpired
    {
        get { return currentime >= targettime; }
        set { IsExpired = value; }
    }

}