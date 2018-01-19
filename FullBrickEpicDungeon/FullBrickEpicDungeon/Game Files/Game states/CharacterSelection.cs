using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


//Spelers kunnen in deze state connecten met de game (met xbox controller of toetsenbord).
//Daarna is het mogelijk om een character te kiezen. Uitgangspunt is dat een character maar één keer gekozen kan worden.

/// <summary>
/// GameState where players are able to connect to the game and choose the appropriate character
/// </summary>
class CharacterSelection : GameObjectList
{
    AnimatedGameObject[] characterSprites = new AnimatedGameObject[4];      //Contains the objects of the shieldmaidens. Necessary to play the appropriate animation
    SpriteGameObject[] borderSprites = new SpriteGameObject[4];             //Array for the border sprites (at max 4 players may join, thus an array max 4 places
    SpriteGameObject[] controlSprites = new SpriteGameObject[4];            //The sprites that show the controls of each player. Some players may have more options than others
    AnimatedGameObject[] readySprite = new AnimatedGameObject[4];           //Contains the objects for the ready button.

    int[] characterSelectIndex = new int[4];                                //Contains the integer that is charactersprite needs to play and switch to the appropriate maiden animation
    bool[] lockInSprite = new bool[4];                                      //Each player has to lock in their choice of character. The array has a value 'true' if the player has made their selection
    Color[] lockInColor = new Color[8];                                     //Array that contains the color for the borders when a certain maiden is shown and upon lock in
    Timer launch = new Timer(2);                                            //The amount of seconds that will elapse when all the players are locked in. When 0, the gamestate will change.

    int playersjoined = 0;                                                  //Amount of players joined currently. Updates when a new player joins
    bool[] keyboardjoined = new bool[2];                                    //Only 2 players are able to play with keyboard input, all the players are able to play with xbox controller
    bool[] xboxjoined = new bool[4];


    //left in this is 1,2,3,4,5,6. (0,1 for keyboard, 2-5 for xbox. Dictionary translates to the number of the playerborder the player joined in.
    Dictionary<int, int> playerborder = new Dictionary<int, int>();

    //toetsenbord controls dictionary van player 0 en 1 in de dictionary hiervoor
    protected Dictionary<Keys, Keys> keyboardControls1;
    protected Dictionary<Keys, Keys> keyboardControls2;

    //matches the player number to the controls dictionary used.
    protected Dictionary<int, Dictionary<Keys, Keys>> keyboardcontrols = new Dictionary<int, Dictionary<Keys, Keys>>();

    public CharacterSelection()
    {
        //Load in all the keyboard controls to the dictionary
        keyboardControls1 = GameEnvironment.SettingsHelper.GenerateKeyboardControls("Assets/Controls/player1controls.txt");
        keyboardControls2 = GameEnvironment.SettingsHelper.GenerateKeyboardControls("Assets/Controls/player2controls.txt");
        keyboardcontrols.Add(0, keyboardControls1);
        keyboardcontrols.Add(1, keyboardControls2);

        //The colors of the background borders, for each maiden their own color during selection and upon lock in
        lockInColor[0] = Color.Purple;
        lockInColor[1] = Color.MediumPurple;
        lockInColor[2] = Color.DarkBlue;
        lockInColor[3] = Color.RoyalBlue;
        lockInColor[4] = Color.Green;
        lockInColor[5] = Color.LightGreen;
        lockInColor[6] = Color.OrangeRed;
        lockInColor[7] = Color.Orange;

        //First load in the title of the state
        SpriteGameObject titleState = new SpriteGameObject("Assets/Sprites/Character selection/CharacterSelectTitle");
        Add(titleState);

        //Make a list of all the possible character sprites, also place all the necessary components
        for (int i = 0; i < 4; i++)
        {
            //load in the borders of the selection
            borderSprites[i] = new SpriteGameObject("Assets/Sprites/Character selection/Border");
            borderSprites[i].Position = new Vector2(GameEnvironment.Screen.X / 4 * i, 170);
            Add(borderSprites[i]);

            //load in Press to join frame
            controlSprites[i] = new SpriteGameObject("Assets/Sprites/Character selection/ControllerParchment/PressToJoinRes400");
            controlSprites[i].Position = new Vector2(GameEnvironment.Screen.X / 4 * i + 40, 450);
            Add(controlSprites[i]);
        }
    }

    //In the update method check if all the players are locked in. If so, start the launch timer
    public override void Update(GameTime gameTime)
    {
        if (AllReadyCheck())
        {
            launch.Update(gameTime);
            if (launch.IsExpired)
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

    /// <summary>
    /// Method that lets a new player connect to the game
    /// </summary>
    /// <param name="controlsnr">Number that correspond to the controls dictionary of the user</param>
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

        //Do +1 to playersjoined. The border the player corresponds to is 0, the addition to the total amount of players joint is thus at the end of the method
        playersjoined++;

    }

    /// <summary>
    /// //handle xbox input/interact button for each joined player, independant of in which playerborder it fits.
    /// </summary>
    /// <param name="inputHelper"></param>
    protected void XboxCharacterSelection(InputHelper inputHelper)
    {
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

    /// <summary>
    /// Handles the keyboard input/interact button for each joined player, independant of in which playerborder it fits.
    /// </summary>
    /// <param name="inputHelper"></param>
    protected void KeyboardCharacterSelection(InputHelper inputHelper)
    {
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

    /// <summary>
    /// Checks if a controller is present in the dictionary / has joined
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Handles the scrolling through the selection to the right. To the right is +1
    /// </summary>
    /// <param name="obj">The character animation object</param>
    /// <param name="animationIndex">Number that corresponds to the current animation of the maiden</param>
    /// <param name="index">Border number</param>
    public void ChangeSpriteRight(AnimatedGameObject obj, int animationIndex, int index)
    {
        //Check if the index is not at max, if so change the index number to the lowest number (=1), else do +1
        if (animationIndex == 4)
            characterSelectIndex[index] = 1;
        else
            characterSelectIndex[index] += 1;
        obj.PlayAnimation("maiden" + characterSelectIndex[index].ToString());
        borderSprites[index].GetColor = lockInColor[(characterSelectIndex[index] - 1) * 2];
    }

    /// <summary>
    /// Handles the scrolling through the selection to the left. To the left is -1
    /// </summary>
    /// <param name="obj">The character animation object</param>
    /// <param name="animationIndex">Number that corresponds to the current animation of the maiden</param>
    /// <param name="index">Border number</param>
    public void ChangeSpriteLeft(AnimatedGameObject obj, int animationIndex, int index)
    {
        //Check if the animationIndex is not the lowest number. If so, change the number to 4, else do -1
        if (animationIndex == 1)
            characterSelectIndex[index] = 4;
        else
            characterSelectIndex[index] -= 1;
        obj.PlayAnimation("maiden" + characterSelectIndex[index].ToString());
        borderSprites[index].GetColor = lockInColor[(characterSelectIndex[index] - 1) * 2];
    }

    /// <summary>
    /// Checks if it is possible to lock in. Impossible to have duplicate characters ready
    /// </summary>
    /// <param name="index">Border number</param>
    /// <returns></returns>
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

    /// <summary>
    /// Method that locks in a player and their character. Changes ready and background accordingly.
    /// Method also able to unlock a character
    /// </summary>
    /// <param name="player">Border number</param>
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

    /// <summary>
    /// Method that checks if all the players are locked in with their character
    /// </summary>
    /// <returns></returns>
    public bool AllReadyCheck()
    {

        for (int i = 0; i < playerborder.Count; i++)
        {
            if (!lockInSprite[i])
            {
                launch.Reset();
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

