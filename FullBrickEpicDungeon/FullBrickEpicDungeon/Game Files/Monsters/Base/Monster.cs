﻿using System;
using System.Collections.Generic;

// NOTE: the gold attribute for the monster is seen as the amount of gold the player gets when it is defeated.
abstract class Monster : SpriteGameObject
{
    BaseAttributes attributes;
    public Monster(string assetName, string type) : base(assetName)
    {
        attributes = new BaseAttributes(type);
    }

    public BaseAttributes Attributes
    {
        get { return attributes; }
        set { attributes = value; }
    }
}