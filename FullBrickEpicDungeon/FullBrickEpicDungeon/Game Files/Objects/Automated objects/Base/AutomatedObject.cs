using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

abstract class AutomatedObject : SpriteGameObject
{
    protected Timer reActiveTimer, duration, setupTimer; //timers will be made in the subclasses. Do note that these timers should not be paused from the start
    protected bool reloaded, triggered;
    protected int trapDamage;
    protected GameObjectList players;
    protected Dictionary<Character, Timer> playersHit;

    public AutomatedObject(string assetname, string id, int sheetIndex, Level level, int damage = 10) : base(assetname, 0)
    {
        reloaded = true;
        triggered = false;
        trapDamage = damage;
        playersHit = new Dictionary<Character, Timer>();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        //Update the timers
        reActiveTimer.Update(gameTime);
        duration.Update(gameTime);
        setupTimer.Update(gameTime);
        UpdatePlayerTrapStatus(gameTime);

        //If the setup is complete, trigger the trap
        if (setupTimer.IsExpired && triggered)
            TriggerTrap();


        //If the trap is triggered, keep track of the duration
        if (duration.IsExpired && setupTimer.IsExpired && !reloaded)
        {
            reloaded = true;
            reActiveTimer.Reset();
            ChangeSpriteIndex(0);
        }

        //If no timers are active, check for targets
        if (reActiveTimer.IsExpired && duration.IsExpired && setupTimer.IsExpired && !triggered && reloaded)
        {
            //If a target is found, the trap is triggered
            if (TargetCheck())
            {
                setupTimer.Reset();
                triggered = true;
            }
        }

        if (Sprite.SheetIndex == 1 && playersHit != null)
            DamagePlayers();
    }

    //The trigger of the trap
    protected virtual void TriggerTrap()
    {
        ChangeSpriteIndex(1);
        duration.Reset();
        reloaded = false;
        triggered = false;
    }

    protected virtual void DamagePlayers()
    {
        players = GameWorld.Find("playerLIST") as GameObjectList;

        foreach (Character player in players.Children)
        {
            if (playersHit.ContainsKey(player))
                continue;

            if (BoundingBox.Intersects(player.BoundingBox))
            {
                player.TakeDamage(trapDamage);
                playersHit.Add(player, new Timer(0.5f));
                playersHit[player].Reset();
            }
        }
    }

    protected virtual void UpdatePlayerTrapStatus(GameTime gameTime)
    {
        players = GameWorld.Find("playerLIST") as GameObjectList;

        foreach (Character player in players.Children)
        {
            if (playersHit.ContainsKey(player))
            {
                playersHit[player].Update(gameTime);
                if (playersHit[player].IsExpired)
                    playersHit.Remove(player);
            }
        }
    }

    //Checks if any players are walking on the trap. If so, activate the setup timer, readying the trap
    protected virtual bool TargetCheck()
    {
        players = GameWorld.Find("playerLIST") as GameObjectList;

        foreach (Character player in players.Children)
        {
            if (BoundingBox.Intersects(player.BoundingBox))
            {
                return true;
            }
        }
        return false;

    }
}

