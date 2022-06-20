using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEVOLVE.Core.Settings
{
    [Serializable]
    public class GameSettings
    {
        public int visibleChunks = 8;
        public float minUpdateMoveDistance = 128;
    }
}
