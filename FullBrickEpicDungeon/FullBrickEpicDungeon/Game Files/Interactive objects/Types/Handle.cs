using System;
using Microsoft.Xna.Framework;

class Handle : InteractiveObject
{
    Timer countDownTimer;
    public Handle(string assetname, string id, int sheetIndex) : base(assetname, id, sheetIndex)
    {
        countDownTimer = new Timer(1);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        countDownTimer.Update(gameTime);
        if (countDownTimer.IsExpired)
        {
            this.Reset();
        }
    }
    protected override void Interact(Character targetCharacter)
    {
        this.Sprite.SheetIndex = 1;
        interacting = false;
        StartResetTimer();
    }

    public override void Reset()
    {
        this.sprite.SheetIndex = 0;
        base.Reset();
    }

    protected void StartResetTimer()
    {
        countDownTimer.Reset();
        countDownTimer.IsPaused = false;
    }

}


