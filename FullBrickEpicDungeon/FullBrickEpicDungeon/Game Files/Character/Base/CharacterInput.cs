using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

abstract partial class Character : AnimatedGameObject
{
    protected Dictionary<Keys, Keys> keyboardControls;
    protected bool xboxControlled, isGliding = false, blockinput = false;

    /// <summary>
    /// Method for handling input for the character
    /// </summary>
    public override void HandleInput(InputHelper inputHelper)
    {
        if (playerControlled)
        {
            Vector2 previousPosition = this.position;
            Vector2 previousWalkingDirection = new Vector2(0, 0);
            if (this.xboxControlled && !inputHelper.ControllerConnected(controllerNumber))
            {
                FullBrickEpicDungeon.DungeonCrawler.mouseVisible = true;
                // will replace with another gamestate that tells you to reconnect your controller
                GameEnvironment.GameStateManager.SwitchTo("pauseState");
            }

            // Handle normal movement
            if (!IsDowned && !IsOnIceChecker())
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
                // Play Animations
                PlayAnimationDirection(walkingdirection);
                weapon.SwordDirectionCheckerManager(walkingdirection);
                walkingdirection = Vector2.Zero;
                base.HandleInput(inputHelper);
            }
            // Handle ice movement
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
                if (!SolidCollisionChecker())
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

    /// <summary>
    /// Method that handles the keyboard input for the character
    /// </summary>
    public void HandleKeyboardMovement(InputHelper inputHelper)
    {
        if (inputHelper.KeyPressed(keyboardControls[Keys.Q]))
        {
            weapon.IsAttacking = true;
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
                weapon.IsAttacking = true;
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

    /// <summary>
    /// Method for handling keyboard movement on ice
    /// </summary>
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

    /// <summary>
    /// Handles the XBOX movement for the player
    /// </summary>
    private void HandleXboxMovement(InputHelper inputHelper)
    {
        if (inputHelper.ControllerConnected(controllerNumber)) //Check if the controller is connected
        {
            if (inputHelper.ButtonPressed(controllerNumber, Buttons.A))
            {
                weapon.IsAttacking = true;
                this.weapon.Attack(GameWorld.Find("monsterLIST") as GameObjectList, GameWorld.Find("TileField") as GameObjectGrid);
                if (weapon.PreviousAttackHit)
                    PlaySFX("attack_hit");
                else
                    PlaySFX("attack_miss");
            }
            if (inputHelper.ButtonPressed(controllerNumber, Buttons.B))
            {
                if (!weapon.AbilityMain.IsOnCooldown)
                {
                    weapon.IsAttacking = true;
                    this.weapon.UseMainAbility(GameWorld.Find("monsterLIST") as GameObjectList, GameWorld.Find("TileField") as GameObjectGrid);
                    PlaySFX("basic_ability");
                }
                else
                {
                    PlaySFX("ability_not_ready");
                }
            }
            //Interact button
            if (inputHelper.ButtonPressed(controllerNumber, Buttons.X))
                InteractCollisionChecker();
            if (inputHelper.ButtonPressed(controllerNumber, Buttons.Y))
            {
                SwitchtoAIChecker();
                return;
            }
            //Movement
            walkingdirection = inputHelper.WalkingDirection(controllerNumber) * this.movementSpeed;
            walkingdirection.Y = -walkingdirection.Y;
        }

    }

    /// <summary>
    /// Method that handles XBOX input while the character is on ice
    /// </summary>
    private void HandleXboxIceMovement(InputHelper inputHelper)
    {
        // If the input is blocked we keep sliding
        if (blockinput)
        {
            if (this.iceSpeed != new Vector2(0, 0))
                this.position += iceSpeed;
        }
        else
        {
            walkingdirection = inputHelper.WalkingDirection(controllerNumber) * this.movementSpeed;
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
        // play animations for the ice sliding direction;
        PlayAnimationDirection(iceSpeed);
    }

    /// <summary>
    /// Checks if there is a targetable Character to switch to, and if there is it switches to that character
    /// </summary>
    public void SwitchtoAIChecker()
    {
        GameObjectList playerList = GameWorld.Find("playerLIST") as GameObjectList;
        int targetPlayerNumber = this.playerNumber + 1;
        for (int i = 1; i < 4; i++)
        {
            // There are  only 4 players, so if the target is 5 we go back to 1
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

    /// <summary>
    /// Method that gives the original player control over the target character
    /// </summary>
    /// <param name="targetCharacter">The character that the player will switch to control</param>
    public void SwitchToCharacter(Character targetCharacter)
    {
        // If the target character is the owner himself, or the target character is on a wrong position (due to how AI is handled) the switch will not commence
        if (targetCharacter == this || !targetCharacter.SolidCollisionChecker() || targetCharacter.IsOnIceChecker())
        {
            PlaySFX("switch_wrong");
            return;
        }
        else
        {
            // Switch control schemes
            if (this.xboxControlled)
            {
                targetCharacter.xboxControlled = true;
                targetCharacter.controllerNumber = this.controllerNumber;
                this.controllerNumber = this.playerNumber;
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


    /// <summary>
    /// Method that plays the correct animation for the direction the character is walking in
    /// </summary>
    /// <param name="walkingdirection"></param>
    public void PlayAnimationDirection(Vector2 walkingdirection)
    {
        // The attack animations have priority over the walking animations, and thus if these are being played we just return from the method
        if (!weapon.IsAttacking)
        {
            if(walkingdirection == Vector2.Zero)
            {
                this.PlayAnimation("idle");
            }
            else if (Math.Abs(walkingdirection.X) >= Math.Abs(walkingdirection.Y))
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
            else
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
        }
    }

    public Vector2 WalkingDirection
    {
        get { return walkingdirection; }
    }
}
