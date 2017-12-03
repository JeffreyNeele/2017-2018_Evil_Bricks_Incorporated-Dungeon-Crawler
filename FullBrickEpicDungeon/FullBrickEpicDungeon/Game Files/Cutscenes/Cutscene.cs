using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    class Cutscene
    {
    Cutscene playing;
    Cutscene[] allCutscenes; //array van alle cutscenes op de juiste volgorde


    public Cutscene()
    {
        allCutscenes = new Cutscene[5]; //Pas de lengte van de array aan als er een Cutscene bij komt boven de 4!
        Cutscene larryShits = new _1LarryShits();
        allCutscenes[0] = larryShits;
        
    }

    public void Play_Cutscene(int sceneNumber)
    {
        playing = allCutscenes[sceneNumber];
    }

    }

