using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum
{
    public partial struct PlayerComp
    {
        public void Init(Frame f, PlayerRef playerRef)
        {
            this.playerRef = playerRef;
        }
    }
}
