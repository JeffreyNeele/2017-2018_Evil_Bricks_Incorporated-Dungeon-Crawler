using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

abstract partial class Character : AnimatedGameObject
{
    protected Dictionary<Keys, Keys> keyboardControls;
    protected bool keyboardControlled;
    protected bool isOnIce = false;
    protected bool isGliding = false;
    protected bool blockinput = false;

    //Method for character input (both xbox controller and keyboard), for now dummy keys for 1 controller are inserted, but the idea should be clear
    //TO DO: a way to distinguish characters / players from each other.
    public override void HandleInput(InputHelper inputHelper)
    {
        if (playerControlled)
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

                if (keyboardControlled)
                {
                    HandleKeyboardMovement(inputHelper);
                }
                else
                {
                    HandleXboxMovement(inputHelper);
                }

            }
            else if (!IsDowned && isOnIce)
            {
                HandleIceMovement(inputHelper);
            }

            if (!SolidCollisionChecker())
            {
                this.iceSpeed = new Vector2(0, 0);
                this.position = previousPosition;
                PlayAnimation("idle");
                blockinput = false;
            }

            base.HandleInput(inputHelper);
        }
    }

    // Method that handles keyboard movement
    public void HandleKeyboardMovement(InputHelper inputHelper)
    {
        if (inputHelper.IsKeyDown(keyboardControls[Keys.W]) || inputHelper.IsKeyDown(keyboardControls[Keys.S]))
        {

            if (inputHelper.IsKeyDown(keyboardControls[Keys.W]))
            {
                if (inputHelper.IsKeyDown(keyboardControls[Keys.A]))
                {
                    this.position += MovementVector(this.movementSpeed, 225);
                    this.PlayAnimation("leftcycle");
                    this.Mirror = false;
                }
                else if (inputHelper.IsKeyDown(keyboardControls[Keys.D]))
                {
                    this.position += MovementVector(this.movementSpeed, 315);
                    this.PlayAnimation("rightcycle");
                    this.Mirror = true;
                }
                else
                {
                    this.position += MovementVector(this.movementSpeed, 270);
                    this.PlayAnimation("backcycle");
                }

            }

            else if (inputHelper.IsKeyDown(keyboardControls[Keys.S]))
            {
                if (inputHelper.IsKeyDown(keyboardControls[Keys.A]))
                {
                    this.position += MovementVector(this.movementSpeed, 135);
                    this.PlayAnimation("leftcycle");
                    this.Mirror = false;
                }
                else if (inputHelper.IsKeyDown(keyboardControls[Keys.D]))
                {
                    this.position += MovementVector(this.movementSpeed, 45);
                    this.PlayAnimation("rightcycle");
                    this.Mirror = true;
                }
                else
                {
                    this.position += MovementVector(this.movementSpeed, 90);
                    this.PlayAnimation("frontcycle");
                }
            }
        }

        else if (inputHelper.IsKeyDown(keyboardControls[Keys.A]))
        {
            this.position += MovementVector(this.movementSpeed, 180);
            this.PlayAnimation("leftcycle");
            this.Mirror = false;
        }

        else if (inputHelper.IsKeyDown(keyboardControls[Keys.D]))
        {
            this.position += MovementVector(this.movementSpeed, 0);
            this.PlayAnimation("rightcycle");
            this.Mirror = true;
        }

        else
        {
            PlayAnimation("idle");
        }

        if (inputHelper.IsKeyDown(keyboardControls[Keys.E])) //Interact key
        {
            ObjectCollisionChecker();
        }
    }

    // Method that handles movement when the character is on ice
    private void HandleIceMovement(InputHelper inputHelper)
    {
        if (blockinput)
        {
            if (this.iceSpeed != new Vector2(0, 0))
                this.position += iceSpeed;
        }
        else
        {
            if (inputHelper.IsKeyDown(keyboardControls[Keys.W]))
            {
                blockinput = true;
                iceSpeed = MovementVector(this.movementSpeed * 3, 270);
                PlayAnimation("backcycle");
            }
            else if (inputHelper.IsKeyDown(keyboardControls[Keys.A]))
            {
                blockinput = true;
                iceSpeed = MovementVector(this.movementSpeed * 3, 180);
                PlayAnimation("leftcycle");
                this.Mirror = false;
            }
            else if (inputHelper.IsKeyDown(keyboardControls[Keys.S]))
            {
                blockinput = true;
                iceSpeed = MovementVector(this.movementSpeed * 3, 90);
                PlayAnimation("frontcycle");
            }
            else if (inputHelper.IsKeyDown(keyboardControls[Keys.D]))
            {
                blockinput = true;
                iceSpeed = MovementVector(this.movementSpeed * 3, 0);
                PlayAnimation("rightcycle");
                this.Mirror = true;
            }
        }
    }

    private void HandleXboxMovement(InputHelper inputHelper)
    {
        if (xboxControls != null) //xboxcontrols zijn niet ingeladen, dus wordt niet door xboxcontroller bestuurd.
        {
            if (inputHelper.ControllerConnected(playerNumber)) //check of controller connected is
            {
                //Attack and Main Ability
                if (inputHelper.ButtonPressed(playerNumber, Buttons.A))
                    this.weapon.Attack(GameWorld.Find("monsterLIST") as GameObjectList, GameWorld.Find("TileField") as GameObjectGrid);
                if (inputHelper.ButtonPressed(playerNumber, Buttons.B))
                    this.weapon.UseMainAbility(GameWorld.Find("monsterLIST") as GameObjectList, GameWorld.Find("TileField") as GameObjectGrid);

                //Interact button
                if (inputHelper.ButtonPressed(playerNumber, Buttons.X))
                    ObjectCollisionChecker();
                //Movement
                walkingdirection = inputHelper.WalkingDirection(playerNumber) * this.movementSpeed;
                walkingdirection.Y = -walkingdirection.Y;
            }
        }
    }
}
