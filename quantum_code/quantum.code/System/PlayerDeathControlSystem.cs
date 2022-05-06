using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum
{
    class PlayerDeathControlSystem : SystemSignalsOnly,
        ISignalOnObjectEnterDeathArea
    {
        public void OnObjectEnterDeathArea(Frame f, EntityRef objEntity)
        {
            Log.Info("OnObjectEnterDeathArea");
            if(f.Has<PlayerType>(objEntity))
            {
                Log.Info("OnObjectEnterDeathArea has PlayerType");
                f.Signals.OnPlayerRespawn(objEntity);
            }
        }
    }
}
