using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

//State die zorgt voor de conversaties tussen characters in de game. Kan ook overgangen maken naar CutSceneState.
class ConversationState : GameObjectList
{
    
    protected IGameLoopObject playingState;

    public ConversationState()
    {
        //makes and shows a new conversation when switched to this state.
        playingState = GameEnvironment.GameStateManager.GetGameState("playingState");
        Conversation conversation = new Conversation("Assets/Conversations/conv_test.txt", 0, 100);
        Add(conversation);



    }

    public override void Update(GameTime gameTime)
    {
        playingState.Update(gameTime);
        base.Update(gameTime);
        
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        playingState.Draw(gameTime, spriteBatch);
        base.Draw(gameTime, spriteBatch);
    }

}

