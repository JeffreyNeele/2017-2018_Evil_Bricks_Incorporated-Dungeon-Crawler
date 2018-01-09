using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

abstract partial class Character : AnimatedGameObject
{


    //Method for character input (both xbox controller and keyboard), for now dummy keys for 1 controller are inserted, but the idea should be clear
    //TO DO: a way to distinguish characters / players from each other.
    public override void HandleInput(InputHelper inputHelper)
    {
        Vector2 previousPosition = this.position;

        if (!IsDowned && !isOnIce && !blockinput)
        {
            velocity = Vector2.Zero;
            //Input keys for basic AA and abilities
            if (inputHelper.KeyPressed(keyboardControls[Keys.Q]))
                this.weapon.Attack(GameWorld.Find("monsterLIST") as GameObjectList, GameWorld.Find("TileField") as GameObjectGrid);
            if (inputHelper.KeyPressed(keyboardControls[Keys.R]))
                this.weapon.UseMainAbility(GameWorld.Find("monsterLIST") as GameObjectList, GameWorld.Find("TileField") as GameObjectGrid);
            if (inputHelper.KeyPressed(keyboardControls[Keys.T]))
                this.weapon.UseSpecialAbility(GameWorld.Find("monsterLIST") as GameObjectList);

            if(xboxControlled)
            {
                HandleInputXboxController(inputHelper);
            }
            else if (keyboardControlled)
            {
                HandleKeyboardInput(inputHelper);
            }

        }
        else if (!IsDowned && isOnIce)
        {
            KeyboardHandleIceMovement(inputHelper);
        }

        if (!SolidCollisionChecker())
        {
            this.iceSpeed = new Vector2(0, 0);
            this.position = previousPosition;
            PlayAnimation("idle");
            blockinput = false;
        }
        walkingdirection = inputHelper.WalkingDirection(playerNumber) * this.movementSpeed;
        PlayAnimationDirection(walkingdirection);

        base.HandleInput(inputHelper);

    }



}
