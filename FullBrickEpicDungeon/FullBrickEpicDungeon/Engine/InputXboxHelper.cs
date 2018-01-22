using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

// Class that handles XBOX input
public partial class InputHelper
{
    // Define gamepadstates for each player
    protected GamePadState currGamePadState1, currGamePadState2, currGamePadState3, currGamePadState4;
    protected GamePadState prevGamePadState1, prevGamePadState2, prevGamePadState3, prevGamePadState4;


    // Updates the XBOX Gamepadstates
    void UpdateXbox()
    {
        prevGamePadState1 = currGamePadState1;
        prevGamePadState2 = currGamePadState2;
        prevGamePadState3 = currGamePadState3;
        prevGamePadState4 = currGamePadState4;

        currGamePadState1 = GamePad.GetState(0);
        currGamePadState2 = GamePad.GetState(1);
        currGamePadState3 = GamePad.GetState(2);
        currGamePadState4 = GamePad.GetState(3);
    }


    // Returns a boolean that represents if a controlelr is connected
    public bool ControllerConnected(int playernumber)
    {
        if (CurrentState(playernumber).IsConnected)
            return true;
        else
        {
            return false;
        }
    }

    // Returns if a button of a controller with a certain player number is pressed
    public bool ButtonPressed(int playernumber, Buttons k)
    {
        return CurrentState(playernumber).IsButtonDown(k) && PreviousState(playernumber).IsButtonUp(k);
    }

    public bool AnyPlayerPressed(Buttons k)
    {
        for(int i = 1; i <= 4; i++)
        {
            if(CurrentState(i).IsButtonDown(k) && PreviousState(i).IsButtonUp(k))
            {
                return true;
            }
        }
        return false;
    }

    public bool IsButtonDown(int playernumber, Buttons k)
    {
        return CurrentState(playernumber).IsButtonDown(k);
    }

    // returns if the input of a playernumber has changed
    public bool HasInputChanged(int playernumber, bool ignoreThumbsticks)
    {
        GamePadState currentState = CurrentState(playernumber);
        GamePadState previousGamePadState = PreviousState(playernumber);

        if ((currentState.IsConnected) && (currentState.PacketNumber != previousGamePadState.PacketNumber))
        {
            //ignore thumbstick movement
            if ((ignoreThumbsticks == true) && ((currentState.ThumbSticks.Left.Length() != previousGamePadState.ThumbSticks.Left.Length()) && (currentState.ThumbSticks.Right.Length() != previousGamePadState.ThumbSticks.Right.Length())))
                return false;
            return true;
        }
        return false;
    }


    //gives continuous vector2 output of left thumbstick
    public Vector2 WalkingDirection(int playernumber)
    {
        float deadzone = 0.25f;
        Vector2 stickInput = CurrentState(playernumber).ThumbSticks.Left;
        float magnitude = (float)Math.Sqrt(stickInput.X * stickInput.X + stickInput.Y * stickInput.Y);
        if (magnitude < deadzone)
            stickInput = Vector2.Zero;
        return stickInput;
    }

    //gives previous vector2 output of left thumbstick
    public Vector2 PrevWalkingDirection(int playernumber)
    {
        //return CurrentState(playernumber).ThumbSticks.Left;
        float deadzone = 0.25f;
        Vector2 stickInput = PreviousState(playernumber).ThumbSticks.Left;
        float magnitude = (float)Math.Sqrt(stickInput.X * stickInput.X + stickInput.Y * stickInput.Y);
        if (magnitude < deadzone)
            stickInput = Vector2.Zero;
        return stickInput;
    }

    public Vector2 StraightDirection(int playernumber, Vector2 currprevdir)
    {
        //the first run it will run for the currentdirection, the second for the previousdirection
        Vector2 direction = currprevdir;
        Vector2 straightdirection = Vector2.Zero;


        if (Math.Abs(direction.X) >= Math.Abs(direction.Y))
        {
            if (direction.X > 0)
            {
                straightdirection = new Vector2(1, 0);
            }
            else if (direction.X < 0)
            {
                straightdirection = new Vector2(-1, 0);
            }
        }
        else if (Math.Abs(direction.Y) > Math.Abs(direction.X))
        {
            if (direction.Y > 0)
            {
                straightdirection = new Vector2(0, 1);
            }
            else if (direction.Y < 0)
            {
                straightdirection = new Vector2(0, -1);
            }
        }
        return straightdirection;
    }

    //gives vector2 4 directions only output of left thumbstick only once.
    public Vector2 MenuDirection(int playernumber, bool X, bool Y)
    {
         
        Vector2 previousDirection = PrevWalkingDirection(playernumber);
        Vector2 currentDirection = WalkingDirection(playernumber);

        previousDirection = StraightDirection(playernumber, previousDirection);
        currentDirection = StraightDirection(playernumber, currentDirection);

        if (X && !Y)
        {
            if (previousDirection.X != currentDirection.X) //direction is niet veranderd
                return currentDirection;
        }
        else if (!X && Y)
        {
            if (previousDirection.Y != currentDirection.Y) //direction is niet veranderd
                return currentDirection;
        }
        else if (X && Y)
        {
            if (previousDirection != currentDirection)
                return currentDirection;
        }
        
       return Vector2.Zero; //does never give output if both X an Y is not wanted






    }


    // Returns the currentstate
    private GamePadState CurrentState(int playernumber)
    {
        switch (playernumber)
        {
            case 1: return currGamePadState1;
            case 2: return currGamePadState2;
            case 3: return currGamePadState3;
            case 4: return currGamePadState4;
            default: throw new IndexOutOfRangeException("The playernumber entered is not between 1 and 4");
        }
    }

    // returns the previous state
    private GamePadState PreviousState(int playernumber)
    {
        switch (playernumber)
        {
            case 1: return prevGamePadState1;
            case 2: return prevGamePadState2;
            case 3: return prevGamePadState3;
            case 4: return prevGamePadState4;
            default: throw new IndexOutOfRangeException("The playernumber entered is not between 1 and 4");
        }
    }
}

