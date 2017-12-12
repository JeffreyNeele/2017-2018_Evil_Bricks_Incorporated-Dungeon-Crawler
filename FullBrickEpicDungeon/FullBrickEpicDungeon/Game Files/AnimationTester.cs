using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

class AnimationTester : AnimatedGameObject
{
    public AnimationTester() : base(0, "tester")
    {
        this.Position = new Microsoft.Xna.Framework.Vector2(GameEnvironment.Screen.X / 2, GameEnvironment.Screen.Y / 2);
        LoadAnimation("Sprites/Lightbringer/lightbringer_idle@3", "testanimator", true, 0.33F);
    }

    public void TestAnimation()
    {
        PlayAnimation("testanimator");
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        PlayAnimation("testanimator");
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        base.HandleInput(inputHelper);
    }
}