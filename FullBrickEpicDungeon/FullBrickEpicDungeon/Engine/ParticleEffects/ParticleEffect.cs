using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

class ParticleEffect : GameObjectList
{
    SpriteGameObject target;
    GameObjectList particleList;
    public ParticleEffect(SpriteGameObject target, string id = "") : base(0, id)
    {
        this.target = target;
        particleList = new GameObjectList();
    }
}
