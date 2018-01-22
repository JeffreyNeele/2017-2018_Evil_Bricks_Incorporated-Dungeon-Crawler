using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class SpikeTrap : AutomatedObject
{
    public SpikeTrap(string assetname, string id, int sheetIndex, Level level) : base(assetname, id, sheetIndex, level)
    {
        duration = new Timer(2.0f);
        duration.IsPaused = false;
        setupTimer = new Timer(0.2f);
        setupTimer.IsPaused = false;
    }
}