
    class TestState : GameObjectList
{

    Conversation conversation;

    public TestState()
    {
        conversation = new Conversation();
        
        conversation.LoadConversation("Content/Conversations/conv_test.txt",0,1);
        conversation.ShowConversation();
        Add(conversation);
    }
}

