using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


class ConversationState : IGameLoopObject
{
    IGameLoopObject playingState;
    Conversation conversation;

    /// <summary>
    /// State for if a conversation state is being displayed
    /// </summary>
    public ConversationState()
    {
        // draw over the playingstate
        playingState = GameEnvironment.GameStateManager.GetGameState("playingState");
        // loads the conversation
        conversation = new Conversation("Assets/Conversations/conv_test.txt");
    }

    public void Update(GameTime gameTime)
    {
        conversation.Update(gameTime);
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        playingState.Draw(gameTime, spriteBatch);
        conversation.Draw(gameTime, spriteBatch);
    }

    public void HandleInput(InputHelper inputHelper)
    {
        conversation.HandleInput(inputHelper);
    }

    public void Reset()
    {
        conversation.Reset();
    }
}

