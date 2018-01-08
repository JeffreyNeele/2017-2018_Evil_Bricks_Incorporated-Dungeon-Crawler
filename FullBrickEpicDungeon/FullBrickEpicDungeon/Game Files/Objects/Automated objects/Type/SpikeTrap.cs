using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class SpikeTrap : AutomatedObject
{
    public SpikeTrap(string assetname, string id, int sheetIndex, Level level) : base(assetname, id, sheetIndex, level)
    {
        reActiveTimer = new Timer(1.0f);
        reActiveTimer.IsPaused = false;
        duration = new Timer(2.0f);
        duration.IsPaused = false;
        setupTimer = new Timer(0.5f);
        setupTimer.IsPaused = false;
    }
}

