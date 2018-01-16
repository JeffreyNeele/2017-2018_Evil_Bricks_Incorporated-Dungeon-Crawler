using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


class CutsceneState : IGameLoopObject
{
    int currentCutscene = 0;


/// <summary>
/// State for cutscenes
/// </summary>
public CutsceneState()
    {
        Cutscene cutscene = new Cutscene();
        currentCutscene += 1;
        GameEnvironment.GameStateManager.SwitchTo("playingState");
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {

    }

    public void HandleInput(InputHelper inputHelper)
    {
        if (inputHelper.KeyPressed(Keys.Space)) //To skip the cutscene, press space
        {
            GameEnvironment.GameStateManager.SwitchTo("playingState");
        }
        
    }

    public void Reset()
    {

    }

    public void Update(GameTime gameTime)
    {

    }
}

