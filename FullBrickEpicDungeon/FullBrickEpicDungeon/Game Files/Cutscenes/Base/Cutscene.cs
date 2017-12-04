using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// NOTE: this is a cutscene manager, not an inheritance class for all cutscenes
    class Cutscene
    {
    Cutscene playing;
    List<Cutscene> cutsceneList;
    public Cutscene()
    {
        cutsceneList = new List<Cutscene>();
        Cutscene larryShits = new _1LarryShits();
        cutsceneList.Add(larryShits);
        
    }

    public void Play_Cutscene(int sceneNumber)
    {
        playing = cutsceneList[sceneNumber];
    }

    }

