using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

class PlayingState : IGameLoopObject
{
    // level list
    protected Level[] levelArray;
    private TimeSpan levelTimer;
    // current level that is being used
    protected int currentLevelIndex;
    protected const int numberOfLevels = 10;
    protected bool cheats;
    /// <summary>
    /// A state that defines the playing state of the game
    /// </summary>
    public PlayingState()
    {
        currentLevelIndex = 1;
        levelArray = new Level[numberOfLevels + 1]; //10 levels
    }

    /// <summary>
    /// Handles input, making it able for the player to pause the game
    /// </summary>
    public void HandleInput(InputHelper inputHelper)
    {
        CurrentLevel.HandleInput(inputHelper);
        if (inputHelper.KeyPressed(Keys.Space) || inputHelper.AnyPlayerPressed(Buttons.Start))
        {
            if (FullBrickEpicDungeon.DungeonCrawler.SFX)
                GameEnvironment.AssetManager.PlaySound("Assets/SFX/pause");

            FullBrickEpicDungeon.DungeonCrawler.mouseVisible = true;
            GameEnvironment.GameStateManager.SwitchTo("pauseState");
        }

        //Enables cheats
        if (inputHelper.KeyPressed(Keys.F1) || inputHelper.ButtonPressed(1, Buttons.DPadUp))
        {
            cheats = !cheats;
        }

        if (cheats)
        {
            CheatMenu(inputHelper);
        }
    }

    /// <summary>
    /// Method that allows the player to increase or decrease the level index on button pressed and allows the player to have infinite health
    /// </summary>
    private void CheatMenu(InputHelper inputHelper)
    {
        if ((inputHelper.KeyPressed(Keys.F2) || inputHelper.ButtonPressed(1, Buttons.DPadLeft)) && currentLevelIndex > 1)
        {
            currentLevelIndex--;
            this.Reset();
        }
        else if ((inputHelper.KeyPressed(Keys.F3) || inputHelper.ButtonPressed(1, Buttons.DPadRight)) && currentLevelIndex < numberOfLevels)
        {
            currentLevelIndex++;
            this.Reset();
        }
        //infinite health cheat
        else if (inputHelper.KeyPressed(Keys.F4) || inputHelper.ButtonPressed(1, Buttons.DPadDown))
        {
            GameObjectList playerList = CurrentLevel.GameWorld.Find("playerList") as GameObjectList;
            foreach (Character p in playerList.Children)
            {
                p.healthCheat = !p.healthCheat;
            }
        }
    }

    public void Initialize()
    {
    }


    public void Reset()
    {
        levelTimer = TimeSpan.Zero;
        Level newlevel = new Level(currentLevelIndex);
        levelArray[currentLevelIndex] = newlevel;
    }

    public void Update(GameTime gameTime)
    {
        levelTimer += gameTime.ElapsedGameTime;
        CurrentLevel.Update(gameTime);
    }
    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        CurrentLevel.Draw(gameTime, spriteBatch);
    }


    public void GoToNextLevel()
    {
        CurrentLevel.Reset();
        if (currentLevelIndex >= levelArray.Length - 1)
        {
            ResetLevelIndex();
            // if all the levels are over switch to the title state
            GameEnvironment.GameStateManager.SwitchTo("titleMenu");
        }
        else
        {
            currentLevelIndex++;
        }
    }


    public void ResetLevelIndex()
    {
        currentLevelIndex = 1;
    }

    // returns the current level being used in the state
    public Level CurrentLevel
    {
        get { return levelArray[currentLevelIndex]; }
    }

    public TimeSpan LevelTime
    {
        get { return levelTimer; }
        set { levelTimer = value; }
    }

}
