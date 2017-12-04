using Microsoft.Xna.Framework;

// A class that maintains a timer
class Timer
{
    private float targettime, currentime;
    private bool paused;
    public Timer(float targettime)
    {
        this.targettime = targettime;
        currentime = 0;
        paused = true;
    }

    // Increases the time
    private void Update(GameTime gameTime)
    {
        if (!paused)
        {
            currentime += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }


    //Resets the timer
    public void Reset()
    {
        currentime = 0;
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
        set { targettime = value; }
    }
    public float SecondsElapsed
    {
        get { return currentime; }
    }
    public bool IsExpired
    {
        get { return currentime >= targettime; }
    }

}