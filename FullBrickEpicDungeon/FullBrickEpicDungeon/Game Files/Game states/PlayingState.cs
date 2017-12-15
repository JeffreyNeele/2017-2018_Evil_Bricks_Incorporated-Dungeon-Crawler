using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


class PlayingState : IGameLoopObject
{
    protected Button pauseButton;
    protected List<Level> levelList;
    protected int currentLevelIndex;

    public PlayingState()
    {
        currentLevelIndex = 0;
        levelList = new List<Level>();
        LoadLevels(1);
    }

    public void HandleInput(InputHelper inputHelper)
    {
        CurrentLevel.HandleInput(inputHelper);

        if (pauseButton.Pressed)
        {
            GameEnvironment.GameStateManager.SwitchTo("playingState");
        }
    }

    public void Reset()
    {
        CurrentLevel.Reset();
    }

    public void Update(GameTime gameTime)
    {

    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        CurrentLevel.Draw(gameTime, spriteBatch);
    }

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
            // GameEnvironment.GameStateManager.SwitchTo aka switch to another state if all levels are finished
        }
        else
        {
            currentLevelIndex++;
        }
    }

    public Level CurrentLevel
    {
        get { return levelList[currentLevelIndex]; }
    }
}
