
using Microsoft.Xna.Framework;

class TestState : GameObjectList
{
    Conversation conversation;

    public TestState()
    {
        conversation = new Conversation();
        
        conversation.LoadConversation("Content/Conversations/conv_test.txt",0,1);
        Add(conversation);
        conversation.ShowConversation();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }
}
