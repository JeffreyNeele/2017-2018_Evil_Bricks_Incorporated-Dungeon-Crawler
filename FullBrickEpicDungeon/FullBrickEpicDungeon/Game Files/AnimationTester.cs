using System;
using System.Collections.Generic;


class AnimationTester : AnimatedGameObject
{
    public AnimationTester() : base(0, "tester")
    {
        this.Position = new Microsoft.Xna.Framework.Vector2(GameEnvironment.Screen.X / 2, GameEnvironment.Screen.Y / 2);
        LoadAnimation("Sprites/Lightbringer/lightbringer_idle@3", "testanimator", true, 0.33F);
    }

    public void TestAnimation()
    {
        PlayAnimation("testanimator");;
    }
}