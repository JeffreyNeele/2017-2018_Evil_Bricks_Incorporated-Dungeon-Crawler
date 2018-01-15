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


    // Returns the walkingdirection of a player
    public Vector2 WalkingDirection(int playernumber)
    {
        float deadzone = 0.25f;
        Vector2 stickInput = CurrentState(playernumber).ThumbSticks.Left;
        float magnitude = (float)Math.Sqrt(stickInput.X * stickInput.X + stickInput.Y * stickInput.Y);
        if (magnitude < deadzone)
            stickInput = Vector2.Zero;
        return stickInput;
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

