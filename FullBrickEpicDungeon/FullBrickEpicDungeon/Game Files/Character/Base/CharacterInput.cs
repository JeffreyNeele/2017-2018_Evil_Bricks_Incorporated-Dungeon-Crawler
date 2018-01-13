using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

abstract partial class Character : AnimatedGameObject
{
    protected Dictionary<Keys, Keys> keyboardControls;
    protected bool xboxControlled, isOnIce = false, isGliding = false, blockinput = false;

    //Method for character input (both xbox controller and keyboard), for now dummy keys for 1 controller are inserted, but the idea should be clear
    //TO DO: a way to distinguish characters / players from each other.
    public override void HandleInput(InputHelper inputHelper)
    {
        if (playerControlled)
        {
            Vector2 previousPosition = this.position;
            Vector2 previousWalkingDirection = new Vector2(0, 0);
            if(this.xboxControlled && !inputHelper.ControllerConnected(relativePlayerNumber))
            {
                FullBrickEpicDungeon.DungeonCrawler.mouseVisible = true;
                // will replace with another gamestate that tells you to reconnect your controller
                GameEnvironment.GameStateManager.SwitchTo("pauseState");
            } 

            if (!IsDowned && !isOnIce)
            {
                velocity = Vector2.Zero;
                if (xboxControlled)
                {
                    HandleXboxMovement(inputHelper);
                }
                else
                {
                    HandleKeyboardMovement(inputHelper);
                }
                this.position += walkingdirection;
                
                // Play walking SFX
                if (walkingdirection != Vector2.Zero && stepSoundTimer.IsExpired)
                {
                    PlaySFX("walk");
                    stepSoundTimer.Reset();
                }
                previousWalkingDirection = walkingdirection;
                PlayAnimationDirection(walkingdirection);
                walkingdirection = Vector2.Zero;
                base.HandleInput(inputHelper);
            }
            else if (!IsDowned)
            {
                if (xboxControlled)
                {
                    HandleXboxIceMovement(inputHelper);
                }
                else
                {
                    HandleKeyboardIceMovement(inputHelper);
                }

                if (iceSpeed != Vector2.Zero && stepSoundTimer.IsExpired)
                {
                    PlaySFX("ice_slide");
                    stepSoundTimer.Reset();
                }
                base.HandleInput(inputHelper);
            }

            //Check if maiden collides with solid object, else adjust the character position
            if (!SolidCollisionChecker())
            {
                this.iceSpeed = new Vector2(0, 0);

                //Check if collision is comes from y value
                this.position.X = previousPosition.X;
                //if there still is collision, recreate old position and check for x value
                if(!SolidCollisionChecker())
                {
                    this.position.X += previousWalkingDirection.X;
                    this.position.Y = previousPosition.Y;
                    if (!SolidCollisionChecker())
                    {
                        this.position = previousPosition;
                        PlayAnimation("idle");
                    }
                }
                blockinput = false;
            }
        }
    }

    // Method that handles keyboard movement
    public void HandleKeyboardMovement(InputHelper inputHelper)
    {
        if (inputHelper.KeyPressed(keyboardControls[Keys.Q]))
        {
            this.weapon.Attack(GameWorld.Find("monsterLIST") as GameObjectList, GameWorld.Find("TileField") as GameObjectGrid);
            if (weapon.PreviousAttackHit)
                PlaySFX("attack_hit");
            else
                PlaySFX("attack_miss");

        } 
        if (inputHelper.KeyPressed(keyboardControls[Keys.R]))
        {
            if (!weapon.AbilityMain.IsOnCooldown)
            {
                this.weapon.UseMainAbility(GameWorld.Find("monsterLIST") as GameObjectList, GameWorld.Find("TileField") as GameObjectGrid);
                PlaySFX("basic_ability");
            }
            else
            {
                PlaySFX("ability_not_ready");
            }
        }
        if (inputHelper.KeyPressed(keyboardControls[Keys.LeftShift]))
        {
            SwitchtoAIChecker();
            return;
        }
        if (inputHelper.IsKeyDown(keyboardControls[Keys.W]))
        {
            if (inputHelper.IsKeyDown(keyboardControls[Keys.A]))
            {
                walkingdirection = MovementVector(this.movementSpeed, 225);
            }
            else if (inputHelper.IsKeyDown(keyboardControls[Keys.D]))
            {
                walkingdirection = MovementVector(this.movementSpeed, 315);
            }
            else
            {
                walkingdirection = MovementVector(this.movementSpeed, 270);
            }

        }

        else if (inputHelper.IsKeyDown(keyboardControls[Keys.S]))
        {
            if (inputHelper.IsKeyDown(keyboardControls[Keys.A]))
            {
                walkingdirection = MovementVector(this.movementSpeed, 135);
            }
            else if (inputHelper.IsKeyDown(keyboardControls[Keys.D]))
            {
                walkingdirection = MovementVector(this.movementSpeed, 45);

            }
            else
            {
                walkingdirection = MovementVector(this.movementSpeed, 90);
            }
        }

        else if (inputHelper.IsKeyDown(keyboardControls[Keys.A]))
        {
            walkingdirection = MovementVector(this.movementSpeed, 180);
        }

        else if (inputHelper.IsKeyDown(keyboardControls[Keys.D]))
        {
            walkingdirection = MovementVector(this.movementSpeed, 0);
        }

        if (inputHelper.IsKeyDown(keyboardControls[Keys.E])) 
        {
            InteractCollisionChecker();
        }

    }

    // Method that handles keyboard movement when the character is on ice
    private void HandleKeyboardIceMovement(InputHelper inputHelper)
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
                iceSpeed = MovementVector(this.movementSpeed, 270);
            }
            else if (inputHelper.IsKeyDown(keyboardControls[Keys.A]))
            {
                blockinput = true;
                iceSpeed = MovementVector(this.movementSpeed, 180);
                this.Mirror = false;
            }
            else if (inputHelper.IsKeyDown(keyboardControls[Keys.S]))
            {
                blockinput = true;
                iceSpeed = MovementVector(this.movementSpeed, 90);
            }
            else if (inputHelper.IsKeyDown(keyboardControls[Keys.D]))
            {
                blockinput = true;
                iceSpeed = MovementVector(this.movementSpeed, 0);
                this.Mirror = true;
            }
        }
        PlayAnimationDirection(iceSpeed);
    }

    // Method that handles xbox movement and interaction
    private void HandleXboxMovement(InputHelper inputHelper)
    {
        if (xboxControls != null) //xboxcontrols zijn niet ingeladen, dus wordt niet door xboxcontroller bestuurd.
        {
            if (inputHelper.ControllerConnected(relativePlayerNumber)) //check of controller connected is
            {
                //Attack and Main Ability
                if (inputHelper.ButtonPressed(relativePlayerNumber, xboxControls[Buttons.A]))
                {
                    this.weapon.Attack(GameWorld.Find("monsterLIST") as GameObjectList, GameWorld.Find("TileField") as GameObjectGrid);
                    if (weapon.PreviousAttackHit)
                        PlaySFX("attack_hit");
                    else
                        PlaySFX("attack_miss");
                }
                if (inputHelper.ButtonPressed(relativePlayerNumber, xboxControls[Buttons.B]))
                {
                    if (!weapon.AbilityMain.IsOnCooldown)
                    {
                        this.weapon.UseMainAbility(GameWorld.Find("monsterLIST") as GameObjectList, GameWorld.Find("TileField") as GameObjectGrid);
                        PlaySFX("basic_ability");
                    }
                    else
                    {
                        PlaySFX("ability_not_ready");
                    }
                }
                //Interact button
                if (inputHelper.ButtonPressed(relativePlayerNumber, xboxControls[Buttons.X]))
                    InteractCollisionChecker();
                if (inputHelper.ButtonPressed(relativePlayerNumber, xboxControls[Buttons.Y]))
                {
                    SwitchtoAIChecker();
                    return;
                }
                //Movement
                walkingdirection = inputHelper.WalkingDirection(relativePlayerNumber) * this.movementSpeed;
                walkingdirection.Y = -walkingdirection.Y;
            }
        }
    }

    // Method that handles xbox movement when the character is on ice
    private void HandleXboxIceMovement(InputHelper inputHelper)
    {
        if (blockinput)
        {
            if (this.iceSpeed != new Vector2(0, 0))
                this.position += iceSpeed;
        }
        else {
            walkingdirection = inputHelper.WalkingDirection(relativePlayerNumber) * this.movementSpeed;
            walkingdirection.Y = -walkingdirection.Y;
            if (Math.Abs(walkingdirection.X) >= Math.Abs(walkingdirection.Y))
            {
                if (walkingdirection.X > 0)
                {
                    blockinput = true;
                    iceSpeed = MovementVector(this.movementSpeed, 0);
                    this.Mirror = true;
                }
                else if (walkingdirection.X < 0)
                {
                    blockinput = true;
                    iceSpeed = MovementVector(this.movementSpeed, 180);
                    this.Mirror = false;
                }
            }
            else if (Math.Abs(walkingdirection.Y) > Math.Abs(walkingdirection.X))
            {
                if (walkingdirection.Y > 0)
                {
                    blockinput = true;
                    iceSpeed = MovementVector(this.movementSpeed, 90);
                }
                else if (walkingdirection.Y < 0)
                {
                    blockinput = true;
                    iceSpeed = MovementVector(this.movementSpeed, 270);
                }

            }
        }
        PlayAnimationDirection(iceSpeed);        
    }


    public void SwitchtoAIChecker()
    {
        GameObjectList playerList = GameWorld.Find("playerLIST") as GameObjectList;
        int targetPlayerNumber = this.playerNumber + 1;
        for(int i = 1; i < 4; i++)
        {
            if (targetPlayerNumber > 4)
            {
                targetPlayerNumber = 1;
            }

            foreach (Character p in playerList.Children)
            {
                if (p.playerNumber == targetPlayerNumber)
                {
                    if (!p.PlayerControlled)
                    {
                        SwitchToCharacter(p);
                        return;
                    }
                }
            }
            targetPlayerNumber++;
        }
    }
    public void SwitchToCharacter(Character targetCharacter)
    {
        if (targetCharacter == this)
        {
            PlaySFX("switch_wrong");
            return;
        }
        else
        {
            if (this.xboxControlled)
            {
                targetCharacter.xboxControlled = true;
                targetCharacter.xboxControls = this.xboxControls;
                targetCharacter.relativePlayerNumber = this.relativePlayerNumber;
                this.xboxControls = null;
                this.relativePlayerNumber = this.playerNumber;
            }
            else
            {
                targetCharacter.xboxControlled = false;
                targetCharacter.keyboardControls = this.keyboardControls;
                this.keyboardControls = null;
            }
            this.playerControlled = false;
            targetCharacter.playerControlled = true;
        }
    }


    //when called with the walkingdirection, it plays the correct animation with the movement.
    public void PlayAnimationDirection(Vector2 walkingdirection)
    {
        if (Math.Abs(walkingdirection.X) > Math.Abs(walkingdirection.Y))
        {
            if (walkingdirection.X > 0)
            {
                this.PlayAnimation("rightcycle");
                this.Mirror = true;
            }
            else if (walkingdirection.X < 0)
            {
                this.PlayAnimation("leftcycle");
                this.Mirror = false;
            }
        }
        else if (Math.Abs(walkingdirection.Y) > Math.Abs(walkingdirection.X))
        {
            if (walkingdirection.Y > 0)
            {
                this.PlayAnimation("frontcycle");
            }
            else if (walkingdirection.Y < 0)
            {
                this.PlayAnimation("backcycle");
            }
        }
        else
            this.PlayAnimation("idle");
    }
}
