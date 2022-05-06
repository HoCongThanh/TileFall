using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum
{
    unsafe class RespawnSystem : SystemSignalsOnly,
        ISignalOnPlayerRespawn
    {
        public void OnPlayerRespawn(Frame f, EntityRef playerEntity)
        {
            if(!f.Has<PlayerType>(playerEntity))
            {
                return;
            }

            if(f.Unsafe.TryGetPointer<RespawnComp>(playerEntity, out var respawnComp))
            {
                if(respawnComp->checkPoint == EntityRef.None)
                {
                    Log.Info("player dont have checkpoint, cannot respawn");
                    return;
                }

                Log.Info("OnPlayerRespawn Has RespawnComp");

                if (f.Unsafe.TryGetPointer<CheckPoint>(respawnComp->checkPoint, out var checkPointComp))
                {
                    Log.Info("OnPlayerRespawn Has CheckPoint");
                    checkPointComp->RespawnPlayer(f, playerEntity);
                }
            }
        }
    }
}
