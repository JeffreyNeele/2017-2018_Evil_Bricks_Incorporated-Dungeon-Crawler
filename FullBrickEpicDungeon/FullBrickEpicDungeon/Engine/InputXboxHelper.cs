using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;


public partial class InputHelper
{
    protected GamePadState currGamePadState1, currGamePadState2, currGamePadState3, currGamePadState4;
    protected GamePadState prevGamePadState1, prevGamePadState2, prevGamePadState3, prevGamePadState4;


    /* XBOX CONTROLLER UPDATE */

    void UpdateXbox()
    {
        prevGamePadState1 = currGamePadState1; //oorspronkelijk currGamePadState1
        prevGamePadState2 = currGamePadState2;
        prevGamePadState3 = currGamePadState3;
        prevGamePadState4 = currGamePadState4;

        currGamePadState1 = GamePad.GetState(0);
        currGamePadState2 = GamePad.GetState(1);
        currGamePadState3 = GamePad.GetState(2);
        currGamePadState4 = GamePad.GetState(3);
    }

    public bool ControllerConnected(int playernumber) //misschien overbodig
    {
        if (CurrentState(playernumber).IsConnected)
            return true;
        else
        {
            Console.WriteLine("A Controller has been disconnected!");
            return false;
        }
    }

   public bool ButtonPressed(int playernumber, Buttons k)
    {
       // Console.WriteLine(PreviousState(playernumber).IsButtonUp(k));
        return CurrentState(playernumber).IsButtonDown(k) && PreviousState(playernumber).IsButtonUp(k);
    }

    public bool IsButtonDown(int playernumber, Buttons k)
    {
        return CurrentState(playernumber).IsButtonDown(k);
    }



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



    public Vector2 WalkingDirection(int playernumber)
    {
        return CurrentState(playernumber).ThumbSticks.Left;
    }




    private GamePadState CurrentState (int playernumber)
    {
        switch(playernumber)
        {
            case 1: return currGamePadState1;
            case 2: return currGamePadState2;
            case 3: return currGamePadState3;
            case 4: return currGamePadState4;
            default: throw new IndexOutOfRangeException("The playernumber entered is not between 1 and 4");
        }
    }


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

