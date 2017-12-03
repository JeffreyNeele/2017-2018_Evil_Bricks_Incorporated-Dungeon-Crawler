using Microsoft.Xna.Framework.Input;


class CutsceneState : GameObjectList
{
    int currentCutscene = 0;



public CutsceneState()
    {
        Cutscene cutscene = new Cutscene();
        cutscene.Play_Cutscene(currentCutscene); //zorgt dat de juiste cutscene wordt afgespeeld
        currentCutscene += 1;
        GameEnvironment.GameStateManager.SwitchTo("playing");
    }


    public override void HandleInput(InputHelper inputHelper)
    {
        base.HandleInput(inputHelper);
        if (inputHelper.KeyPressed(Keys.Space)) //To skip the cutscene, press space
        {
            GameEnvironment.GameStateManager.SwitchTo("playing");
        }
        
    }
}

