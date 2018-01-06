using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

class LittlePenguin : AImonster
{
    float slideSpeed;
    public LittlePenguin(Level currentLevel) : base(0f, currentLevel, "LittlePenguin")
    {
        this.baseattributes.HP = 80;
        this.baseattributes.Armour = 0;
        this.baseattributes.Attack = 0;
        this.baseattributes.Gold = 75;
        attributes = baseattributes;
        slideSpeed = 100f;
        //Load de animations in, voor nu Bunny genomen als test
        LoadAnimation("Assets/Sprites/Enemies/bunny_default", "idle", false);
        PlayAnimation("idle");
    }

    //Draft method for sliding direction
    public void slideDirection()
    {
        //Take the position of the target
        GameObjectList players = GameWorld.Find("playerLIST") as GameObjectList;
    }

    //Draft method for checking if the penguin collides with a wall
    public bool CheckCollision()
    {
        //Take the grid to check for all tiles that are solid or doors
        GameObjectGrid field = GameWorld.Find("TileField") as GameObjectGrid;

        return false;
    }


}

