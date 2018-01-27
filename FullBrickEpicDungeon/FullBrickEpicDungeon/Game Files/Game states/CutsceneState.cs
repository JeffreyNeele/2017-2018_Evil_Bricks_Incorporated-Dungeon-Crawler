using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;


class CutsceneState : IGameLoopObject
{
    int currentCutsceneNumber = 0;
    List<Cutscene> cutsceneList = new List<Cutscene>();
    enum Cutscenenames {LarryShits,LarryGetsCaptured,ThroneRoom1,ThroneRoom2};
    


/// <summary>
/// State for cutscenes
/// </summary>
public CutsceneState()
    {
        LoadCutScenes();
    }


    /// <summary>
    /// Loads all cutscenes named in Cutscenenames
    /// </summary>
    public void LoadCutScenes()
    {
        for (int x = 0; x < Enum.GetNames(typeof(Cutscenenames)).Length; x++)
        {
            Cutscene newcutscene = new Cutscene("Assets/Cutscenes/" + Enum.GetName(typeof(Cutscenenames), x));
            cutsceneList.Add(newcutscene);
        }
    }


    public void Update(GameTime gameTime)
    {
        CurrentCutscene.Update(gameTime);
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        CurrentCutscene.Draw(gameTime, spriteBatch);
    }

    public void HandleInput(InputHelper inputHelper)
    {

        
    }

    /// <summary>
    /// Wordt aangeroepen bij SwitchTo("cutscene")
    /// </summary>
    public void Setup()
    {
        GameEnvironment.GameStateManager.SwitchTo("conversation");
    }

    public void Reset() {}

    public void GoToNextCutscene()
    {
        if (currentCutsceneNumber >= cutsceneList.Count)
        {
            // if all the levels are over switch to the title state
            throw new IndexOutOfRangeException("Could not find cutscene " + (currentCutsceneNumber + 1));
        }
        switch(currentCutsceneNumber)
        {
            //bij 3 zijn ze door het luik gestort. Dan moet hij naar playingstate. Standaard wordt ervanuit gegaan dat er nog een cutscene komt.
            case 3: GameEnvironment.GameStateManager.SwitchTo("playingState"); 
                    currentCutsceneNumber++;
                break;
            default: currentCutsceneNumber++;
                break;
        }

    }



    private Cutscene CurrentCutscene
    {
        get { return cutsceneList[currentCutsceneNumber]; }
    }
}

