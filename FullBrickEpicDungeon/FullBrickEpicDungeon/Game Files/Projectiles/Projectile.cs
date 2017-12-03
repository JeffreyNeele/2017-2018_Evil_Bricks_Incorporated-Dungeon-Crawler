using System;
using System.Collections.Generic;

class Projectile : SpriteGameObject
{
    protected bool piercing;
    protected Projectile(bool piercing, string assetName) : base (assetName)
    {
        this.piercing = piercing;
    }


}