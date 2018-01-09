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
    AnimatedGameObject[] readySprite = new AnimatedGameObject[4];
    int[] characterSelectIndex = new int[4];
    bool[] lockInSprite = new bool[4];
    Color[] lockInColor = new Color[8];
    float launchCount = 1;
    public CharacterSelection()
    {
        //The colors of the background borders
        lockInColor[0] = Color.LightSlateGray;
        lockInColor[1] = Color.DarkGray;
        lockInColor[2] = Color.DarkBlue;
        lockInColor[3] = Color.Blue;
        lockInColor[4] = Color.Green;
        lockInColor[5] = Color.LightGreen;
        lockInColor[6] = Color.OrangeRed;
        lockInColor[7] = Color.Orange;

        //The background
        SpriteGameObject backgroundSprite = new SpriteGameObject("Assets/Sprites/Character selection/Achtergrond");
        Add(backgroundSprite);

        //Make a list of all the possible character sprites, also place all the necessary components
        for (int i = 0; i < characterSprites.Length; i++)
        {
            //First load in the title of the state
            SpriteGameObject titleState = new SpriteGameObject("Assets/Sprites/Character selection/CharacterSelectTitle");
            Add(titleState);

            //load in the borders of the selection
            borderSprites[i] = new SpriteGameObject("Assets/Sprites/Character selection/Border", 1);
            borderSprites[i].Position = new Vector2(GameEnvironment.Screen.X / 4 * i, 170);
            Add(borderSprites[i]);

            //make and load in the animations
            characterSprites[i] = new AnimatedGameObject(2);
            characterSprites[i].LoadAnimation("Assets/Sprites/Shieldmaiden/shieldmaiden_default", "maiden1", true);
            characterSprites[i].LoadAnimation("Assets/Sprites/Shieldmaiden/shieldmaiden_blue", "maiden2", true);
            characterSprites[i].LoadAnimation("Assets/Sprites/Shieldmaiden/shieldmaiden_green", "maiden3", true);
            characterSprites[i].LoadAnimation("Assets/Sprites/Shieldmaiden/shieldmaiden_orange", "maiden4", true);
            
            //position of the sprites
            characterSprites[i].Position = new Vector2(borderSprites[i].Position.X + borderSprites[i].Width / 2,
                borderSprites[i].Position.Y + 150);
            Add(characterSprites[i]);

            //play all the different animations
            characterSprites[i].PlayAnimation("maiden" + (i + 1));

            //Add the index number of the selected animation
            characterSelectIndex[i] = i + 1;

            //Change color of background accordingly to the playing animation
            borderSprites[i].GetColor = lockInColor[(characterSelectIndex[i] - 1) * 2];

            //Place all the ready sprites
            readySprite[i] = new AnimatedGameObject(2);
            readySprite[i].Position = new Vector2(borderSprites[i].Position.X + borderSprites[i].Width / 2, borderSprites[i].Position.Y + borderSprites[i].Height / 7 * 6);
            readySprite[i].LoadAnimation("Assets/Sprites/Character selection/Ready-not", "notReady", true);
            readySprite[i].LoadAnimation("Assets/Sprites/Character selection/Ready!", "Ready", true);
            readySprite[i].PlayAnimation("notReady");
            Add(readySprite[i]);


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

        //Input player 1, keyboard controlled
        if (!lockInSprite[0])
        {
            if (inputHelper.KeyPressed(Keys.Right))
                ChangeSpriteRight(characterSprites[0], characterSelectIndex[0], 0);

            else if (inputHelper.KeyPressed(Keys.Left))
                ChangeSpriteLeft(characterSprites[0], characterSelectIndex[0], 0);
        }

        //Player 1, lock in character selection.
        if (inputHelper.KeyPressed(Keys.Enter))
        {
            //Player 1 Lock in
            if(CheckLockIn(0))
                lockInSprite[0] = !lockInSprite[0];

            if (lockInSprite[0])
            {
                borderSprites[0].GetColor = lockInColor[(characterSelectIndex[0] - 1) * 2 + 1];
                readySprite[0].PlayAnimation("Ready");
            }
            else
            {
                borderSprites[0].GetColor = lockInColor[(characterSelectIndex[0] - 1) * 2];
                readySprite[0].PlayAnimation("notReady");
            }

            //Player 3 lock in
            if (CheckLockIn(2))
                lockInSprite[2] = !lockInSprite[2];
            if (lockInSprite[2])
            {
                borderSprites[2].GetColor = lockInColor[(characterSelectIndex[2] - 1) * 2 + 1];
                readySprite[2].PlayAnimation("Ready");
            }
            else
            {
                borderSprites[2].GetColor = lockInColor[(characterSelectIndex[2] - 1) * 2];
                readySprite[2].PlayAnimation("notReady");
            }

        }

        //input player 2, keyboard controlled
        if (!lockInSprite[1])
        {
            if (inputHelper.KeyPressed(Keys.D))
                ChangeSpriteRight(characterSprites[1], characterSelectIndex[1], 1);

            else if (inputHelper.KeyPressed(Keys.A))
                ChangeSpriteLeft(characterSprites[1], characterSelectIndex[1], 1);
        }

        if (inputHelper.KeyPressed(Keys.Space))
        {
            //Player 2 Lock in
            if (CheckLockIn(1))
                lockInSprite[1] = !lockInSprite[1];

            if (lockInSprite[1])
            {
                borderSprites[1].GetColor = lockInColor[(characterSelectIndex[1] - 1) * 2 + 1];
                readySprite[1].PlayAnimation("Ready");
            }
            else
            {
                borderSprites[1].GetColor = lockInColor[(characterSelectIndex[1] - 1) * 2];
                readySprite[1].PlayAnimation("notReady");
            }

            //Player 4 lock in
            if (CheckLockIn(3))
                lockInSprite[3] = !lockInSprite[3];
            if (lockInSprite[3])
            {
                borderSprites[3].GetColor = lockInColor[(characterSelectIndex[3] - 1) * 2 + 1];
                readySprite[3].PlayAnimation("Ready");
            }
            else
            {
                borderSprites[3].GetColor = lockInColor[(characterSelectIndex[3] - 1) * 2];
                readySprite[3].PlayAnimation("notReady");
            }


        }
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

    //Checks if all the players are locked in with their character.
    public bool AllReadyCheck()
    {
        for(int i = 0; i < lockInSprite.Length; i++)
        {
            if (!lockInSprite[i])
            {
                launchCount = 3;
                return false;
            }
        }
        return true;
    }
}

