using System;
using Microsoft.Xna.Framework;

class Handle : InteractiveObject
{
    Timer countDownTimer;
    int handlenumber = 0;
    public Handle(string assetname, string id, int sheetIndex) : base(assetname, id, sheetIndex)
    {
        countDownTimer = new Timer((float)0.2);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        countDownTimer.Update(gameTime);
        if (countDownTimer.IsExpired)
        {
            this.Reset();
            ActivateCorrObject("close"); //sluit de bijbehorende objecten.
        }
    }
    protected override void Interact(Character targetCharacter)
    {
        //verandert de hendel sprite naar aan stand
        this.ChangeSpriteIndex(1);
        ActivateCorrObject("open"); //opent de bijbehorende objecten
        interacting = false;
        StartResetTimer();
    }

    public void ActivateCorrObject(string action)
    {
        //Checks which objects has the same objectnumber as this handlenumber and opens or closes the corresponding objects.
        //give "open" to open the corresponding object. Give "close" to close the corresponding object.

        GameObjectGrid tileField = GameWorld.Find("TileField") as GameObjectGrid;
        foreach (var openableObject in tileField.Objects)
        {
            if (openableObject is OpenableObject)
            {
                if (((OpenableObject) openableObject).Objectnumber == handlenumber)
                {
                    if(action == "open")
                    ((OpenableObject)openableObject).Open();

                    if(action == "close")
                    ((OpenableObject)openableObject).Close();
                }
            }
        }
    }

    public override void Reset()
    {
        this.ChangeSpriteIndex(0);
        base.Reset();
    }

    protected void StartResetTimer()
    {
        countDownTimer.Reset();
        countDownTimer.IsPaused = false;
    }

    public int ObjectNumberConnected
    {
        get { return this.handlenumber; }
        set { handlenumber = value; }
    }

}


