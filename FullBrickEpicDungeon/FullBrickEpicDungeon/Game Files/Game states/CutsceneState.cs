using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;


class CutsceneState : IGameLoopObject
{
    //IGameLoopObject conversation;
    int currentCutsceneNumber = 0;
    List<Cutscene> cutsceneList = new List<Cutscene>();
    enum Cutscenenames {LarryShits,LarryGetsCaptured,ThroneRoom1,ThroneRoom2};
    


/// <summary>
/// State for cutscenes
/// </summary>
public CutsceneState()
    {
        LoadCutScenes();
        //conversation = GameEnvironment.GameStateManager.GetGameState("conversation");

    }

    public void LoadCutScenes()
    {
        for (int x = 0; x < Enum.GetNames(typeof(Cutscenenames)).Length; x++)
        {
            Cutscene newcutscene = new Cutscene("Assets/Cutscenes/" + Enum.GetName(typeof(Cutscenenames), x));
            cutsceneList.Add(newcutscene);
           // GameEnvironment.GameStateManager.SwitchTo("playingState");
        }
    }


    public void Update(GameTime gameTime)
    {
        //conversation.Update(gameTime);
        CurrentCutscene.Update(gameTime);
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        //conversation.Draw(gameTime, spriteBatch);
        CurrentCutscene.Draw(gameTime, spriteBatch);
    }

    public void HandleInput(InputHelper inputHelper)
    {
       /* if (inputHelper.KeyPressed(Keys.Space)) //To skip the cutscene, press space
        {
            GameEnvironment.GameStateManager.SwitchTo("playingState");
        }*/
        
    }

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

