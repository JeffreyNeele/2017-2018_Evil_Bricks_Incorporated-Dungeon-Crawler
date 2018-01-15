using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


//Spelers kunnen in deze state connecten met de game (met xbox controller of toetsenbord).
//Daarna is het mogelijk om een character te kiezen. Uitgangspunt is dat een character maar één keer gekozen kan worden.
class CharacterSelection : GameObjectList
{
    AnimatedGameObject[] characterSprites = new AnimatedGameObject[4];
    SpriteGameObject[] borderSprites = new SpriteGameObject[4];
    SpriteGameObject[] controlSprites = new SpriteGameObject[4];
    AnimatedGameObject[] readySprite = new AnimatedGameObject[4];

    int[] characterSelectIndex = new int[4];
    bool[] lockInSprite = new bool[4];
    Color[] lockInColor = new Color[8];
    float launchCount = 1;

    int playersjoined = 0;
    bool[] keyboardjoined = new bool[2];
    bool[] xboxjoined = new bool[4];

    
    //left in this is 1,2,3,4,5,6. (0,1 for keyboard, 2-5 for xbox. Dictionary translates to the number of the playerborder the player joined in.
    Dictionary<int, int> playerborder = new Dictionary<int, int>();

    //toetsenbord controls dictionary van player 0 en 1 in de dictionary hiervoor
    protected Dictionary<Keys, Keys> keyboardControls1;
    protected Dictionary<Keys, Keys> keyboardControls2;

    //matches the player number to the controls dictionary used.
    protected Dictionary<int, Dictionary<Keys, Keys>> keyboardcontrols= new Dictionary<int, Dictionary<Keys, Keys>>();

    public CharacterSelection()
    {
        keyboardControls1 = GameEnvironment.SettingsHelper.GenerateKeyboardControls("Assets/KeyboardControls/player1controls.txt");
        keyboardControls2 = GameEnvironment.SettingsHelper.GenerateKeyboardControls("Assets/KeyboardControls/player2controls.txt");
        keyboardcontrols.Add(0, keyboardControls1);
        keyboardcontrols.Add(1, keyboardControls2);

        //The colors of the background borders
        lockInColor[0] = Color.Purple;
        lockInColor[1] = Color.MediumPurple;
        lockInColor[2] = Color.DarkBlue;
        lockInColor[3] = Color.RoyalBlue;
        lockInColor[4] = Color.Green;
        lockInColor[5] = Color.LightGreen;
        lockInColor[6] = Color.OrangeRed;
        lockInColor[7] = Color.Orange;

        //The background
        SpriteGameObject backgroundSprite = new SpriteGameObject("Assets/Sprites/Character selection/Achtergrond");
        Add(backgroundSprite);

        //Make a list of all the possible character sprites, also place all the necessary components
        for (int i = 0; i < 4; i++)
        {
            //First load in the title of the state
            SpriteGameObject titleState = new SpriteGameObject("Assets/Sprites/Character selection/CharacterSelectTitle");
            Add(titleState);

            //load in the borders of the selection
            borderSprites[i] = new SpriteGameObject("Assets/Sprites/Character selection/Border", 1);
            borderSprites[i].Position = new Vector2(GameEnvironment.Screen.X / 4 * i, 170);
            Add(borderSprites[i]);

            //load in Press to join frame
            controlSprites[i] = new SpriteGameObject("Assets/Sprites/Character selection/ControllerParchment/PressToJoinRes400",2);
            controlSprites[i].Position = new Vector2(GameEnvironment.Screen.X / 4 * i +40, 450);
            Add(controlSprites[i]);
        }   
    }






    public override void Update(GameTime gameTime)
    {
        if (AllReadyCheck())
        {
            launchCount -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (launchCount <= 0)
                GameEnvironment.GameStateManager.SwitchTo("playingState");
        }
        base.Update(gameTime);
    }





    public override void HandleInput(InputHelper inputHelper)
    {
        base.HandleInput(inputHelper);


        //Join the player that presses the button
        if (playersjoined < 4)
        {
            //Join keyboard players if input is detected
            if (inputHelper.KeyPressed(Keys.S))
            {
                if (keyboardjoined[0] == false)
                {
                    keyboardjoined[0] = true;
                    JoinPlayer(0);
                }
            }

            if (inputHelper.KeyPressed(Keys.Down))
            {
                if (keyboardjoined[1] == false)
                {
                    keyboardjoined[1] = true;
                    JoinPlayer(1);
                }
            }

            //Join xbox players if input is detected
            for (int controller = 1; controller <= 4; controller++)
                if (inputHelper.ButtonPressed(controller, Buttons.A) && xboxjoined[controller - 1] == false)
                {
                    xboxjoined[controller - 1] = true;
                    JoinPlayer(controller + 1); //to yield 2-5 in the dictionary
                }  
        }


        //the methods below handle the characterselection left right input and lock in
        XboxCharacterSelection(inputHelper);
        KeyboardCharacterSelection(inputHelper);
        
    }





    public void JoinPlayer(int controlsnr)
    {
        //make and load in the animations
        characterSprites[playersjoined] = new AnimatedGameObject(2);
        characterSprites[playersjoined].LoadAnimation("Assets/Sprites/Character selection/shieldmaiden_default", "maiden1", true);
        characterSprites[playersjoined].LoadAnimation("Assets/Sprites/Character selection/shieldmaiden_blue", "maiden2", true);
        characterSprites[playersjoined].LoadAnimation("Assets/Sprites/Character selection/shieldmaiden_green", "maiden3", true);
        characterSprites[playersjoined].LoadAnimation("Assets/Sprites/Character selection/shieldmaiden_orange", "maiden4", true);

        //position of the sprites
        characterSprites[playersjoined].Position = new Vector2(borderSprites[playersjoined].Position.X + borderSprites[playersjoined].Width / 2, borderSprites[playersjoined].Position.Y + 100);
        Add(characterSprites[playersjoined]);

        //play all the different animations
        characterSprites[playersjoined].PlayAnimation("maiden" + (playersjoined + 1));

        //Add the index number of the selected animation
        characterSelectIndex[playersjoined] = playersjoined + 1;

        //Change color of background accordingly to the playing animation
        borderSprites[playersjoined].GetColor = lockInColor[(characterSelectIndex[playersjoined] - 1) * 2];

        //Place the ready sprite
        readySprite[playersjoined] = new AnimatedGameObject(2);
        readySprite[playersjoined].Position = new Vector2(borderSprites[playersjoined].Position.X + borderSprites[playersjoined].Width / 2, borderSprites[playersjoined].Position.Y + borderSprites[playersjoined].Height / 7 * 6);
        readySprite[playersjoined].LoadAnimation("Assets/Sprites/Character selection/Ready-not", "notReady", true);
        readySprite[playersjoined].LoadAnimation("Assets/Sprites/Character selection/Ready!", "Ready", true);
        readySprite[playersjoined].PlayAnimation("notReady");
        Add(readySprite[playersjoined]);


        //link the controlnumber to the bordernumber
        playerborder.Add(controlsnr, this.playersjoined); //playersjoined is the same as the border number in this method

        //load in Controls  Sprite
        if (controlsnr <= 1) //0 and 1 is keyboard
            controlSprites[playersjoined] = new SpriteGameObject("Assets/Sprites/Character selection/ControllerParchment/KeyboardControls2-2long400", 2);
        if (controlsnr >= 2) //2 to 5 is xbox
            controlSprites[playersjoined] = new SpriteGameObject("Assets/Sprites/Character selection/ControllerParchment/XboxControlled1long3-400", 2);

        controlSprites[playersjoined].Position = new Vector2(GameEnvironment.Screen.X / 4 * playersjoined + 40, 450);
        Add(controlSprites[playersjoined]);

        playersjoined++;

    }





    protected void XboxCharacterSelection(InputHelper inputHelper)
    {
        //handle xbox input/interact button for each joined player, independant of in which playerborder it fits.
        //iterates for controllers 2-5 (xbox controllers)
        for (int i = 2; i < 6; i++)
        {
            if (ControllerJoined(i))
            {
                //gives the correct number for input controllers (1-4)
                int controllernr = i - 1;

                //if the player is not locked in, handle left and right input to change character
                if (!lockInSprite[playerborder[i]])
                {
                    //right
                    if (inputHelper.MenuDirection(controllernr, true, false) == new Vector2(1, 0))
                        ChangeSpriteRight(characterSprites[playerborder[i]], characterSelectIndex[playerborder[i]], playerborder[i]);
                    //left
                    else if (inputHelper.MenuDirection(controllernr, true, false) == new Vector2(-1, 0))
                        ChangeSpriteLeft(characterSprites[playerborder[i]], characterSelectIndex[playerborder[i]], playerborder[i]);
                }
                //lock in character selection
                if (inputHelper.ButtonPressed(controllernr, Buttons.Y))
                    LockinPlayer(playerborder[i]);
            }
        }
    }



    protected void KeyboardCharacterSelection(InputHelper inputHelper)
    {
        //handle keyboard input/interact button for each joined player, independant of in which playerborder it fits.
        //iterates for controller 0 and 1 (keyboard)
        for (int i = 0; i <= 1; i++)
        {
            if (ControllerJoined(i))
            {
                
                    //right
                    if (inputHelper.KeyPressed(keyboardcontrols[i][Keys.D]))
                        ChangeSpriteRight(characterSprites[playerborder[i]], characterSelectIndex[playerborder[i]], playerborder[i]);

                    //left
                    else if (inputHelper.KeyPressed(keyboardcontrols[i][Keys.A]))
                        ChangeSpriteLeft(characterSprites[playerborder[i]], characterSelectIndex[playerborder[i]], playerborder[i]);

                //lock in character selection.
                if (inputHelper.KeyPressed(keyboardcontrols[i][Keys.E]))
                    LockinPlayer(playerborder[i]);
            }
        }
    }




    // if the controller is present in the dictionary (in other words: joined)
    protected bool ControllerJoined(int player)
    {

        foreach (int controller in playerborder.Keys)
        {
            if (controller == player)
            {
                Console.WriteLine(player);
                return true;
            }
        }
        return false;
    }




    //scrolling right through the selection is +1
    public void ChangeSpriteRight(AnimatedGameObject obj, int animationIndex, int index)
    {
        //Check if the index is not at max, if so change the index number to the lowest number (=1)
        if (animationIndex == 4)
            characterSelectIndex[index] = 1;
        else
            characterSelectIndex[index] += 1;
        obj.PlayAnimation("maiden" + characterSelectIndex[index].ToString());
        borderSprites[index].GetColor = lockInColor[(characterSelectIndex[index] - 1) * 2];
    }



    //scrolling left through the selection is -1
    public void ChangeSpriteLeft(AnimatedGameObject obj, int animationIndex, int index)
    {
        if (animationIndex == 1)
            characterSelectIndex[index] = 4;
        else
            characterSelectIndex[index] -= 1;
        obj.PlayAnimation("maiden" + characterSelectIndex[index].ToString());
        borderSprites[index].GetColor = lockInColor[(characterSelectIndex[index] - 1) * 2];
    }





    //Checks if it is possible to lock in
    public bool CheckLockIn(int index)
    {
        for (int i = 0; i < lockInSprite.Length; i++)
        {
            if (index == i)
                continue;
            else if (lockInSprite[i])
                if (characterSelectIndex[i] == characterSelectIndex[index])
                    return false;
        }
        return true;
    }


    protected void LockinPlayer(int player)
    {
       if (CheckLockIn(player))
            lockInSprite[player] = !lockInSprite[player];

        if (lockInSprite[player])
        {
            borderSprites[player].GetColor = lockInColor[(characterSelectIndex[player] - 1) * 2 + 1];
            readySprite[player].PlayAnimation("Ready");
        }
        else
        {
            borderSprites[player].GetColor = lockInColor[(characterSelectIndex[player] - 1) * 2];
            readySprite[player].PlayAnimation("notReady");
        }
    }



    //Checks if all the players are locked in with their character.
    public bool AllReadyCheck()
    {
       
        for (int i = 0; i < playerborder.Count; i++)
        {
            if (!lockInSprite[i])
            {
                launchCount = 2;
                return false;
            }
        }
        if (playerborder.Count >= 2)
        {
            return true;
        }
            return false;
    }

}

