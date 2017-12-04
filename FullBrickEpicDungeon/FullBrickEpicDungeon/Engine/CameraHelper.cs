using Microsoft.Xna.Framework;

// Class that mantains an internal offset for the actual camera for easy camera adjustment
public class CameraHelper
{
    Vector2 windowSize, position;
    public CameraHelper(Vector2 windowSize)
    {
        this.position = Vector2.Zero;
    }
    public void Reset(Vector2 playerPosition, Vector2 gameSize, Point screenSize)
    {
        this.position = Vector2.Zero;
    }
    // Place the position of the Camera on the middle of the screen
    public void Center()
    {
        this.position = new Vector2(windowSize.X / 2, windowSize.Y / 2);
    }
    public Vector2 CameraOffset
    {
        get { return position; }
        set { position = value; }
    }
    public Vector2 WindowSize
    {
        get { return windowSize; }
        set { windowSize = value; }
    }
}
