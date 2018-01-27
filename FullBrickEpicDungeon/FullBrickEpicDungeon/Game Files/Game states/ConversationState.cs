using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Input;

class ConversationState : IGameLoopObject
{
    IGameLoopObject drawOverState;
    protected List<Conversation> conversationList = new List<Conversation>();
    enum Conversationnames { LarryShits,LarryCaptured,ThroneRoom1,ThroneRoom2};
    int currentConversationNumber = 0;
    bool prevPlaying;

    /// <summary>
    /// State for if a conversation state is being displayed
    /// </summary>
    public ConversationState()
    {
        LoadConversations();
    }




    /// <summary>
    /// Loads the conversations into the conversation list
    /// </summary>
    /// <param name="conversationAmount">total amount of levels</param>
    public void LoadConversations()
    {
        int conversationsamount = Enum.GetNames(typeof(Conversationnames)).Length;
        for (int x = 0; x < conversationsamount; x++)
        {
            Conversation newConversation = new Conversation("Assets/Conversations/" + Enum.GetName(typeof(Conversationnames), x) + ".txt");
            conversationList.Add(newConversation);
        }
    }

    public void Update(GameTime gameTime)
    {
        CurrentConversation.Update(gameTime);
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        drawOverState.Draw(gameTime, spriteBatch);
        CurrentConversation.Draw(gameTime, spriteBatch);
    }

    public void HandleInput(InputHelper inputHelper)
    {
        CurrentConversation.HandleInput(inputHelper);

        //Only during a Cutscene you are able to skip a conversation. In playingstate you may or may not anger monsters, so you cannot skip
        if ((GameEnvironment.GameStateManager.PreviousGameState as IGameLoopObject) == GameEnvironment.GameStateManager.GetGameState("cutscene"))
        {
            if (inputHelper.KeyPressed(Keys.Enter) || inputHelper.ButtonPressed(1, Buttons.Back)) //To skip the cutscene, press Enter or Back for player 1 on Xbox
            {
                (GameEnvironment.GameStateManager.GetGameState("playingState") as PlayingState).Reset();
                GameEnvironment.GameStateManager.SwitchTo("playingState");
            }
        }
    }

    public void Initialize()
    {
        IGameLoopObject prevGameState = GameEnvironment.GameStateManager.PreviousGameState as IGameLoopObject;
        // draw over the playingstate if it was in playingstate
        if (prevGameState == GameEnvironment.GameStateManager.GetGameState("playingState"))
        {
            prevPlaying = true;
            drawOverState = GameEnvironment.GameStateManager.GetGameState("playingState");
        }
        //draw over cutscene if previousstate was cutscene
        else if ((GameEnvironment.GameStateManager.PreviousGameState as IGameLoopObject) == GameEnvironment.GameStateManager.GetGameState("cutscene"))
        {
            prevPlaying = false;
            drawOverState = GameEnvironment.GameStateManager.GetGameState("cutscene");
        }
        else
        {
            throw new Exception("Cutscene cannot be called from this GameState" + GameEnvironment.GameStateManager.PreviousGameState);
        }
    }

    public void Reset()
    {
        CurrentConversation.Reset();
    }

    public void GoToNextConversation()
    {
        CurrentConversation.Reset();
        if (currentConversationNumber >= conversationList.Count)
        {
            throw new IndexOutOfRangeException("There is no conversation file left, although you are trying to start another one.");   
        }
        //if the conversation is finished, start up the next event

        if (prevPlaying) //conversation came from playingstate
        {
            (GameEnvironment.GameStateManager.GetGameState("playingState") as PlayingState).Reset();
            GameEnvironment.GameStateManager.SwitchTo("playingState");
        }

        else if (!prevPlaying) //conversation came from a cutsceneState
        {
            CurrentConversation.Reset();
            //load next cutscene for next time it starts.
            (GameEnvironment.GameStateManager.GetGameState("cutscene") as CutsceneState).GoToNextCutscene();

            //depending on which cutscene just completed, switch to the next cutscene or the next playingstate
            switch (currentConversationNumber)
            {
                //after the ThroneRoom2 scene, the players play the game
                case (int)Conversationnames.ThroneRoom2:
                    (GameEnvironment.GameStateManager.GetGameState("playingState") as PlayingState).Reset();
                    GameEnvironment.GameStateManager.SwitchTo("playingState");
                    break;
                default:
                    GameEnvironment.GameStateManager.SwitchTo("cutscene");
                    break;
            }
        }
            currentConversationNumber++;
        
    }

    private Conversation CurrentConversation
    {
       get{ return conversationList[currentConversationNumber]; }
    }
}

