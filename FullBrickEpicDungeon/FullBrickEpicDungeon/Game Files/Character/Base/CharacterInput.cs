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
        Vector2 previousPosition = this.position;

        if (!IsDowned && !isOnIce && !blockinput)
        {
            velocity = Vector2.Zero;
            //Input keys for basic AA and abilities
            

            if (keyboardControlled)
            {
                HandleKeyboardInput(inputHelper);
            }
            else
            {
                // Add the xbox method here...
            }
        }
        // NOTE: the Ice method has to be updated to account for XBOX controls, maybe with a ||, but this will be a problem as keyboardcontrols will be null if a controller is used
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

    // Method that handles keyboard movement
    public void HandleKeyboardInput(InputHelper inputHelper)
    {
        if (inputHelper.KeyPressed(keyboardControls[Keys.Q]))
            this.weapon.Attack(GameWorld.Find("monsterLIST") as GameObjectList);
        if (inputHelper.KeyPressed(keyboardControls[Keys.R]))
            this.weapon.UseMainAbility(GameWorld.Find("monsterLIST") as GameObjectList);
        if (inputHelper.KeyPressed(keyboardControls[Keys.T]))
            this.weapon.UseSpecialAbility(GameWorld.Find("monsterLIST") as GameObjectList);

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
    public void HandleIceMovement(InputHelper inputHelper)
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
}
