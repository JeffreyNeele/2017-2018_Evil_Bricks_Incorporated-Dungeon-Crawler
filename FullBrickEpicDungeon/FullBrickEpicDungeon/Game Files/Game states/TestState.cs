
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class TestState : GameObjectList
{
    AnimationTester animationTester;

    public TestState()
    {
        //conversation = new Conversation();
        animationTester = new AnimationTester();
        //conversation.LoadConversation("Content/Conversations/conv_test.txt", 0, 100);
        //Add(conversation);
        Add(animationTester);
        //conversation.ShowConversationBox();

        // dit moet waarschijnlijk ergens anders gezet worden.
        Character Lightbringer = new Lightbringer();
        Lightbringer.Position = new Vector2(500, 500);
        Add(Lightbringer);

        Character shieldmaiden = new Shieldmaiden();
        shieldmaiden.Position = new Vector2(300, 300);
        Add(shieldmaiden);

    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        base.Draw(gameTime, spriteBatch);
        
    }
}
