using System;

class Handle : InteractiveObject
{

    public Handle(string assetname, string id, int sheetIndex) : base(assetname, id, sheetIndex)
    {

    }

    protected override void Interact(Character targetCharacter)
    {
        bool HandleON = true;
        HandleON = !HandleON;
        this.ChangeSpriteImage("Assets/Sprites/InteractiveObjects/handle2");
        Console.WriteLine("I do it");

    }

    public override void Reset()
    {
        this.ChangeSpriteImage("Assets/Sprites/InteractiveObjects/handle1");
        Console.WriteLine("I reset");
    }

}


