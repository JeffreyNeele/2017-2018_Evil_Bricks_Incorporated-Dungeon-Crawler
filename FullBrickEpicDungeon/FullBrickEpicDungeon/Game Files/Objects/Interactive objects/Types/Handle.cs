using System.Collections.Generic;
using Microsoft.Xna.Framework;

/// <summary>
/// Class to define a handle object
/// </summary>
class Handle : InteractiveObject
{
    Timer countDownTimer;
    int handlenumber = 0;
    bool summoningHandle, alreadySummoned;
    List<Monster> summonList;

    /// <param name="assetname">Path to be able to load in the sprite</param>
    /// <param name="id">defined id to be able to find the door</param>
    /// <param name="sheetIndex">Defines which picture of the animation will be shown</param>
    public Handle(string assetname, Level currentlevel, string id, int sheetIndex, bool summonAbility = false) : base(assetname, currentlevel,  id, sheetIndex)
    {
        countDownTimer = new Timer((float)0.2);
        summonList = new List<Monster>();
        summoningHandle = summonAbility;
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
        if(!alreadySummoned && summoningHandle)
            SummonMonsters();
        interacting = false;
        StartResetTimer();
    }

    //Method that opens all the affected objects
    public void ActivateCorrObject(string action)
    {
        //Checks which objects has the same objectnumber as this handlenumber and opens or closes the corresponding objects.
        //give "open" to open the corresponding object. Give "close" to close the corresponding object.

        GameObjectGrid tileField = currentlevel.GameWorld.Find("TileField") as GameObjectGrid;
        GameObjectList objects = currentlevel.GameWorld.Find("objectLIST") as GameObjectList;

        foreach (var openableObject in tileField.Objects)
        {
            if (openableObject is OpenableObject)
            {
                if(this.ObjectNumberConnected == 99)
                {
                    ((OpenableObject)openableObject).Open();
                    if (openableObject is Door)
                        ((Door)openableObject).openDoor = true;
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

            if (openableObject is Lock)
                if (this.ObjectNumberConnected == 99)
                    ((Lock)openableObject).Visible = false;
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

    public void SummonMonsters()
    {
        GameObjectList monsters = currentlevel.GameWorld.Find("monsterLIST") as GameObjectList;
        for (int i = 0; i < summonList.Count; i++)
        {
            if (summonList[i] is LittlePenguin)
            {
                Monster monster = new LittlePenguin(currentlevel);
                monster.Position = summonList[i].Position;
                monsters.Add(monster);
            }
            else if(summonList[i] is Bunny)
            {
                Monster monster = new Bunny(currentlevel);
                monster.Position = summonList[i].Position;
                monsters.Add(monster);
            }
            else if(summonList[i] is BossBunny)
            {
                Monster monster = new BossBunny(currentlevel);
                monster.Position = summonList[i].Position;
                monsters.Add(monster);
            }
        }
        alreadySummoned = true;
    }

    public int ObjectNumberConnected
    {
        get { return this.handlenumber; }
        set { handlenumber = value; }
    }

    public List<Monster> handleSummon
    {
        get { return summonList; }
    }

    public bool ableToSummon
    {
        get { return summoningHandle; }
        set { summoningHandle = value; }
    }

}


