using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum
{
    public unsafe partial struct RespawnComp
    {
        public void AddCheckPoint(Frame f, EntityRef checkPointEntityRef)
        {
            checkPoint = checkPointEntityRef; 
        }
    }
}
