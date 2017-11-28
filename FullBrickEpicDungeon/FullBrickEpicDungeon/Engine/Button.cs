using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Button : SpriteGameObject
{
    protected bool isPressed;
    protected Dictionary<string, int> stateNamesList;
    public Button(string AssetName, int layer = 0, string id = "") : base(AssetName, layer, id)
    {
        isPressed = false;
        stateNamesList = new Dictionary<string, int>();
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        isPressed = inputHelper.MouseLeftButtonPressed() &&
            BoundingBox.Contains((int)inputHelper.MousePosition.X, (int)inputHelper.MousePosition.Y);
    }

    public void AddState(int sheetindex, string id)
    {
        if(sheetindex <= Sprite.NumberSheetElements)
        {
            stateNamesList.Add(id, sheetindex);
        }
        else
        {
            throw new IndexOutOfRangeException("Given sheetindex is higher than the amount of elements in the given spritesheet!");
        }
    }

    public void SwitchState(string id)
    {
        int index;
        try
        {
            index = stateNamesList[id];
        }
        catch
        {
            throw new ArgumentNullException("this ID is not in the current list!");
        }

        Sprite.SheetIndex = index;
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

