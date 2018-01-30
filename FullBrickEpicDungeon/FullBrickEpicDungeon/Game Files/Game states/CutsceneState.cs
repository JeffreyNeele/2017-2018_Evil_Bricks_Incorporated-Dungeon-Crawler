using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System;
using System.IO;

class CutsceneState : IGameLoopObject
{
    int currentCutsceneNumber = 0;
    List<Cutscene> cutsceneList = new List<Cutscene>();
    enum Cutscenenames {LarryShits,LarryGetsCaptured,ThroneRoom1,ThroneRoom2};
    GameObjectList HintText;



    /// <summary>
    /// State for cutscenes
    /// </summary>
    public CutsceneState()
    {
        LoadCutScenes();
        HintText = new GameObjectList(100);
        LoadHint("Assets/Cutscenes/HintText.txt");
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
        HintText.Update(gameTime);
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        CurrentCutscene.Draw(gameTime, spriteBatch);
        HintText.Draw(gameTime, spriteBatch);
    }

    public void HandleInput(InputHelper inputHelper)
    {

        
    }

    /// <summary>
    /// Wordt aangeroepen bij SwitchTo("cutscene")
    /// </summary>
    public void Initialize()
    {
        if(FullBrickEpicDungeon.DungeonCrawler.music && MediaPlayer.State == MediaState.Stopped)
            GameEnvironment.AssetManager.PlayMusic("Assets/Music/cutscene");
        GameEnvironment.GameStateManager.SwitchTo("conversation");
    }

    public void Reset()
    {
        currentCutsceneNumber = 0;
    }

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
            case 3:
                    
                    (GameEnvironment.GameStateManager.GetGameState("playingState") as PlayingState).Reset();
                    GameEnvironment.GameStateManager.SwitchTo("playingState"); 
                    currentCutsceneNumber++;
                    Reset(); //The last Cutscene resets the cutscenes.
                break;
            default: currentCutsceneNumber++;
                break;
        }

    }


    private void LoadHint(string path)
    {
        // call with "Assets/Credits/CreditText" //
        path = "Content/" + path;
        List<string> textLines = new List<string>();
        StreamReader fileReader = new StreamReader(path);
        string line = fileReader.ReadLine();
        int width = line.Length;
        while (line != null)
        {
            textLines.Add(line);
            line = fileReader.ReadLine();
        }
        ShowHint(textLines);
    }


    private void ShowHint(List<string> hintLines)
    {
        for (int i = 0; i < hintLines.Count; i++)
        {
            TextGameObject hintline = new TextGameObject("Assets/Fonts/ConversationFont", 100)
            {
                Color = Color.Black,
                Text = hintLines[i],
            };
            hintline.Position = new Vector2(GameEnvironment.Screen.X / 2 - hintline.Size.X / 2, 20 + i * 50);
            HintText.Add(hintline);
        }
    }




    private Cutscene CurrentCutscene
    {
        get { return cutsceneList[currentCutsceneNumber]; }
    }
}

