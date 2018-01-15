using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

//State die zorgt voor de conversaties tussen characters in de game. Kan ook overgangen maken naar CutSceneState.
class ConversationState : IGameLoopObject
{
    IGameLoopObject playingState;
    Conversation conversation;

    public ConversationState()
    {
        //makes and shows a new conversation when switched to this state.
        playingState = GameEnvironment.GameStateManager.GetGameState("playingState");
        conversation = new Conversation("Assets/Conversations/conv_test.txt", 0, 100);
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

