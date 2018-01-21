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
    IGameLoopObject conversation;
    int currentCutscene = 0;
    List<Cutscene> cutsceneList = new List<Cutscene>();
    enum Cutscenenames {LarryShits,ThroneRoom1,ThroneRoom2};
    


/// <summary>
/// State for cutscenes
/// </summary>
public CutsceneState()
    {
        conversation = GameEnvironment.GameStateManager.GetGameState("conversation");
        LoadCutScenes();
    }

    public void LoadCutScenes()
    {
        for (int x = 0; x < Enum.GetNames(typeof(Cutscenenames)).Length; x++)
        {
            Cutscene newcutscene = new Cutscene("Assets/Cutscenes/" + Enum.GetName(typeof(Cutscenenames), x) + ".png");
            cutsceneList.Add(newcutscene);
            GameEnvironment.GameStateManager.SwitchTo("playingState");
        }
    }


    public void Update(GameTime gameTime)
    {
        conversation.Update(gameTime);
        CurrentCutscene.Update(gameTime);
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        conversation.Draw(gameTime, spriteBatch);
       CurrentCutscene.Draw(gameTime, spriteBatch);
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


    private Cutscene CurrentCutscene
    {
        get { return cutsceneList[currentCutscene]; }
    }
}

