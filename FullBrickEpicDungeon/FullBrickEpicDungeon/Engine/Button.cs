using System;
using System.Collections.Generic;

class Button : SpriteGameObject
{
    protected bool isPressed;
    public Button(string AssetName, int layer = 0, string id = "") : base(AssetName, layer, id)
    {
        isPressed = false;
    }

    //This Method checks if the button was pressed
    public override void HandleInput(InputHelper inputHelper)
    {
        isPressed = inputHelper.MouseLeftButtonPressed() &&
            BoundingBox.Contains((int)inputHelper.MousePosition.X, (int)inputHelper.MousePosition.Y);
    }

    public override void Reset()
    {
        base.Reset();
        isPressed = false;
    }

    public bool Pressed
    {
        get { return isPressed; }
    }

}

