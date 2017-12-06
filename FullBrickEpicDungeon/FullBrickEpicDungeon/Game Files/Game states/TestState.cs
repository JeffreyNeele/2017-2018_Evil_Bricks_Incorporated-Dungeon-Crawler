
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class TestState : GameObjectList
{
    Conversation conversation;
    AnimationTester animationTester;
    public TestState()
    {
        
        conversation = new Conversation();
        conversation.LoadConversation("Content/Conversations/conv_test.txt",0,1);
        Add(conversation);
        animationTester = new AnimationTester();
        animationTester.TestAnimation();
        conversation.ShowConversation();
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
