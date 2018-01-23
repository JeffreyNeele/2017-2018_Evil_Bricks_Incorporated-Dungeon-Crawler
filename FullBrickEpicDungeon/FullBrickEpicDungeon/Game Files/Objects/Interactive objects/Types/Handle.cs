using Microsoft.Xna.Framework;

/// <summary>
/// Class to define a handle object
/// </summary>
class Handle : InteractiveObject
{
    Timer countDownTimer;
    int handlenumber = 0;

    /// <param name="assetname">Path to be able to load in the sprite</param>
    /// <param name="id">defined id to be able to find the door</param>
    /// <param name="sheetIndex">Defines which picture of the animation will be shown</param>
    public Handle(string assetname, string id, int sheetIndex) : base(assetname, id, sheetIndex)
    {
        countDownTimer = new Timer((float)0.2);
    }

    //Update the countdownTimer. If countdownTimer is expired, objects that the handle affects are closed
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        if(ObjectNumberConnected != 99)
            countDownTimer.Update(gameTime);
        if (countDownTimer.IsExpired)
        {
            this.Reset();
            ActivateCorrObject("close"); //sluit de bijbehorende objecten.
        }
    }

    //Method that changes the sprite and opens all the objects that the handle affects. Reset the handle timer so all the objects stay open
    protected override void Interact(Character targetCharacter)
    {
        //verandert de hendel sprite naar aan stand
        this.sprite.SheetIndex = 1;
        ActivateCorrObject("open"); //opent de bijbehorende objecten
        interacting = false;
        StartResetTimer();
    }

    //Method that opens all the affected objects
    public void ActivateCorrObject(string action)
    {
        //Checks which objects has the same objectnumber as this handlenumber and opens or closes the corresponding objects.
        //give "open" to open the corresponding object. Give "close" to close the corresponding object.

        GameObjectGrid tileField = GameWorld.Find("TileField") as GameObjectGrid;
        GameObjectList objects = GameWorld.Find("objectLIST") as GameObjectList;

        foreach (var openableObject in tileField.Objects)
        {
            if (openableObject is OpenableObject)
            {
                if(this.ObjectNumberConnected == 99)
                {
                    ((OpenableObject)openableObject).Open();
                    continue;
                }
                if (((OpenableObject) openableObject).Objectnumber == handlenumber)
                {
                    if(action == "open")
                    ((OpenableObject)openableObject).Open();

                    if(action == "close")
                    ((OpenableObject)openableObject).Close();
                }
            }
        }

        foreach (var openableObject in objects.Children)
        {
            if (openableObject is OpenableObject)
            {
                if (this.ObjectNumberConnected == 99)
                {
                    ((OpenableObject)openableObject).Open();
                    if (openableObject is Door)
                        ((Door)openableObject).openDoor = true;
                    continue;
                }
                if (((OpenableObject)openableObject).Objectnumber == handlenumber)
                {
                    if (action == "open")
                        ((OpenableObject)openableObject).Open();

                    if (action == "close")
                        ((OpenableObject)openableObject).Close();
                }
            }
        }
    }

    //Reset the handle to the starting picture
    public override void Reset()
    {
        this.sprite.SheetIndex = 0;
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


