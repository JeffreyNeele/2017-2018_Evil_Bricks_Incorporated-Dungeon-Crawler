using Microsoft.Xna.Framework;

// A class that maintains a timer
class Timer
{
    private float targettime, currentime;
    private bool paused;
    protected bool expired;
    public Timer(float targettime)
    {
        this.targettime = targettime;
        this.currentime = 0;
        paused = true;
        expired = false;
    }

    // Increases the time
    public void Update(GameTime gameTime)
    {
        if (expired)
        {
            IsPaused = true;
        }
        else if (!paused)
        {
            expired = currentime >= targettime;
            currentime += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

    }


    //Resets the timer
    public void Reset()
    {
        currentime = 0;
        IsPaused = false;
        expired = false;
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
        set { currentime = value; }
    }
    public bool IsExpired
    {
        get { return expired; }
        set { expired = value; }
    }

}