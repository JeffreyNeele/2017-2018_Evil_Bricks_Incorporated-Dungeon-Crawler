using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;

class ConversationState : IGameLoopObject
{
    IGameLoopObject playingState;
    Conversation conversation;
    protected List<Conversation> conversationList = new List<Conversation>();
    enum Conversationnames { LarryShits, ThroneRoom };
    int currentConversationNumber = 0;

    /// <summary>
    /// State for if a conversation state is being displayed
    /// </summary>
    public ConversationState()
    {

        // draw over the playingstate
        playingState = GameEnvironment.GameStateManager.GetGameState("playingState");
        // loads the conversation
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
        playingState.Draw(gameTime, spriteBatch);
        CurrentConversation.Draw(gameTime, spriteBatch);
    }

    public void HandleInput(InputHelper inputHelper)
    {
        CurrentConversation.HandleInput(inputHelper);
    }

    public void Reset()
    {
        CurrentConversation.Reset();
    }

    public void GoToNextConversation()
    {
        CurrentConversation.Reset();
        if (currentConversationNumber>= conversationList.Count - 1)
        {
            // if all the levels are over switch to the title state
            throw new IndexOutOfRangeException("Could not find conversation " + currentConversationNumber+1);
        }
        else
        {
            currentConversationNumber++;
        }
    }

    private Conversation CurrentConversation
    {
       get{ return conversationList[currentConversationNumber]; }
    }
}

