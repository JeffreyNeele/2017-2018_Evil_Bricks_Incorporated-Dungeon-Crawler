using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

class PlayingState : IGameLoopObject 
{
    // level list
    protected List<Level> levelList;
    // current level that is being used
    protected int currentLevelIndex;

    /// <summary>
    /// A state that defines the playing state of the game
    /// </summary>
    public PlayingState()
    {
        currentLevelIndex = 8;
        levelList = new List<Level>();
        // Loads the levels from all level files
        LoadLevels(10);
    }

    /// <summary>
    /// Handles input, making it able for the player to pause the game
    /// </summary>
    public void HandleInput(InputHelper inputHelper)
    {
        CurrentLevel.HandleInput(inputHelper);

        if (inputHelper.KeyPressed(Keys.Space))
        {
            if(FullBrickEpicDungeon.DungeonCrawler.SFX)
                GameEnvironment.AssetManager.PlaySound("Assets/SFX/pause");

            FullBrickEpicDungeon.DungeonCrawler.mouseVisible = true;
            GameEnvironment.GameStateManager.SwitchTo("pauseState");
        }
    }

    public void Reset()
    {
        CurrentLevel.Reset();
    }

    public void Update(GameTime gameTime)
    {
        CurrentLevel.Update(gameTime);
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        CurrentLevel.Draw(gameTime, spriteBatch);
    }

    /// <summary>
    /// Loads the levels into the level list
    /// </summary>
    /// <param name="levelAmount">total amount of levels</param>
    public void LoadLevels(int levelAmount)
    {
        for(int x = 1; x <= levelAmount; x++)
        {
            Level newlevel = new Level(x);
            levelList.Add(newlevel);
        }
    }

    public void GoToNextLevel()
    {
        CurrentLevel.Reset();
        if (currentLevelIndex >= levelList.Count - 1)
        {
            // if all the levels are over switch to the title state
            GameEnvironment.GameStateManager.SwitchTo("titleMenu");
        }
        else
        {
            currentLevelIndex++;
        }
    }

    // returns the current level being used in the state
    public Level CurrentLevel
    {
        get { return levelList[currentLevelIndex]; }
    }

}
