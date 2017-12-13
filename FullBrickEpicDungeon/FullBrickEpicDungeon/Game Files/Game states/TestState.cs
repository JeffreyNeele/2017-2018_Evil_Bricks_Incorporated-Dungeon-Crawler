
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class TestState : GameObjectList
{
    Conversation conversation;
    AnimationTester animationTester;
    public TestState()
    {
        conversation = new Conversation();
        animationTester = new AnimationTester();
        conversation.LoadConversation("Assets/Conversations/conv_test.txt",0,100);
        Add(conversation);
        conversation.ShowConversationBox();
    }

    public override void Update(GameTime gameTime)
    {
        animationTester.Update(gameTime);
        base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        animationTester.Draw(gameTime, spriteBatch);
        base.Draw(gameTime, spriteBatch);
    }
}
