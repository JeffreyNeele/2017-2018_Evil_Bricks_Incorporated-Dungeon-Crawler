using Microsoft.Xna.Framework;

class Timer
{
    private float targettime, currentime;
    private bool paused;
    public Timer(float targettime)
    {
        this.targettime = targettime;
        currentime = 0;
        paused = false;
    }

    private void Update(GameTime gameTime)
    {
        if (!paused)
        {
            currentime += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }

    public bool IsExpired()
    {
        return currentime >= targettime;
    }

    public void Reset()
    {
        currentime = 0;
    }

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
}