using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;

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
        //conversation = new Conversation("Assets/Conversations/conv_test.txt");
    }

    /// <summary>
    /// Loads the levels into the level list
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
        if ((GameEnvironment.GameStateManager.PreviousGameState as IGameLoopObject) == GameEnvironment.GameStateManager.GetGameState("cutscene"))
        {
            prevPlaying = false;
            drawOverState = GameEnvironment.GameStateManager.GetGameState("cutscene");
        }

        // loads the conversation
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
            throw new IndexOutOfRangeException("There is no conversation left, although you are trying to make one.");   
            //There are no conversations left
        }
         //if the conversation is finished, start up the next event
       
            if(prevPlaying) //conversation came from playingstate
            GameEnvironment.GameStateManager.SwitchTo("playingState");

            else if (!prevPlaying) //conversation came from a cutsceneState
            {
                CurrentConversation.Reset();
                //load next cutscene for next time it starts.
                (GameEnvironment.GameStateManager.GetGameState("cutscene") as CutsceneState).GoToNextCutscene(); 

                //depending on which cutscene just completed, switch to the next cutscene or the next playingstate
                switch(currentConversationNumber)
                {   
                    //after the ThroneRoom2 scene, the players play the game
                    case (int)Conversationnames.ThroneRoom2: GameEnvironment.GameStateManager.SwitchTo("playingState");
                        break;
                    default: GameEnvironment.GameStateManager.SwitchTo("cutscene");
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

