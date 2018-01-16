using System.Collections.Generic;
/// <summary>
/// Manager for all cutscenes
/// </summary>
    class CutsceneManager
    {
    Cutscene playing;
    List<Cutscene> cutsceneList;
    public CutsceneManager()
    {
        cutsceneList = new List<Cutscene>();
    }

    public void Play_Cutscene(int sceneNumber)
    {
        playing = cutsceneList[sceneNumber];
    }

    }

