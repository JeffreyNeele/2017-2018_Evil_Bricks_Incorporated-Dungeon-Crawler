﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;

public class AnimatedGameObject : SpriteGameObject
{
    protected Dictionary<string,Animation> animations;
    public AnimatedGameObject(int layer = 0, string id = "")
        : base("", layer, id)
    {
        animations = new Dictionary<string, Animation>();
    }
    
   
    public void LoadAnimation(string assetName, string id, bool looping, 
                              float frameTime = 0.1f)
    {
        Animation anim = new Animation(assetName, looping, frameTime);
        animations[id] = anim;        
    }

    public void PlayAnimation(string id)
    {
        if (sprite == animations[id])
        {
            return;
        }
        if (sprite != null)
        {
            animations[id].Mirror = sprite.Mirror;
        }
        animations[id].Play();
        sprite = animations[id];
        origin = new Vector2(sprite.Width / 2, sprite.Height);        
    }

    public override void Update(GameTime gameTime)
    {
        if (sprite == null)
        {
            return;
        }
        CurrentAnimation.Update(gameTime);
        base.Update(gameTime);
    }

    public Animation CurrentAnimation
    {
        get { return sprite as Animation; }
    }


}